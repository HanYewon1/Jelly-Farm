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

    void Hide() // �˾� â �����
    {
        // �˾� â�� ���̴� ������ ���� �۵�
        if (_rectTransform.anchoredPosition.y >= 0)
        {
            // ��ġ �Է� Ȯ�� (ù ��° ��ġ�� ó��) + UI ��� �ƴ� ���� ��ġ�ߴ��� Ȯ��
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !IsPointerOverUIObject())
            {
                _animator.SetTrigger("DoHide");
                _animator.ResetTrigger("DoShow");
            }
        }
    }

    // ȭ���� UI ��Ҹ� ��ġ�ߴ��� Ȯ���ϴ� �޼���
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0; // UI ��Ҹ� ��ġ�ߴٸ� true ��ȯ
    }

    public void Exit() // �˾� â �ݱ�
    {
        // Exit ��ư�� ������ �� â�� ����
        _animator.SetTrigger("DoHide");
        _animator.ResetTrigger("DoShow");
    }
}
