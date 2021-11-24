using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform SpawnPoint; //���������� ����� ������ 
    [SerializeField] GameObject[] ShapePrefabs;
    [SerializeField] BlockRay[] Lines;

    [SerializeField] Text TextScore; // ����� �����
    private int score = 0; //�������� ���� ��� ����� 

    Vector3 offset;

    void Start()
    {
        score = 0; //��������� ����� 
        TextScore.text = $"����: {score}"; 

        offset = new Vector3(0, -0.5f, 0); //�������� ������ ����

        for (int i = 0; i < Lines.Length; i++) //������� �����, ����������� ��������� ������ 
        {
            Lines[i].index = i;
        }
        SpawnNextShape();
    }

    void Update()
    {
        
    }

    public void SpawnNextShape()    //������� �����
    {
        int index = Random.Range(0, ShapePrefabs.Length);  //����������� �����, �� 0 � �� ����� ������� (5 - ���-�� �����)

        GameObject shape = Instantiate(ShapePrefabs[index], SpawnPoint.position, Quaternion.identity); //�-��� ������� �� ������� ������� ������  

        index = Random.Range(0, 4); //�-��� �������� �������� ���������� �������
        shape.transform.eulerAngles = new Vector3(0, 0, 90 * index);
    }

    public void RunRays() //������ ����� 
    {
        for (int i = 0; i < Lines.Length; i++)
        {
            Lines[i].RunRay();
        }

        CheckLines();
    }

    void CheckLines()    //������� ����� (������� ������ � ����)
    {
        for (int i = 0; i < Lines.Length; i++)
        {
            if (Lines[i].blocks.Count == 11)
            {
                //print("��� ������");
                Lines[i].RemoveLine();//�������� ���� 
                score++;
                TextScore.text = $"����: {score}";
                DropDuwn(i);
                break;
            }
        }
    }

    void DropDuwn(int startIndex) //��������� ����� � �������� ������� 
    {
        for (int i = startIndex + 1; i < Lines.Length; i++)
        {
            BlockRay Line = Lines[i];
            for (int k = 0; k < Line.blocks.Count; k++) //������� ���� ������ ������ ����� 
            {
                Line.blocks[k].gameObject.transform.position += offset; //�������� ������ ����
            }
        }

        RunRays();
    }
}
