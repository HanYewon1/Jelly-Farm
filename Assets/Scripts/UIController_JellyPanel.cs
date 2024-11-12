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


    void Hide() //�˾� â �����
    {
        //UI�� ������ �ٸ� ȭ�� ��ġ �� â ������
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