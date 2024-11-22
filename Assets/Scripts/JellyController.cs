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
    public float _exp; // ������ ����ġ

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
        while (true) //2�ʵ��� ������ ���� �� 1�� ���
        {
            RandomMove();
            yield return new WaitForSeconds(3);
            nextPosition = Vector2.zero;
            _animator.SetBool("isWalk", false);
            yield return new WaitForSeconds(2);
        }
    }

    void RandomMove() //���� �������� �̵�
    {
        //���� ���� ����
        nextPosition = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

        //�������� �̵� �� flipX ó��

        if (nextPosition.x < 0) //����
        {
            GetComponent<SpriteRenderer>().flipX = true;
            _animator.SetBool("isWalk", true);
        }
        else if (nextPosition.x > 0) //������
        {
            GetComponent<SpriteRenderer>().flipX = false;
            _animator.SetBool("isWalk", true);
        }
        else //�������� ���� ���
        {
            _animator.SetBool("isWalk", false);
        }


    }

    void OnMouseDown()//���콺 Ŭ�� ��
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (rayhit.collider != null) //ray�� �ݶ��̴��� �浹 ��
        {
            if (_exp < gameManager.maxExp)
            {
                ++_exp; //Ŭ���ϸ� ����ġ 1�� ����
            }
            gameManager.JelatinChange(_id, _level); //����ƾ �� ����
            StartCoroutine(Pause()); //���� �̵� ����
            SoundManager.Instance.Sound("Touch"); //��ġ �� ȿ����

        }

    }

    IEnumerator Pause() //��� �̵� ���߰� Touch �ִϸ��̼�
    {
        clickPause = true;
        _animator.SetBool("isWalk", false);
        _animator.SetTrigger("doTouch");
        yield return new WaitForSeconds(1);
        clickPause = false;
    }

    void Exp() //�ð��� ������ �ڵ����� ���̴� ����ġ
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
        if (_exp > (50 * _level) && _level < 3) //���� �ٲ� ������ �ִϸ����� �ٲ�
        {
            gameManager.ChangeAc(_animator, ++_level);
        }
    }

    void Shadow() //jelly prefab�� ���� �׸��� ��ġ ����
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

    void OnTriggerEnter2D(Collider2D collision) //���� �������� ������ �� �ֵ��� ����
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