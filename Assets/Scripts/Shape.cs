using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    Coroutine moveCoroutine = null;

    bool canLaft = true;
    bool canRight = true;

    bool isMoving = true;

    float time = 0f;

    Transform map;

    [SerializeField] int speedIndex = 1; //���������� ������� 

    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map").transform; // �������� Map
        moveCoroutine = StartCoroutine(MoveDown());
    }

    void Update()
    {
        if (isMoving) //���� �-��� ����� �� �� ����� ��������� 
        {
            time += Time.deltaTime;

            Move();
        }
    }

    void Move() //�������� ������ �� �������� (����������� ���� 1)
    {
        float x = 0;
        float y = 0;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && canLaft)
        {
            x = -0.5f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && canRight)
        {
            x = 0.5f;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            //y = -0.5f; //���������� �� ����������, � ������ ����������� �������� �������� 
            speedIndex = 4;
            if (time >= 1f / speedIndex)
            RestartCoroutine();
        }
        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            speedIndex = 1;
            //RestartCoroutine();
        }

        transform.Translate(x, y, 0, Space.World); //���������� ���������� ����������� 
    } 

    private void LateUpdate()
    {
        RotateShape();  
    }

    IEnumerator MoveDown() //�������� ������
    {
        while (true)
        {
            time = 0; //����� �������� ������� 
            transform.Translate(0, -0.5f, 0, Space.World); //��������� ������ ���� 
            yield return new WaitForSeconds(1f / speedIndex); //����� ����� ������������ ������
        }
    }

    void RestartCoroutine()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); //��������� ������
            moveCoroutine = StartCoroutine(MoveDown());  //���������� ������ 
        }
    }

    void RotateShape()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, -90);
        }
    }

    internal void BorderColided(string tag)
    {
        switch (tag)
        {
            case "LeftBorder":
                canLaft = false;
                break;

            case "RightBorder":
                canRight = false;
                break;

            case "BottomBorder":
            case "Block":
                StopMovement();
                break;

            default:
                canLaft = true;
                canRight = true;
                break;
        }
    }

    void StopMovement()  //�-��� ��� ��������� ������ ����� 
    {
        if (isMoving == false) return;

        isMoving = false;

        if (moveCoroutine != null)
        {
            //print("stop"); //��������
            StopCoroutine(moveCoroutine);
            moveCoroutine = null; // ��������� ������ 
        }

        while(transform.childCount > 0) //�������� �������� ��������� 
        {
            GameObject g = transform.GetChild(0).gameObject; //���� � �������� 0
            g.GetComponent<Block>().isMoving = false;
            g.transform.SetParent(map); //������ ������������ ������, �������� ���� Map
            g.transform.eulerAngles = Vector3.zero; //��������� ���� ������  
            g.tag = "Block";
        }

        FindObjectOfType<GameManager>().RunRays();

        FindObjectOfType<GameManager>().SpawnNextShape(); //����� ����� ������ 

        Destroy(gameObject); //����������� ������ 
    }
}
