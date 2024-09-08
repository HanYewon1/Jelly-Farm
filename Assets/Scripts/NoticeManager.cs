using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoticeManager : MonoBehaviour
{
    public string[] NoticeList;
    public TMP_Text Notice_List;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        Notice_List.text = NoticeList[0].ToString(); //���� ���� �� �˸� ����
    }

   public void SellButtonNotice()
    {
        Notice_List.text = NoticeList[2].ToString(); //Sell ��ư ������ �˸� ����
    }

    public void NotJelatinNotice_Jelly()
    {
        if(gameManager._jelatin < gameManager.jellyJelatinList[gameManager._page])
            Notice_List.text = NoticeList[3].ToString(); //����ƾ ���� �� �˸� ����
    }
    public void NotGoldNotice_Jelly()
    {
        if (gameManager._gold < gameManager.jellyGoldList[gameManager._page])
            Notice_List.text = NoticeList[4].ToString(); //��� ���� �� �˸� ����
    }
    public void NotGoldNotice_Num()
    {
        if (gameManager._gold < gameManager.numGoldList[gameManager.numPage])
            Notice_List.text = NoticeList[4].ToString(); //��� ���� �� �˸� ����
    }
    public void NotGoldNotice_Click ()
    {
        if (gameManager._gold < gameManager.clickGoldList[gameManager.clickPage])
            Notice_List.text = NoticeList[4].ToString(); //��� ���� �� �˸� ����
    }
    public void NotNumNotice()
    {
        if (gameManager.Jelly_List.Count >= gameManager.numPage * 2)
            Notice_List.text = NoticeList[4].ToString(); //���뷮 ���� �� �˸� ����
    }

    public void UnlockNotice()
    {
        for(int i = 0; i < 12; i++)
        {
            if (gameManager.unlockList[i] == false) return;
        }
        Notice_List.text = NoticeList[1].ToString(); //��� ���� �ر� �� �˸� ����
    }
}
