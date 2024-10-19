using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController_JellyPanel : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Animator _animator;
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        Hide();

    }


    void Hide() //팝업 창 숨기기
    {
        //UI를 제외한 다른 화면 터치 시 창 내려감
        if (_rectTransform.anchoredPosition.y >= 0)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !IsPointerOverUIObject())
            {
                _animator.SetTrigger("DoHide");
                _animator.ResetTrigger("DoShow");

            }
        }
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0; // UI 요소를 터치했다면 true 반환
    }
}
