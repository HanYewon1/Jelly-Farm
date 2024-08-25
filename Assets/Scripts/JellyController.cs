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
        while (true) //2�ʵ��� ������ ���� �� 1�� ���
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
    void RandomMove() //���� �������� �̵�
    {
        float pos_x = transform.position.x;
        float pos_y = transform.position.y;
        //���� ���� ����
        nextPosition = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        
        //�������� �̵� �� flipX ó��
        if (nextPosition.x < 0) //����
        {
            GetComponent<SpriteRenderer>().flipX = true;
            _animator.SetBool("isWalk", true);
        }
        else if(nextPosition.x > 0) //������
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
                if (_exp < gameManager.maxExp) ++_exp; //Ŭ���ϸ� ����ġ 1�� ����
                gameManager.JelatinChange(_id, _level); //����ƾ �� ����
                StartCoroutine(Pause()); //���� �̵� ����
                
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
        if (_exp < gameManager.maxExp) _exp += Time.deltaTime;
        if (_exp > (50 * _level) && _level < 3) //���� �ٲ� ������ �ִϸ����� �ٲ�
        {
            gameManager.ChangeAc(_animator, ++_level);
        }
    }

}
