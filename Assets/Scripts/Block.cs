using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    Shape parent;

    public bool isMoving = true;

    private void Start()
    {
        parent = GetComponentInParent<Shape>();
    }

    private void OnTriggerEnter2D(Collider2D collision)    
    {
        if(collision.CompareTag("LeftBorder") || collision.CompareTag("RightBorder")) // проверяет касание левой и правой границы 
        {
            //print(collision.tag);
            parent.BorderColided(collision.tag);
        }
        else if (collision.CompareTag("BottomBorder") || //проверка на касание нижней границы или блока (и проверка по координате х)
         (collision.CompareTag("Block") &&
         Mathf.Approximately(collision.transform.position.x, transform.position.x))) //математическая ф-цоя для срвнения близко стоящих значений 
        {
            if (parent != null) //проверка родителя
            {
                parent.BorderColided(collision.tag);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // ф-ция для сдвига фигур в стороны 
    {
        if (collision.CompareTag("LeftBorder") || collision.CompareTag("RightBorder"))
        {
            parent.BorderColided("None"); // проверяет блок switch и переходит к default 
        }
    }
}
