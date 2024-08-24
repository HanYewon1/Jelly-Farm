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
            _clickTime = Time.time; //���콺 ���� ���� �ð�
        }
        if (_dragActive && Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            Drop();
            return;
        }
        if ((Input.GetMouseButton(0) || Input.touchCount > 0)&&(Time.time - _clickTime >=0.2))
        { //0.2�� �̻� Ŭ���ϰ� ���� ���
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
            _offset = transform.position - _worldPosition; //�巡�� �� ��ü�� ��ġ ��ġ�� ������
        
        }

        void Drag()
        {//�巡�� ���� ��ü ��ġ ����
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

