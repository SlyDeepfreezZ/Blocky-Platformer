using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] float _jumpVelocity = 200;
    [SerializeField] int _maxJumps = 2;
    [SerializeField] Transform _feet;
    [SerializeField] float _downPull = 5;
    [SerializeField] float _maxJumpDuration = 0.1f;

    Vector2 _startPosition;
    int _jumpsRemaining = 2;
    float _fallTimer;
    float _jumpTimer;

    void Start()
    {
        _startPosition = transform.position;
        _jumpsRemaining = _maxJumps;
    }
    void Update()
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        bool isGrounded = hit != null;
        
        var horizontal = Input.GetAxis("Horizontal") * _speed;
        var rigidbody2D = GetComponent<Rigidbody2D>();

        if (Mathf.Abs(horizontal) >= 1)
        {
            rigidbody2D.velocity = new Vector2(horizontal, rigidbody2D.velocity.y);
            Debug.Log($"Velocity = {rigidbody2D.velocity}");
        }

        var animator = GetComponent<Animator>();
        bool walking = horizontal != 0;
        animator.SetBool("Walk", walking);

        if (horizontal != 0)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = horizontal < 0;
        }

        if (Input.GetButtonDown("Fire1") && _jumpsRemaining > 0)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, _jumpVelocity);
            _jumpsRemaining--;
            Debug.Log($"Jumps remaining{_jumpsRemaining}");
            _fallTimer = 0;
            _jumpTimer = 0;
        }
        else if (Input.GetButton("Fire1") && _jumpTimer <= _maxJumpDuration)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, _jumpVelocity);
            _fallTimer = 0;
        }
        
        _jumpTimer += Time.deltaTime;

        if (isGrounded && _fallTimer > 0)
        {
            _fallTimer = 0;
            _jumpsRemaining = _maxJumps;
        }
        else
        {
            _fallTimer += Time.deltaTime;
            var downForce = _downPull * _fallTimer * _fallTimer;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y - downForce);
        }
    }

    internal void ResetToStart()
    {
        transform.position = _startPosition;
    }
}
