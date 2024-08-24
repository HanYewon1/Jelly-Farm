using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour
{
    GameManager gameManager;
    JellyController jellyController;
    private static DragController _instance;
    public bool _dragActive = false;
    private Vector2 _screenposition;
    private Vector3 _worldPosition;
    private Vector3 _offset;

    private float _clickTime;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        jellyController = GetComponent<JellyController>(); 
    }
    void Update()
    {
        DragControll();
    }
    void DragControll()
    {
        if(Input.GetMouseButtonDown(0)||Input.touchCount==1&&Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _clickTime = Time.time; //마우스 눌린 이후 시간
        }
        if (_dragActive && Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            Drop();
            return;
        }
        if ((Input.GetMouseButton(0) || Input.touchCount > 0)&&(Time.time - _clickTime >=0.2))
        { //0.2초 이상 클릭하고 있을 경우
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
                _worldPosition = Camera.main.ScreenToWorldPoint(_screenposition);

        }
            else if(Input.touchCount > 0)
            {
                _screenposition=Input.GetTouch(0).position;
                _worldPosition = Camera.main.ScreenToWorldPoint(_screenposition);

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
            _offset = transform.position - _worldPosition; //드래그 시 객체가 터치 위치에 맞춰짐
        
        }

        void Drag()
        {//드래그 도중 객체 위치 설정
            transform.position = _worldPosition + _offset;
        }

        void Drop()
        {
        if (gameManager.isSell)
        {
            gameManager.GoldChange(jellyController._id, jellyController._level);
            Destroy(gameObject);
        }
            _dragActive = false;
        }
    }

