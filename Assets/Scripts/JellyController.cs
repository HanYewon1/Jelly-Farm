using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JellyController : MonoBehaviour
{
    public GameObject shadow;
    public GameManager gameManager;
    public GameObject gameManagerObject;
    public SpriteRenderer spriteRenderer;

    public float shadow_pos_y;
    public float speed = 5f;
    public float minX, maxX, minY, maxY;
    public float _exp; // 젤리별 경험치

    public Vector3[] PointList;

    public int _id;
    public int _level;

    public bool clickPause;
    
    

    Rigidbody2D rb;

    private Vector2 nextPosition;
    private Animator _animator;
    // Start is called before the first frame update

    private void Start()
    {
        _exp = 0f;
    }
    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        gameManagerObject = GameObject.Find("GameManager");
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(MovePause());
        clickPause = false;

        Shadow();
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

    void RandomMove() //랜덤 방향으로 이동
    {
        //방향 벡터 설정
        nextPosition = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

        //왼쪽으로 이동 시 flipX 처리

        if (nextPosition.x < 0) //왼쪽
        {
            GetComponent<SpriteRenderer>().flipX = true;
            _animator.SetBool("isWalk", true);
        }
        else if (nextPosition.x > 0) //오른쪽
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
            if (_exp < gameManager.maxExp)
            {
                ++_exp; //클릭하면 경험치 1씩 증가
            }
            gameManager.JelatinChange(_id, _level); //젤라틴 값 증가
            StartCoroutine(Pause()); //젤리 이동 멈춤
            SoundManager.Instance.Sound("Touch"); //터치 시 효과음

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
        if (gameManager == null)
        {
            Debug.LogError("gameManager is null in Exp()");
            return;
        }
        if (_exp < gameManager.maxExp)
        {
            _exp += Time.deltaTime;
        }
        if (_exp > (50 * _level) && _level < 3) //레벨 바뀔 때마다 애니메이터 바뀜
        {
            gameManager.ChangeAc(_animator, ++_level);
        }
    }

    void Shadow() //jelly prefab에 맞춰 그림자 위치 조종
    {
        shadow = transform.Find("Shadow").gameObject;
        switch (_id)
        {
            case 0: shadow_pos_y = -0.05f; break;
            case 3: shadow_pos_y = -0.14f; break;
            case 6: shadow_pos_y = -0.12f; break;
            case 10: shadow_pos_y = -0.16f; break;
            case 11: shadow_pos_y = -0.16f; break;
            default: shadow_pos_y = -0.05f; break;
        }
        shadow.transform.localPosition = new Vector3(0, shadow_pos_y, 0);
    }

    void OnTriggerEnter2D(Collider2D collision) //범위 내에서만 움직일 수 있도록 설정
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            nextPosition = -nextPosition;
            
            if(nextPosition.x < 0) 
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (nextPosition.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

        }
    }

}