using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NoticeManager : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Animator _animator;

    public string[] NoticeList;
    public TMP_Text Notice_List_Text;
    public GameManager gameManager;
    // Start is called before the first frame update

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        
        Notice_List_Text.text = NoticeList[0].ToString(); //���� ���� �� �˸� ����
        SetNotice();
    }
    
    public void SetNotice() //�˸� ����
    {
        _animator.SetTrigger("ShowNotice");
    }


    public void SellButtonNotice()
    {
        Notice_List_Text.text = NoticeList[2].ToString(); //Sell ��ư ������ �˸� ����
        SetNotice();
    }

    public void NotJelatinNotice_Jelly()
    {
        if (gameManager._jelatin < gameManager.jellyJelatinList[gameManager._page])
        {
            Notice_List_Text.text = NoticeList[3].ToString(); //����ƾ ���� �� �˸� ����
            SetNotice();
        }
    }
    public void NotGoldNotice_Jelly()
    {
        if (gameManager._gold < gameManager.jellyGoldList[gameManager._page])
        {
            Notice_List_Text.text = NoticeList[4].ToString(); //��� ���� �� �˸� ����
            SetNotice();
        }
    }
    public void NotGoldNotice_Num()
    {
        if (gameManager._gold < gameManager.numGoldList[gameManager.numPage])
        { Notice_List_Text.text = NoticeList[4].ToString(); //��� ���� �� �˸� ����
            SetNotice();
        }
    }
    public void NotGoldNotice_Click ()
    {
        if (gameManager._gold < gameManager.clickGoldList[gameManager.clickPage])
        { Notice_List_Text.text = NoticeList[4].ToString(); //��� ���� �� �˸� ����
            SetNotice();
        }
    }
    public void NotNumNotice()
    {
        if (gameManager.Jelly_List.Count >= gameManager.numPage * 2)
        { Notice_List_Text.text = NoticeList[5].ToString(); //���뷮 ���� �� �˸� ����
            SetNotice();
        }
    }

    public void UnlockNotice()
    {
        Notice_List_Text.text = NoticeList[1].ToString(); //��� ���� �ر� �� �˸� ����
        SetNotice();
    }
}
