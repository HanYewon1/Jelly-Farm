using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; //Text, Image 사용 위함

public class GameManager : MonoBehaviour
{
    public RuntimeAnimatorController[] level_ac;
    //데이터 저장할 변수들
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

    private static int _page = 0; // UI 페이지

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
            unlockList[i] = true; //페이지 0, 1, 2 해금된 상태로 시작
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
        if (_page >= 11) return; //최대 11페이지
        Debug.Log("Right button clicked");
        _page++;
        Debug.Log("page: " + _page);
        PanelChange();
    }

    public void OnLeftButtonClick()
    {
        if(_page <=0) return; //최소 0페이지
        Debug.Log("Left button clicked");
        _page--;
        Debug.Log("page: " + _page);
        PanelChange();
    }

    void PanelChange()
    {
        //0, 1, 2 페이지는 항상 해금
        LockGroup.gameObject.SetActive(!unlockList[_page]);
        PageNumber.text = string.Format("#{0:00}", (_page + 1));

        if (LockGroup.gameObject.activeSelf) //잠금된 화면
        {
            lockedJelly.sprite = jellySpriteList[_page];
            Jelly_Jelatin_List.text = jellyJelatinList[_page].ToString();
        }
        else //해금된 화면
        {
            Jelly_Gold_List.text = jellyGoldList[_page].ToString();
            Jelly_Sprite_List.sprite = jellySpriteList[_page];
            Jelly_Name_List.text = jellyNameList[_page];
        }
    }

    public void JelatinChange(int id, int level) //젤라틴 값 변화
    {
        _jelatin += (id + 1) * level;
        if (_jelatin > maxJelatin) _jelatin = maxJelatin;
        Jelatin_Text.text = _jelatin.ToString();
    }
    public void GoldChange(int id, int level) //골드 값 변화
    {
        _gold += jellyGoldList[id] * level;
        if (_gold > maxGold) _gold = maxGold;
        Gold_Text.text = _gold.ToString();
    }

    public void LockList() //해금
    {
        
        //보유하고 있는 젤라틴이 필요한 젤라틴보다 적으면 무효
        if (_jelatin < jellyJelatinList[_page]) return;
        unlockList[_page] = true;
        PanelChange();
        _jelatin -= jellyJelatinList[_page]; //보유 젤라틴 - 필요한 젤라틴
        
    }



}
