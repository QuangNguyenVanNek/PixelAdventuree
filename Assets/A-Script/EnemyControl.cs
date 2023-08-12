using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isMovingR = true;
    public bool isRunning;
    public bool isMoving = true;
    [SerializeField] private Rigidbody2D rg;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Transform player;
    [SerializeField] private float distCheck;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerController _player;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DistToPlayer();
        AnimUpdate();
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Turn"))
        {
            if (isMovingR)
            {
                isMovingR = false;
            }
            else
            {
                isMovingR = true;
            }
        }
    }

    void EnemyMoving(float speed)
    {
        if (isMoving)
        {
            if (isMovingR)
            {
                sprite.flipX = true;
                rg.velocity = new Vector2(speed, rg.velocity.y);
            }
            else
            {
                sprite.flipX = false;
                rg.velocity = new Vector2(-speed, rg.velocity.y);
            }
        }
    }

    void DistToPlayer()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer < distCheck && (player.position.y - transform.position.y >= 0) &&
            (player.position.y - transform.position.y <= 1))
        {
            isRunning = true;
            if (player.position.x < transform.position.x)
            {
                isMovingR = false;
            }
            else
            {
                isMovingR = true;
            }

            EnemyMoving(4f);
        }
        else
        {
            isRunning = false;
            EnemyMoving(2f);
        }
    }

    void AnimUpdate()
    {
        if (isMoving && !_player.isHit && !isRunning)
        {
            anim.Play("PigWalk");
        }

        if (_player.isHit && !isRunning)
        {
            anim.Play("PigHit2");
        }

        if (isRunning)
        {
            anim.Play("PigRun");
        }
    }
   
    
}