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
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _animator.SetTrigger("DoHide");
                _animator.ResetTrigger("DoShow");

            }
        }
    }

}