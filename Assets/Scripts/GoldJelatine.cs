using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GoldJelatine : MonoBehaviour
{
    public TextMeshProUGUI JelatinText;
    public TextMeshProUGUI GoldText;
    public int JelatinInt = 0;
    public int GoldInt = 0;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
       
        JelatinInt += 1;
        UpdateUI();
        Debug.Log(JelatinInt);
    }
    private void UpdateUI()
    {
        //정수에서 문자열으로 변환
        JelatinText.text=JelatinInt.ToString();
        GoldText.text=GoldInt.ToString();
    }

}
