using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; //Text, Image 사용 위함

public class GameManager : MonoBehaviour
{
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

    public int _jelatin;
    public TMP_Text Jelatin_Text;

    public int _gold;
    public TMP_Text Gold_Text;
    public bool isSell;
    public int maxGold;

    private static int _page = 0; // UI 페이지

    JellyController jellyController;

    private void Start()
    {
        jellyController = GetComponent<JellyController>();
    }
    void Awake()
    {
        isSell = false;
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
        Jelly_Gold_List.text = jellyGoldList[_page].ToString();
        Jelly_Sprite_List.sprite = jellySpriteList[_page];
        Jelly_Jelatin_List.text = jellyJelatinList[_page].ToString();
        Jelly_Name_List.text = jellyNameList[_page];
        PageNumber.text = string.Format("#{0:00}", (_page + 1));
    }

    public void JelatinChange() //젤라틴 값 변화
    {
        Jelatin_Text.text = _jelatin.ToString();
    }
    public void GoldChange(int id, int level)
    {
        _gold += jellyGoldList[id] * level;
        if (_gold > maxGold) _gold = maxGold;
        Gold_Text.text = _gold.ToString();
    }



}
