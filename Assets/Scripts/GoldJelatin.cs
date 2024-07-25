using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GoldJelatin : MonoBehaviour
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
       
        ClickJG();
    }

    void ClickJG() //���� Ŭ�� �� ����ƾ ����
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (rayhit.collider != null) {
            if (Input.GetMouseButtonDown(0))
            {
                JelatinInt++;
                UpdateUI();
                Debug.Log(JelatinInt);
            }
        }
    }
   
    private void UpdateUI()
    {
        //�������� ���ڿ����� ��ȯ
        JelatinText.text=JelatinInt.ToString();
        GoldText.text=GoldInt.ToString();
    }

}
