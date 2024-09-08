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

    public List<JellyController> Jelly_List = new List<JellyController>();
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
    bool[] unlockList;

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

    public GameObject jellyPrefab;

    public int _page = 0; // UI 페이지

    JellyController jellyController;

    void Awake()
    {
        Instance = this;
        isSell = false;
        unlockList = new bool[12];
        for(int i = 0; i <= 2; i++)
        {
            unlockList[i] = true; //페이지 0, 1, 2 해금된 상태로 시작
            LockGroup.gameObject.SetActive(!unlockList[i]);
        }
        data_manager = data_manager_obj.GetComponent<DataManager>();
    }

    private void Start()
    {
        jellyController = GetComponent<JellyController>();
        invoke("LoadData", 0.1f);
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
        SoundManager.Instance.Sound("Button");
    }

    public void OnLeftButtonClick()
    {
        if(_page <=0) return; //최소 0페이지
        Debug.Log("Left button clicked");
        _page--;
        Debug.Log("page: " + _page);
        PanelChange();
        SoundManager.Instance.Sound("Button");
    }

    void PanelChange()
    {
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
        _jelatin += (id + 1) * level * clickPage;
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
        SoundManager.Instance.Sound("Unlock");
        _jelatin -= jellyJelatinList[_page]; //보유 젤라틴 - 필요한 젤라틴

        
    }

    public void Buy()//골드로 젤리 구매
    {
        if (_gold < jellyGoldList[_page]) return;
        GameObject obj = Instantiate(jellyPrefab, new Vector3(0, 0, 0), Quaternion.identity); //젤리 생성
        JellyController jellyController = obj.GetComponent<JellyController>();
        obj.name = "Jelly " + _page;
        jellyController._id = _page;
        jellyController.spriteRenderer.sprite = jellySpriteList[_page];
        _gold -= jellyGoldList[_page]; //보유 골드 - 필요한 골드
        SoundManager.Instance.Sound("Buy");
    }

    public void NumGoldUpgrade() //plant panel 버튼
    {
        if (_gold < numGoldList[numPage]) return;
        _gold -= numGoldList[numPage++]; //보유 골드 - 필요한 골드
        if (numPage >= 5) NumGroup.gameObject.SetActive(false);
        else numGoldText.text = numGoldList[numPage].ToString();
        numSubText.text = "젤리 수용량 " + numPage * 2;
        SoundManager.Instance.Sound("Button");
    }

    public void ClickGodlUpgrade() //plant panel 버튼
    {
        if (_gold < clickGoldList[clickPage]) return;
        _gold -= clickGoldList[clickPage]; //보유 골드 - 필요한 골드
        if (clickPage >= 5) ClickGroup.gameObject.SetActive(false);
        else clickGoldText.text = clickGoldList[clickPage++].ToString();
        clickSubText.text = "클릭 생산량 x " + clickPage;
        SoundManager.Instance.Sound("Button");

    }

    void LoadData()
    {
        lock_group.gameObject.SetActive(!unlockList[_page]);

        for(int i = 0; i < Jelly_Data_List.Count; ++i)
        {
            GameObject obj = Instantiate(jellyPrefab, Jelly_Data_List[i]._pos, Quaternion.identity);
        }
    }
}
