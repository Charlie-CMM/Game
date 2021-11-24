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

    [SerializeField] int speedIndex = 1; //увеличение индекса 

    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map").transform; // родитель Map
        moveCoroutine = StartCoroutine(MoveDown());
    }

    void Update()
    {
        if (isMoving) //если ф-ция верна то мы можем двигаться 
        {
            time += Time.deltaTime;

            Move();
        }
    }

    void Move() //движение фигуры на стерочку (исправление бага 1)
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
            //y = -0.5f; //перемещаем не стрелочкой, а только увеличиваем скорость смещения 
            speedIndex = 4;
            if (time >= 1f / speedIndex)
            RestartCoroutine();
        }
        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            speedIndex = 1;
            //RestartCoroutine();
        }

        transform.Translate(x, y, 0, Space.World); //добавление глобальное направление 
    } 

    private void LateUpdate()
    {
        RotateShape();  
    }

    IEnumerator MoveDown() //движение фигуры
    {
        while (true)
        {
            time = 0; //сброс счетчика времени 
            transform.Translate(0, -0.5f, 0, Space.World); //опускание фигуры вниз 
            yield return new WaitForSeconds(1f / speedIndex); //время между повторениями рутины
        }
    }

    void RestartCoroutine()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); //остановку рутины
            moveCoroutine = StartCoroutine(MoveDown());  //перезапуск рутины 
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

    void StopMovement()  //ф-ция для остановки фигуры снизу 
    {
        if (isMoving == false) return;

        isMoving = false;

        if (moveCoroutine != null)
        {
            //print("stop"); //проверка
            StopCoroutine(moveCoroutine);
            moveCoroutine = null; // обнуление рутины 
        }

        while(transform.childCount > 0) //проверка дочерних элементов 
        {
            GameObject g = transform.GetChild(0).gameObject; //блок с индексом 0
            g.GetComponent<Block>().isMoving = false;
            g.transform.SetParent(map); //меняем родительский объект, родитель стад Map
            g.transform.eulerAngles = Vector3.zero; //изменение угла фигуры  
            g.tag = "Block";
        }

        FindObjectOfType<GameManager>().RunRays();

        FindObjectOfType<GameManager>().SpawnNextShape(); //вызов новой фигуры 

        Destroy(gameObject); //уничтожение фигуры 
    }
}
