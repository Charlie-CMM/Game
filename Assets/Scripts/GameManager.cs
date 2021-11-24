using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform SpawnPoint; //реализация точки спавна 
    [SerializeField] GameObject[] ShapePrefabs;
    [SerializeField] BlockRay[] Lines;

    [SerializeField] Text TextScore; // вывод очков
    private int score = 0; //создание поля для очков 

    Vector3 offset;

    void Start()
    {
        score = 0; //обнуление счета 
        TextScore.text = $"Счет: {score}"; 

        offset = new Vector3(0, -0.5f, 0); //опускаем фигуры вниз

        for (int i = 0; i < Lines.Length; i++) //перебор линий, определение порядкого номера 
        {
            Lines[i].index = i;
        }
        SpawnNextShape();
    }

    void Update()
    {
        
    }

    public void SpawnNextShape()    //спавнер фигур
    {
        int index = Random.Range(0, ShapePrefabs.Length);  //рандомайзер фигур, от 0 и до длины массивы (5 - кол-во фигур)

        GameObject shape = Instantiate(ShapePrefabs[index], SpawnPoint.position, Quaternion.identity); //ф-ция которая из префаба создает объект  

        index = Random.Range(0, 4); //ф-ция спавнитс случайно повернутым образом
        shape.transform.eulerAngles = new Vector3(0, 0, 90 * index);
    }

    public void RunRays() //запуск лучей 
    {
        for (int i = 0; i < Lines.Length; i++)
        {
            Lines[i].RunRay();
        }

        CheckLines();
    }

    void CheckLines()    //перебор линий (сколько блоков в ряду)
    {
        for (int i = 0; i < Lines.Length; i++)
        {
            if (Lines[i].blocks.Count == 11)
            {
                //print("ряд полный");
                Lines[i].RemoveLine();//удаление ряда 
                score++;
                TextScore.text = $"Счет: {score}";
                DropDuwn(i);
                break;
            }
        }
    }

    void DropDuwn(int startIndex) //опускание фигур с текущего индекса 
    {
        for (int i = startIndex + 1; i < Lines.Length; i++)
        {
            BlockRay Line = Lines[i];
            for (int k = 0; k < Line.blocks.Count; k++) //перебор всех блоков внутри линии 
            {
                Line.blocks[k].gameObject.transform.position += offset; //смещение блоков вниз
            }
        }

        RunRays();
    }
}
