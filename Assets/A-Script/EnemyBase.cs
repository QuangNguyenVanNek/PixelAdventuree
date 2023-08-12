using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rg;
    public bool isMovingR = true;
    public bool isRunning;
    public bool isMoving = true;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Transform player;
    [SerializeField] private float distCheck;
    [SerializeField] private Animator anim;
    public bool isDead = false;
    private void Update()
    {
    }

    public virtual void EnemyHurt()
    {
        isDead = true;
        rg.velocity = new Vector2(rg.velocity.x, 8);
        gameObject.GetComponent<Collider2D>().enabled = false;
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

    public virtual void EnemyMoving(float speed)
    {
        if (isMoving)
        {
            Debug.Log("Co the di chuyen");
            if (isMovingR)
            {
                Debug.Log("Dang di sang phai");
                sprite.flipX = true;
                rg.velocity = new Vector2(speed, rg.velocity.y);
            }
            else
            {
                Debug.Log("Dang di sang trai");
                sprite.flipX = false;
                rg.velocity = new Vector2(-speed, rg.velocity.y);
            }
        }
    }

    public virtual void DistToPlayer()
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

    public virtual void UpdateAnim()
    {
        if (isDead)
        {
            Debug.Log("Dead");
            anim.Play("PigHit2");
        }
    }
}