using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellJelly : MonoBehaviour
{
    DragController dragController;


    private void Start()
    {
        dragController = GetComponent<DragController>();
    }
    void Update()
    {
        Sell();
    }

    void Sell()
    {
        if (EventSystem.current.IsPointerOverGameObject() && dragController != null)
        {
            Destroy(this.gameObject);
            Debug.Log("Sell");
        }
        {

        }
    }
}
