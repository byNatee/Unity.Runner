using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;

    private float _jumpSpeed = 0.4f;
    private float _moveSpeed = 0.2f;
    private float _jumpForce = 3;
    private float _gravity = -10;

    private int _laneOffset = 3;

    private Vector3 _lastPos, _nextPos;

    private bool _isGround;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        SwipeManager.SwipeEvent += OnSwipe;
    }

    private void Update()
    {
        _isGround = (Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Ground")).Length != 0);

        if (_isGround)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        }
        else
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _gravity, _rb.velocity.z);
        }
    }

    private void OnSwipe(Vector2 direction)
    {
        if(RoadGenerator.instance.speed != 0)
            MovePlayer(direction);
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
            if (_isGround)
                transform.DOMoveY(transform.position.y + _jumpForce, _jumpSpeed).SetEase(Ease.OutQuad);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "CantMove")
        {
            transform.DOMove(_lastPos, _moveSpeed);
        }
    }
}
