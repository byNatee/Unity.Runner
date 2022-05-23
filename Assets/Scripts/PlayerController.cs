using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    private  const float _jumpSpeed = 0.4f;
    private  const float _moveSpeed = 0.2f;
    private  const float _jumpForce = 3;
    private  const float _gravity = -10;
    private  const int _laneOffset = 3;

    private Animator _animator;
    private Rigidbody _rb;
    private Vector3 _lastPos, _nextPos;
    private bool _isGround;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        SwipeManager.SwipeEvent += OnSwipe;
    }

    private void Update()
    {
        _isGround = (Physics.OverlapSphere(transform.position, 0.5f, LayerMask.GetMask("Ground")).Length != 0);
        _rb.velocity = new Vector3(_rb.velocity.x, _gravity, _rb.velocity.z);

        if(RoadGenerator.instance.speed != 0)
            _animator.SetBool("Run", true);

        gameObject.layer = IsAnimationPlaying("Roll") ? 9 : 8;


    }

    private void OnSwipe(Vector2 direction)
    {
        if(RoadGenerator.instance.speed != 0)
            MovePlayer(direction);
    }
    public bool IsAnimationPlaying(string animationName)
    {
        var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsName(animationName))
            return true;

        return false;
    }

    private void MovePlayer(Vector2 dir)
    {
        if (dir == Vector2.right && transform.position.x < _laneOffset)
        {
            _lastPos = transform.position;
            _nextPos = transform.position + new Vector3(_laneOffset, 0, 0);
            transform.DOMove(_nextPos, _moveSpeed);
        }

        if (dir == Vector2.left && transform.position.x > -_laneOffset)
        {
            _lastPos = transform.position;
            _nextPos = transform.position + new Vector3(-_laneOffset, 0, 0);
            transform.DOMove(_nextPos, _moveSpeed);
        }

        if (dir == Vector2.up)
        {
            if (_isGround)
            {
                transform.DOMoveY(transform.position.y + _jumpForce, _jumpSpeed).SetEase(Ease.OutQuad);
            }
        }

        if (dir == Vector2.down)
        {
            _animator.Play("Roll");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "CantMove")
        {
            transform.DOMove(_lastPos, _moveSpeed);
        }

        if(collision.gameObject.layer == 7 || collision.gameObject.layer == 10)
        {
            Debug.Log("Death");
        }
    }
}
