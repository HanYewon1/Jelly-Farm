using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyController : MonoBehaviour
{
    public float speed = 5f;
    public float minX, maxX, minY, maxY;
    private Vector2 JellyPosition;
    private Vector2 nextPosition;
    private Animator _animator;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        StartCoroutine(MovePause());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = nextPosition * speed * Time.deltaTime;
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

  
    
        
    
}
