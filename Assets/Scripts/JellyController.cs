using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JellyController : MonoBehaviour
{
    public float speed = 5f;
    public float minX, maxX, minY, maxY;
    public bool clickPause;
    private Vector2 JellyPosition;
    private Vector2 nextPosition;
    private Animator _animator;
    Rigidbody2D rb;
    // Start is called before the first frame update
    private void Awake()
    {
       
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        StartCoroutine(MovePause());
        clickPause = false;
    }

    private void Update()
    {
        Click();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!clickPause)
            rb.velocity = nextPosition * speed * Time.deltaTime;
        else rb.velocity = Vector2.zero;
        
    }

    IEnumerator MovePause()
    {
        while (true) //2초동안 움직임 실행 후 1초 대기
        {
            RandomMove();
            yield return new WaitForSeconds(3);
            nextPosition = Vector2.zero;
            _animator.SetBool("isMove", false);
            yield return new WaitForSeconds(2);
        }
    }

    void RandomMove() //랜덤 방향으로 이동
    {
        nextPosition = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        JellyPosition = transform.position;
        //왼쪽으로 이동 시 flipX 처리
        if (nextPosition.x < 0) //왼쪽
        {
            GetComponent<SpriteRenderer>().flipX = true;
            _animator.SetBool("isMove", true);
        }
        else if(nextPosition.x > 0) //오른쪽
        {
            GetComponent<SpriteRenderer>().flipX = false;
            _animator.SetBool("isMove", true);
        }
        else //움직이지 않을 경우
        {
            _animator.SetBool("isMove", false);
        }
    }

    void Click() //마우스 클릭 시
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (rayhit.collider != null) //ray가 콜라이더와 충돌 시
        {
            if (Input.GetMouseButtonDown(0)) //마우스 클릭 시
                StartCoroutine(Pause());
        } 
    }

    IEnumerator Pause() //즉시 이동 멈추고 Touch 애니메이션
    {
        clickPause = true;
        _animator.SetBool("isMove", false);
        _animator.SetTrigger("isTouch");
        yield return new WaitForSeconds(1);
        clickPause = false;
    }
}
