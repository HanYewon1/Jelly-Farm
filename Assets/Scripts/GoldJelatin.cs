using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GoldJelatin : MonoBehaviour
{
    
    public TextMeshProUGUI JelatinText;
    public TextMeshProUGUI GoldText;
    public int JelatinInt = 0;
    public int GoldInt = 0;
    static float ExpInt = 0f;
    public int level = 1;
    static int levelExp = 30; //�������� �ʿ��� ����ġ
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
       
        //ClickJG();
       // Exp();
    }

    void ClickJG() //���� Ŭ�� �� ����ƾ, ����ġ ����
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (rayhit.collider != null) {
            if (Input.GetMouseButtonDown(0))
            {
                JelatinInt++; //����ƾ ����
                ExpInt++; //����ġ ����
                UpdateUI();
                //Debug.Log(JelatinInt);
            }
        }
    }

     private void Exp()
    {
        ExpInt += Time.deltaTime; //����ġ�� �ð� ���ϱ�
        Debug.Log(ExpInt);
        if( level<3 && ExpInt >= level*levelExp ) {
            level++;
        }
        
    }


    private void UpdateUI()
    {
        //�������� ���ڿ����� ��ȯ
        JelatinText.text=JelatinInt.ToString();
        GoldText.text=GoldInt.ToString();
    }

}
