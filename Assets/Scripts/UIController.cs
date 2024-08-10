using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private Animator _animator;
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        if( _animator.SetBool("DoShow", true)&&Input.GetMouseButtonDown(0))
    }
}
