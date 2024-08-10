using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour
{
    private static DragController _instance;
    public bool _dragActive = false;
    private Vector2 _screenposition;
    private Vector3 _worldPosition;
    private Vector3 _offset;
    GoldJelatin goldJelatin;

    private void Start()
    {
        goldJelatin= GetComponent<GoldJelatin>();
    }
    void Update()
    {
        DragControll();
    }
    void DragControll()
    {
        if (_dragActive && Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            Drop();
            return;
        }
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            GetInputPosition();
            _worldPosition = Camera.main.ScreenToWorldPoint(_screenposition);
            if (_dragActive) Drag();
            else TryInitDrag();
        }
    }

        void GetInputPosition()
        {
            if(Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                _screenposition = new Vector2(mousePos.x, mousePos.y);
            }
            else if(Input.touchCount > 0)
            {
                _screenposition=Input.GetTouch(0).position;
            }
        }
        void TryInitDrag()
        {
        RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero);
        if(hit.collider!=null&&hit.collider.gameObject==gameObject)
        {
            InitDrag();
        }
           
        }

        void InitDrag()
        {
            _dragActive = true;
            _offset = transform.position - Camera.main.ScreenToWorldPoint(_screenposition);
        
        }

        void Drag()
        {
        transform.position = new Vector2(_worldPosition.x, _worldPosition.y);
        }

        void Drop()
        {
           
        if (EventSystem.current.IsPointerOverGameObject()==true)
        {
                goldJelatin.GoldInt += 200;
                Debug.Log("Sell");
            
            
            Destroy(gameObject);
        }
        _dragActive = false;
    }
    }

