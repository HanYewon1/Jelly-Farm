using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JellyController : MonoBehaviour
{
    public GameObject leftTop;
    public GameObject rightBottom;
    public float speed = 5f;
    public float minX, maxX, minY, maxY;
    public bool clickPause;
    private Vector2 nextPosition;
    private Animator _animator;
    Rigidbody2D rb;
    public Vector3[] PointList;
    public GameManager gameManager;
    public int _id;
    public int _level;
    public float _exp;
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
        Exp();

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
            _animator.SetBool("isWalk", false);
            yield return new WaitForSeconds(2);
        }
    }
   /* void Border()
    {
        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        if (pos_x < leftTop.transform.position.x || pos_x > rightBottom.transform.position.x)
        {
            nextPosition.x = -nextPosition.x;
        }
        else if (pos_y > leftTop.transform.position.y || pos_y < rightBottom.transform.position.y)
        {
            nextPosition.y = -nextPosition.y;
        }
    }*/
    void RandomMove() //랜덤 방향으로 이동
    {
        float pos_x = transform.position.x;
        float pos_y = transform.position.y;
        //방향 벡터 생성
        nextPosition = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        
        //왼쪽으로 이동 시 flipX 처리
        if (nextPosition.x < 0) //왼쪽
        {
            GetComponent<SpriteRenderer>().flipX = true;
            _animator.SetBool("isWalk", true);
        }
        else if(nextPosition.x > 0) //오른쪽
        {
            GetComponent<SpriteRenderer>().flipX = false;
            _animator.SetBool("isWalk", true);
        }
        else //움직이지 않을 경우
        {
            _animator.SetBool("isWalk", false);
        }
        
    }

    void OnMouseDown()//마우스 클릭 시
    {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (rayhit.collider != null) //ray가 콜라이더와 충돌 시
            {
                if (_exp < gameManager.maxExp) ++_exp; //클릭하면 경험치 1씩 증가
                gameManager.JelatinChange(_id, _level); //젤라틴 값 증가
                StartCoroutine(Pause()); //젤리 이동 멈춤
                
            }
            
    }

    IEnumerator Pause() //즉시 이동 멈추고 Touch 애니메이션
    {
        clickPause = true;
        _animator.SetBool("isWalk", false);
        _animator.SetTrigger("doTouch");
        yield return new WaitForSeconds(1);
        clickPause = false;
    }

    void Exp() //시간이 지나면 자동으로 쌓이는 경험치
    {
        if (_exp < gameManager.maxExp) _exp += Time.deltaTime;
        if (_exp > (50 * _level) && _level < 3) //레벨 바뀔 때마다 애니메이터 바뀜
        {
            gameManager.ChangeAc(_animator, ++_level);
        }
    }

}
