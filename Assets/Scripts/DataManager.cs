using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public List<Data> Jelly_List = new List<Data>();

    public int _jelatin;
    public int _gold;
    public int numPage;
    public int clickPage;

    public bool[] Jelly_Unlock_List = new bool[12];
    
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    string _path;

    private void Awake()
    {
        Instance = this;
        _path = Path.Combine(Application.persistentDataPath, "database.json");
    }
    // Start is called before the first frame update
    void Start()
    {
        JsonLoad();
    }
    public void JsonLoad()
    {
        SaveData save_data = new SaveData();

        if (!File.Exists(_path))//ó�� ������ ����
        {
            GameManager.Instance._jelatin = 100;
            GameManager.Instance._gold = 200;
            GameManager.Instance.numPage = 1;
            GameManager.Instance.clickPage = 1;
            GameManager.Instance._isClear = false;
            JsonSave();
        }
        else
        {
            string load_json = File.ReadAllText(_path);
            save_data = JsonUtility.FromJson<SaveData>(load_json);

            if(save_data!=null)
            {
                //Jelly_List �ҷ�����
                GameManager.Instance.Jelly_Data_List = new List<Data>(save_data.Jelly_List);
                
                //Jelly_Unlock_List �ҷ�����
                for(int i=0;i<save_data.Jelly_Unlock_List.Length; ++i)
                {
                    GameManager.Instance.unlockList[i] = save_data.Jelly_Unlock_List[i];
                }

                //���� ���� �ҷ�����
                GameManager.Instance._jelatin = save_data._jelatin;
                GameManager.Instance._gold =save_data._gold;
                GameManager.Instance.numPage = save_data.numPage;
                GameManager.Instance.clickPage = save_data.clickPage;
            }
        }
    }

    public void JsonSave()
    {
        SaveData save_data = new SaveData();

        //Jelly_List ����
        foreach(var jelly in GameManager.Instance.Jelly_Data_List)
        {
            save_data.Jelly_List.Add(jelly);
        }

        //Jelly_Unlock_List ����
        for(int i = 0; i < GameManager.Instance.unlockList.Length; ++i)
        {
            save_data.Jelly_Unlock_List[i] = GameManager.Instance.unlockList[i];

        }

        //���� ���� ����
        save_data._jelatin = GameManager.Instance._jelatin;
        save_data._gold = GameManager.Instance._gold;
        save_data.numPage = GameManager.Instance.numPage;
        save_data.clickPage = GameManager.Instance.clickPage;

        //JSON ���� ����
        string _json = JsonUtility.ToJson(save_data, true);
        File.WriteAllText(_path, _json);
    }
    void OnApplicationQuit()
    {
        JsonSave(); // ���� �� ������ ����
    }
}
