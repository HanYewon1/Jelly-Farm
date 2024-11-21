using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; //Text, Image 사용 위함

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Data> Jelly_Data_List = new List<Data>();

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
    public bool[] unlockList;

    //plant panel
    public int[] numGoldList; //젤리수용량 버튼
    public int numPage;
    public int[] clickGoldList; //클릭생산량 버튼
    public int clickPage;
    public TMP_Text numGoldText;
    public TMP_Text clickGoldText;
    public TMP_Text numSubText;
    public TMP_Text clickSubText;
    public GameObject NumGroup;
    public GameObject ClickGroup;
    public GameObject data_manager_obj;
    public DataManager data_manager;
    public NoticeManager noticeManager;
    public int numberofJelly; //현재 젤리 개수

    public bool _isClear;

    public GameObject jellyPrefab;

    public int _page = 0; // UI 페이지

    JellyController jellyController;

    void Awake()
    {
        Instance = this;
        isSell = false;
        unlockList = new bool[12];
        data_manager = data_manager_obj.GetComponent<DataManager>();
        LoadData();
    }

    private void Start()
    {
        jellyController = GetComponent<JellyController>();
        Gold_Text.text = _gold.ToString();
        Jelatin_Text.text = _jelatin.ToString();
    }

    // AnimatorController 교체
    public void ChangeAc(Animator anim, int level)
    {
        anim.runtimeAnimatorController = level_ac[level - 1];
    }

    // 판매 여부 체크
    public void CheckSell()
    {
        isSell = true;
    }

    //판매 후 젤리 감소
    public void JellyDecrease()
    {
        numberofJelly--; //현재 젤리 수 감소
        if (numberofJelly == 0)
        {
            Debug.Log("마지막 젤리 삭제됨");
        }
        
        isSell = false;
    }

    // 오른쪽 버튼 클릭
    public void OnRightButtonClick()
    {
        if (_page >= 11) return; //최대 11페이지
        Debug.Log("Right button clicked");
        _page++;
        PanelChange();
        SoundManager.Instance.Sound("Button");
    }

    // 왼쪽 버튼 클릭
    public void OnLeftButtonClick()
    {
        if (_page <= 0) return; //최소 0페이지
        Debug.Log("Left button clicked");
        _page--;
        PanelChange();
        SoundManager.Instance.Sound("Button");
    }

    // 패널 변경
    void PanelChange()
    {
        LockGroup.SetActive(!unlockList[_page]);
        PageNumber.text = string.Format("#{0:00}", (_page + 1));

        if (LockGroup.activeSelf) // 잠금된 화면
        {
            lockedJelly.sprite = jellySpriteList[_page];
            Jelly_Jelatin_List.text = jellyJelatinList[_page].ToString();
        }
        else // 해금된 화면
        {
            Jelly_Gold_List.text = jellyGoldList[_page].ToString();
            Jelly_Sprite_List.sprite = jellySpriteList[_page];
            Jelly_Name_List.text = jellyNameList[_page];
        }
    }

    // 젤라틴 값 변화
    public void JelatinChange(int id, int level)
    {
        _jelatin += (id + 1) * level * clickPage;
        if (_jelatin > maxJelatin) _jelatin = maxJelatin;
        Jelatin_Text.text = _jelatin.ToString();
    }

    // 골드 값 변화
    public void GoldChange(int id, int level)
    {
        _gold += jellyGoldList[id] * level;
        if (_gold > maxGold) _gold = maxGold;
        Gold_Text.text = _gold.ToString();
    }

    // 잠금 해제 처리
    public void LockList()
    {
        if (_jelatin < jellyJelatinList[_page])
        {
            noticeManager.NotJelatinNotice_Jelly();
            return;
        }

        unlockList[_page] = true;
        PanelChange();
        SoundManager.Instance.Sound("Unlock");
        _jelatin -= jellyJelatinList[_page];
        Jelatin_Text.text = _jelatin.ToString();

        if (GameClear()) //모두 해금 시 게임 클리어
        {
            noticeManager.UnlockNotice();
        }
    }

    // 게임 클리어 여부 체크
    private bool GameClear()
    {
        foreach (bool isUnlocked in unlockList)
        {
            if (!isUnlocked) return false;
        }
        return true;
    }

    // 젤리 구매
    public void Buy()
    {
        if (_gold < jellyGoldList[_page])
        {
            noticeManager.NotGoldNotice_Jelly();
            return;
        }
        if (numberofJelly >= numPage * 2)
        {
            noticeManager.NotNumNotice();
            return;
        }

        GameObject obj = Instantiate(jellyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        if (obj == null)
        {
            Debug.LogError("jellyPrefab instantiation failed!");
            return;
        }

        JellyController jellyController = obj.GetComponent<JellyController>();
        if (jellyController == null)
        {
            Debug.LogError("JellyController is null!");
            return;
        }

        obj.name = "Jelly " + _page;
        jellyController._id = _page;
        jellyController._exp = 0f;
        jellyController.spriteRenderer.sprite = jellySpriteList[_page];
        _gold -= jellyGoldList[_page];
        Gold_Text.text = _gold.ToString();
        numberofJelly++;
        SoundManager.Instance.Sound("Buy");
    }

    // 수용량 업그레이드
    public void NumGoldUpgrade()
    {
        Console.WriteLine("click");
        if (_gold < numGoldList[numPage - 1])//gold 가 필요 gold보다 작을 경우
        {
            noticeManager.NotGoldNotice_Jelly();//알림창
            return;
        }
        else
        {
            _gold -= numGoldList[numPage - 1];//gold - 필요 gold
            Gold_Text.text = _gold.ToString(); //골드 업데이트
            if (numPage >= 5) NumGroup.SetActive(false);//최대 레벨에 달성했을 경우 클릭 비활성화
            else numGoldText.text = numGoldList[numPage].ToString();// 버튼 텍스트 업데이트
            numSubText.text = "젤리 수용량 " + (numPage + 1) * 2; //텍스트 업데이트
            SoundManager.Instance.Sound("Button");//버튼 소리
            numPage++;//페이지 증가
        }
    }

    // 클릭 생산량 업그레이드
    public void ClickGoldUpgrade()
    {
        Console.WriteLine("click");
        if (_gold < clickGoldList[clickPage - 1]) //gold 가 필요 gold보다 작을 경우
        {
            noticeManager.NotGoldNotice_Jelly(); //알림창
            return;
        }
        else
        {
            _gold -= clickGoldList[clickPage - 1]; //gold - 필요 gold
            Gold_Text.text = _gold.ToString(); //골드 업데이트
            if (clickPage >= 5) ClickGroup.SetActive(false); //최대 레벨에 달성했을 경우 클릭 비활성화
            else clickGoldText.text = clickGoldList[clickPage].ToString(); // 버튼 텍스트 업데이트
            clickSubText.text = "클릭 생산량 x " + clickPage;//텍스트 업데이트
            SoundManager.Instance.Sound("Button"); //버튼 소리
            clickPage++; //페이지 증가
        }
    }

    public void Clear()
    {
        gameObject.SetActive(true);
    }

    void LoadData()
    {
        LockGroup.SetActive(!unlockList[_page]);

        for (int i = 0; i < Jelly_Data_List.Count; ++i)
        {
            Instantiate(jellyPrefab, Jelly_Data_List[i]._pos, Quaternion.identity);
        }
    }
}