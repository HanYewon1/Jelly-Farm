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
    public int numberofJelly; //���� ���� ����

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

    // AnimatorController ��ü
    public void ChangeAc(Animator anim, int level)
    {
        anim.runtimeAnimatorController = level_ac[level - 1];
    }

    // �Ǹ� ���� üũ
    public void CheckSell()
    {
        isSell = true;
    }

    //�Ǹ� �� ���� ����
    public void JellyDecrease()
    {
        numberofJelly--; //���� ���� �� ����
        if (numberofJelly == 0)
        {
            Debug.Log("������ ���� ������");
        }
        
        isSell = false;
    }

    // ������ ��ư Ŭ��
    public void OnRightButtonClick()
    {
        if (_page >= 11) return; //�ִ� 11������
        Debug.Log("Right button clicked");
        _page++;
        PanelChange();
        SoundManager.Instance.Sound("Button");
    }

    // ���� ��ư Ŭ��
    public void OnLeftButtonClick()
    {
        if (_page <= 0) return; //�ּ� 0������
        Debug.Log("Left button clicked");
        _page--;
        PanelChange();
        SoundManager.Instance.Sound("Button");
    }

    // �г� ����
    void PanelChange()
    {
        LockGroup.SetActive(!unlockList[_page]);
        PageNumber.text = string.Format("#{0:00}", (_page + 1));

        if (LockGroup.activeSelf) // ��ݵ� ȭ��
        {
            lockedJelly.sprite = jellySpriteList[_page];
            Jelly_Jelatin_List.text = jellyJelatinList[_page].ToString();
        }
        else // �رݵ� ȭ��
        {
            Jelly_Gold_List.text = jellyGoldList[_page].ToString();
            Jelly_Sprite_List.sprite = jellySpriteList[_page];
            Jelly_Name_List.text = jellyNameList[_page];
        }
    }

    // ����ƾ �� ��ȭ
    public void JelatinChange(int id, int level)
    {
        _jelatin += (id + 1) * level * clickPage;
        if (_jelatin > maxJelatin) _jelatin = maxJelatin;
        Jelatin_Text.text = _jelatin.ToString();
    }

    // ��� �� ��ȭ
    public void GoldChange(int id, int level)
    {
        _gold += jellyGoldList[id] * level;
        if (_gold > maxGold) _gold = maxGold;
        Gold_Text.text = _gold.ToString();
    }

    // ��� ���� ó��
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

        if (GameClear()) //��� �ر� �� ���� Ŭ����
        {
            noticeManager.UnlockNotice();
        }
    }

    // ���� Ŭ���� ���� üũ
    private bool GameClear()
    {
        foreach (bool isUnlocked in unlockList)
        {
            if (!isUnlocked) return false;
        }
        return true;
    }

    // ���� ����
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

    // ���뷮 ���׷��̵�
    public void NumGoldUpgrade()
    {
        Console.WriteLine("click");
        if (_gold < numGoldList[numPage - 1])//gold �� �ʿ� gold���� ���� ���
        {
            noticeManager.NotGoldNotice_Jelly();//�˸�â
            return;
        }
        else
        {
            _gold -= numGoldList[numPage - 1];//gold - �ʿ� gold
            Gold_Text.text = _gold.ToString(); //��� ������Ʈ
            if (numPage >= 5) NumGroup.SetActive(false);//�ִ� ������ �޼����� ��� Ŭ�� ��Ȱ��ȭ
            else numGoldText.text = numGoldList[numPage].ToString();// ��ư �ؽ�Ʈ ������Ʈ
            numSubText.text = "���� ���뷮 " + (numPage + 1) * 2; //�ؽ�Ʈ ������Ʈ
            SoundManager.Instance.Sound("Button");//��ư �Ҹ�
            numPage++;//������ ����
        }
    }

    // Ŭ�� ���귮 ���׷��̵�
    public void ClickGoldUpgrade()
    {
        Console.WriteLine("click");
        if (_gold < clickGoldList[clickPage - 1]) //gold �� �ʿ� gold���� ���� ���
        {
            noticeManager.NotGoldNotice_Jelly(); //�˸�â
            return;
        }
        else
        {
            _gold -= clickGoldList[clickPage - 1]; //gold - �ʿ� gold
            Gold_Text.text = _gold.ToString(); //��� ������Ʈ
            if (clickPage >= 5) ClickGroup.SetActive(false); //�ִ� ������ �޼����� ��� Ŭ�� ��Ȱ��ȭ
            else clickGoldText.text = clickGoldList[clickPage].ToString(); // ��ư �ؽ�Ʈ ������Ʈ
            clickSubText.text = "Ŭ�� ���귮 x " + clickPage;//�ؽ�Ʈ ������Ʈ
            SoundManager.Instance.Sound("Button"); //��ư �Ҹ�
            clickPage++; //������ ����
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