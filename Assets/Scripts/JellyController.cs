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
        while (true) //2�ʵ��� ������ ���� �� 1�� ���
        {
            RandomMove();
            yield return new WaitForSeconds(3);
            nextPosition = Vector2.zero;
            _animator.SetBool("isMove", false);
            yield return new WaitForSeconds(2);
        }
    }

    void RandomMove() //���� �������� �̵�
    {
        nextPosition = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        JellyPosition = transform.position;
        //�������� �̵� �� flipX ó��
        if (nextPosition.x < 0) //����
        {
            GetComponent<SpriteRenderer>().flipX = true;
            _animator.SetBool("isMove", true);
        }
        else if(nextPosition.x > 0) //������
        {
            GetComponent<SpriteRenderer>().flipX = false;
            _animator.SetBool("isMove", true);
        }
        else //�������� ���� ���
        {
            _animator.SetBool("isMove", false);
        }
    }

    void Click() //���콺 Ŭ�� ��
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (rayhit.collider != null) //ray�� �ݶ��̴��� �浹 ��
        {
            if (Input.GetMouseButtonDown(0)) //���콺 Ŭ�� ��
                StartCoroutine(Pause());
        } 
    }

    IEnumerator Pause() //��� �̵� ���߰� Touch �ִϸ��̼�
    {
        clickPause = true;
        _animator.SetBool("isMove", false);
        _animator.SetTrigger("isTouch");
        yield return new WaitForSeconds(1);
        clickPause = false;
    }
}
