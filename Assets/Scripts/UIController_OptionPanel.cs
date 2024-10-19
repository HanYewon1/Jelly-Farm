using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController_OptionPanel : MonoBehaviour
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

    void Hide() // 팝업 창 숨기기
    {
        // 팝업 창이 보이는 상태일 때만 작동
        if (_rectTransform.anchoredPosition.y >= 0)
        {
            // 터치 입력 확인 (첫 번째 터치만 처리) + UI 요소 아닌 곳을 터치했는지 확인
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !IsPointerOverUIObject())
            {
                _animator.SetTrigger("DoHide");
                _animator.ResetTrigger("DoShow");
            }
        }
    }

    // 화면의 UI 요소를 터치했는지 확인하는 메서드
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0; // UI 요소를 터치했다면 true 반환
    }

    public void Exit() // 팝업 창 닫기
    {
        // Exit 버튼을 눌렀을 때 창을 숨김
        _animator.SetTrigger("DoHide");
        _animator.ResetTrigger("DoShow");
    }
}
