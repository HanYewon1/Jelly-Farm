using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private bool _DragActive = false;
    private Vector2 _screenposition;
    private Vector3 _worldPosition;

    private void Awake()
    {
        DragController[] controllers = FindObjectsOfType<DragController>();
        if (controllers.Length > 1) Destroy(gameObject);
    }
    void Update()
    {
        if (_DragActive && Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            Drop();
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            _screenposition = new Vector2(mousePos.x, mousePos.y);
        }
        else if (Input.touchCount > 0)
        {
            _screenposition = Input.GetTouch(0).position;
        }
        else return;

        _worldPosition = Camera.main.ScreenToWorldPoint(_screenposition);
        if (_DragActive) Drag();
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero);
            if (hit.collider != null) InitDrag();
        }

        void InitDrag()
        {
            _DragActive = true;
        }

        void Drag()
        {
            gameObject.transform.position = new Vector2(_worldPosition.x, _worldPosition.y);
        }

        void Drop()
        {
            _DragActive = false;
        }
    }
}
