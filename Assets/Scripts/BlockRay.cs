using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRay : MonoBehaviour
{ 
    [SerializeField] float Distance; //����� ��� ��������� ���� 

    public List<Block> blocks = new List<Block>(); //����������� ������, ������� ������ ��� ����� 
    public int index;

    Vector3 PosRay;     //������ ����

    void Start()
    {
        float x = GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f * transform.localScale.x + 0.1f; //�������� ������������ ������ ����� � ��� ������ �������� ��� �� � 
        PosRay = transform.position + new Vector3(x, 0, 0);
    }

    void Update()
    {
        //Debug.DrawLine(PosRay, PosRay + new Vector3(Distance, 0, 0), Color.red); //����������� ���� 
    }

    public void RunRay() //������ ���� 
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(PosRay, Vector2.right, Distance);

        if(hits.Length > 0) //���������, �������� �� ��� ����� 
        {
            blocks.Clear();
            for (int i = 0; i < hits.Length; i++) 
            {
                if (hits[i].collider.CompareTag("Block"))
                {
                    blocks.Add(hits[i].collider.GetComponent<Block>()); //���������� ������ � ������ 
                }
            }
        }

        //print($"{gameObject.name} - {blocks.Count}");
    } 

    public void RemoveLine() //�-��� ��� �������� ����� ������� 
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            Destroy(blocks[i].gameObject);
        }

        blocks.Clear();
    }
}
