using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] float _jumpForce = 200;
    [SerializeField] int _maxJumps = 2;
    [SerializeField] Transform _feet;

    Vector2 _startPosition;
    int _jumpsRemaining = 2;

    void Start()
    {
        _startPosition = transform.position;
        _jumpsRemaining = _maxJumps;
    }
    void Update()
    {
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
            rigidbody2D.AddForce(Vector2.up * _jumpForce);
            _jumpsRemaining--;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var hit = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Default"));
        if (hit != null)
        {
            _jumpsRemaining = _maxJumps;
        }
    }

    public void ResetToStart()
    {
        transform.position = _startPosition;
    }
}
