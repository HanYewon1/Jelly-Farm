using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; //Text, Image ��� ����

public class GameManager : MonoBehaviour
{
    public RuntimeAnimatorController[] level_ac;
    //������ ������ ������
    public int[] jellyGoldList;
    public Sprite[] jellySpriteList;
    public int[] jellyJelatinList;
    public string[] jellyNameList;

    public TMP_Text Jelly_Gold_List;
    public Image Jelly_Sprite_List;
    public TMP_Text Jelly_Jelatin_List;
    public TMP_Text Jelly_Name_List;
    public TMP_Text PageNumber;

    //locked
    public GameObject LockGroup;
    public Image lockedJelly;
    public Image lockedimage;

    public int _jelatin;
    public TMP_Text Jelatin_Text;

    public int _gold;
    public TMP_Text Gold_Text;
    public bool isSell;
    public int maxGold;
    public int maxJelatin;
    public int maxExp;
    bool[] unlockList;

    private static int _page = 0; // UI ������

    JellyController jellyController;

    private void Start()
    {
        jellyController = GetComponent<JellyController>();
    }
    void Awake()
    {
        isSell = false;
        unlockList = new bool[12];
        for(int i = 0; i <= 2; i++)
        {
            unlockList[i] = true; //������ 0, 1, 2 �رݵ� ���·� ����
            LockGroup.gameObject.SetActive(!unlockList[i]);
        }
       ;

    }
    public void ChangeAc(Animator anim, int level)
    {
        anim.runtimeAnimatorController = level_ac[level - 1];
    }
    public void CheckSell()
    {
        isSell = (isSell == false);
    }

    public void OnRightButtonClick()
    {
        if (_page >= 11) return; //�ִ� 11������
        Debug.Log("Right button clicked");
        _page++;
        Debug.Log("page: " + _page);
        PanelChange();
    }

    public void OnLeftButtonClick()
    {
        if(_page <=0) return; //�ּ� 0������
        Debug.Log("Left button clicked");
        _page--;
        Debug.Log("page: " + _page);
        PanelChange();
    }

    void PanelChange()
    {
        //0, 1, 2 �������� �׻� �ر�
        LockGroup.gameObject.SetActive(!unlockList[_page]);
        PageNumber.text = string.Format("#{0:00}", (_page + 1));

        if (LockGroup.gameObject.activeSelf) //��ݵ� ȭ��
        {
            lockedJelly.sprite = jellySpriteList[_page];
            Jelly_Jelatin_List.text = jellyJelatinList[_page].ToString();
        }
        else //�رݵ� ȭ��
        {
            Jelly_Gold_List.text = jellyGoldList[_page].ToString();
            Jelly_Sprite_List.sprite = jellySpriteList[_page];
            Jelly_Name_List.text = jellyNameList[_page];
        }
    }

    public void JelatinChange(int id, int level) //����ƾ �� ��ȭ
    {
        _jelatin += (id + 1) * level;
        if (_jelatin > maxJelatin) _jelatin = maxJelatin;
        Jelatin_Text.text = _jelatin.ToString();
    }
    public void GoldChange(int id, int level) //��� �� ��ȭ
    {
        _gold += jellyGoldList[id] * level;
        if (_gold > maxGold) _gold = maxGold;
        Gold_Text.text = _gold.ToString();
    }

    public void LockList() //�ر�
    {
        
        //�����ϰ� �ִ� ����ƾ�� �ʿ��� ����ƾ���� ������ ��ȿ
        if (_jelatin < jellyJelatinList[_page]) return;
        unlockList[_page] = true;
        PanelChange();
        _jelatin -= jellyJelatinList[_page]; //���� ����ƾ - �ʿ��� ����ƾ
        
    }



}
