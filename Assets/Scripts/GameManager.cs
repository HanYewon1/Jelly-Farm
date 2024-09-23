using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; //Text, Image ��� ����

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<JellyController> Jelly_List = new List<JellyController>();
    public List<Data> Jelly_Data_List = new List<Data>();

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
    public bool[] unlockList;

    //plant panel
    public int[] numGoldList; //�������뷮 ��ư
    public int numPage;
    public int[] clickGoldList; //Ŭ�����귮 ��ư
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

    public bool _isClear;

    public GameObject jellyPrefab;

    public int _page = 0; // UI ������

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

    public void ChangeAc(Animator anim, int level)
    {
        anim.runtimeAnimatorController = level_ac[level - 1];
    }
    public void CheckSell()
    {
        isSell = !isSell;
        if (Jelly_List.Count > 0)
        {
            Jelly_List.RemoveAt(Jelly_List.Count - 1);
            Console.WriteLine("Jelly_List���� ������ ��� ������");
        }

    }

    public void OnRightButtonClick()
    {
        if (_page >= 11) return; //�ִ� 11������
        Debug.Log("Right button clicked");
        _page++;
        Debug.Log("page: " + _page);
        PanelChange();
        SoundManager.Instance.Sound("Button");
    }

    public void OnLeftButtonClick()
    {
        if(_page <=0) return; //�ּ� 0������
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
        _jelatin += (id + 1) * level * clickPage;
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
        if (_jelatin < jellyJelatinList[_page])
        {
            noticeManager.NotJelatinNotice_Jelly();
            return;
        }

        unlockList[_page] = true;
        PanelChange();
        SoundManager.Instance.Sound("Unlock");
        _jelatin -= jellyJelatinList[_page]; //���� ����ƾ - �ʿ��� ����ƾ
        Jelatin_Text.text = _jelatin.ToString();

        if (GameClear()) //��� �ر� �� ���� Ŭ����
        {
            noticeManager.UnlockNotice();
        }

        
    }

    private bool GameClear() //��� �ر��ߴ��� �Ǵ�
    {
        foreach(bool isUnlocked in unlockList)
        {
            if (!isUnlocked) return false;
        }
        return true;
    }

    public void Buy()//���� ���� ����
    {
        if (_gold < jellyGoldList[_page])
        {
            noticeManager.NotGoldNotice_Jelly();
            return;
        }
        if (Jelly_List.Count >= numPage * 2)
        {
            noticeManager.NotNumNotice();
            return;
        }


        GameObject obj = Instantiate(jellyPrefab, new Vector3(0, 0, 0), Quaternion.identity); //���� ����
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
        _gold -= jellyGoldList[_page]; //���� ��� - �ʿ��� ���
        Gold_Text.text = _gold.ToString();
        Jelly_List.Add(jellyController);
        SoundManager.Instance.Sound("Buy");
        
    }

    public void NumGoldUpgrade() //plant panel ��ư
    {
        if (_gold < numGoldList[numPage - 1])
        {
            noticeManager.NotGoldNotice_Jelly();
            return;
        }
        _gold -= numGoldList[numPage - 1];//���� ��� - �ʿ��� ���
        if (numPage >= 5) NumGroup.gameObject.SetActive(false);
        else numGoldText.text = numGoldList[numPage].ToString();
        numSubText.text = "���� ���뷮 " + numPage * 2;
        SoundManager.Instance.Sound("Button");
        numPage++;
    }

    public void ClickGoldUpgrade() //plant panel ��ư
    {
        if (_gold < clickGoldList[clickPage - 1])
        {
            noticeManager.NotGoldNotice_Jelly();
            return;
        }
        _gold -= clickGoldList[clickPage - 1]; //���� ��� - �ʿ��� ���
        if (clickPage >= 5) ClickGroup.gameObject.SetActive(false);
        else clickGoldText.text = clickGoldList[clickPage].ToString();
        clickSubText.text = "Ŭ�� ���귮 x " + clickPage;
        SoundManager.Instance.Sound("Button");
        clickPage++;

    }
    public void Clear()
    {
        gameObject.SetActive(true);
    }

    void LoadData()
    {
        LockGroup.gameObject.SetActive(!unlockList[_page]);

        for(int i = 0; i < Jelly_Data_List.Count; ++i)
        {
            GameObject obj = Instantiate(jellyPrefab, Jelly_Data_List[i]._pos, Quaternion.identity);
        }
    }

}
