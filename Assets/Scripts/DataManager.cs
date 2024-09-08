using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int _jelatin;
    public int _gold;
    public bool[] Jelly_Unlock_List = new bool[12];
    public List<Data> Jelly_List = new List<Data>();
}

public class DataManager : MonoBehaviour
{
    string _path;
    // Start is called before the first frame update
    void Start()
    {
        _path = Path.Combine(Appliaction.dataPath, "database.json");
        JsonLoad();
    }
    public void JsonLoad()
    {
        SaveData save_data = new SaveData();
        if (!File.Exists(_path))
        {
            GameManager.Instance._jelatin = 0;
            GameManager.Instance._gold = 0;
            JsonSave();
        }
        else
        {
            string load_json = File.ReadAllText(_path);
            save_data = JsonUtility.FromJson<SaveData>(load_json);

            if(save_data!=null)
            {
                for(int i = 0; i < save_data.Jelly_List.Count; ++i)
                {
                    GameManager.Instance.Jelly_Data_List.Add(save_data.Jelly_List[i])
                }
                for(int i=0;i<save_data.Jelly_Unlock_List.Length; ++i)
                {
                    GameManager.Instance.Jelly_Unlock_List[i] = save_data.Jelly_Unlock_List[i];
                }
                GameManager.Instance._jelatin = save_data._jelatin;
                GameManager.Instance._gold =save_data._gold;
            }
        }
    }

    public void JsonSave()
    {
        SaveData save_data = new SaveData();

        for(int i=0;i<GameManager.Instance.Jelly_List.Count;++i)
        {
            JellyController jellyController = GameManager.Instance.Jelly_List[i];
            save_data.Jelly_List.Add(new Data(jellyController.gameObject.transform.position, jellyController._id, jellyController._level, jellyController._exp));
        }
        for(int i = 0; i < GameManager.Instance.Jelly_Unlock_List.Length; ++i)
        {
            save_data.Jelly_Unlock_List[i] = GameManager.Instance.Jelly_Unlock_List[i];

        }
        save_data._jelatin = GameManager.Instance._jelatin;
        save_data._gold = GameManager.Instance._gold;

        string _json = JsonUtility.ToJson(save_data, true);
        File.WriteAllText(_path, _json);
    }
    // Update is called once per frame
   
}
