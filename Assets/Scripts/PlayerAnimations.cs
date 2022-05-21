using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        SwipeManager.SwipeEvent += OnSwipe;
    }

    private void Update()
    {
        if(RoadGenerator.instance.speed != 0)
        {
            _animator.SetBool("Run", true);
        }
    }
    private void OnSwipe(Vector2 direction)
    {
        if(direction == Vector2.up)
        {
            //jump animation
        }
        else if(direction == Vector2.down)
        {
            _animator.Play("Roll");
        }
    }
}
