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
    static int levelExp = 30; //레벨업에 필요한 경험치
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

    void ClickJG() //젤리 클릭 시 젤라틴, 경험치 증가
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (rayhit.collider != null) {
            if (Input.GetMouseButtonDown(0))
            {
                JelatinInt++; //젤라틴 증가
                ExpInt++; //경험치 증가
                UpdateUI();
                //Debug.Log(JelatinInt);
            }
        }
    }

     private void Exp()
    {
        ExpInt += Time.deltaTime; //경험치에 시간 더하기
        Debug.Log(ExpInt);
        if( level<3 && ExpInt >= level*levelExp ) {
            level++;
        }
        
    }


    private void UpdateUI()
    {
        //정수에서 문자열으로 변환
        JelatinText.text=JelatinInt.ToString();
        GoldText.text=GoldInt.ToString();
    }

}
