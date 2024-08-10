using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellJelly : MonoBehaviour
{
    GoldJelatin goldJelatin;
    DragController dragController;


    private void Start()
    {
        goldJelatin = GetComponent<GoldJelatin>();
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
            goldJelatin.GoldInt += 200;
            Debug.Log("Sell");
        }
        {

        }
    }
}
