using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRay : MonoBehaviour
{ 
    [SerializeField] float Distance; //длина или дистанция луча 

    public List<Block> blocks = new List<Block>(); //запоминание блоков, которые попали под блоки 
    public int index;

    Vector3 PosRay;     //начало луча

    void Start()
    {
        float x = GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f * transform.localScale.x + 0.1f; //смещение относительно центра блока и где должен находить луч по х 
        PosRay = transform.position + new Vector3(x, 0, 0);
    }

    void Update()
    {
        //Debug.DrawLine(PosRay, PosRay + new Vector3(Distance, 0, 0), Color.red); //отображение луча 
    }

    public void RunRay() //запуск луча 
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(PosRay, Vector2.right, Distance);

        if(hits.Length > 0) //проверяет, коснулся ли луч блока 
        {
            blocks.Clear();
            for (int i = 0; i < hits.Length; i++) 
            {
                if (hits[i].collider.CompareTag("Block"))
                {
                    blocks.Add(hits[i].collider.GetComponent<Block>()); //добавление блоков в список 
                }
            }
        }

        //print($"{gameObject.name} - {blocks.Count}");
    } 

    public void RemoveLine() //ф-ция для удаления всего объекта 
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            Destroy(blocks[i].gameObject);
        }

        blocks.Clear();
    }
}
