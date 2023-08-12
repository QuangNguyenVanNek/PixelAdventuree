using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;

    [SerializeField] private int force;

    [SerializeField] private Rigidbody2D rg;

    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private Collider2D coll;

    [SerializeField] private Animator anim;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private LayerMask EnemyLayer;

    [SerializeField] private int slideSpeed;

    [SerializeField] private int forceJumpWall;

    // [SerializeField] private TrailRenderer tr;

    private float dirX = 0f;

    private bool isGrounded;

    private bool doubleJump;

    private bool isWallR;
    private bool isWallL;
    private bool isWallU;
    private bool isWall;

    private bool EnemyR;
    private bool EnemyL;
    private bool EnemyUnder;

    private bool canRun = true;

    private bool isSliding;
    private bool isRunning;
    private bool isJumping;
    private bool isFalling;
    private bool isIdling;
    private bool isHitting;
    public bool isHit = false;
    private bool canJump = true;
    private int countJumping = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        // FlipX
        if (dirX > 0)
        {
            sprite.flipX = false;
        }
        else if (dirX < 0)
        {
            sprite.flipX = true;
        }

        // Double Jump
        isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);
        if (canJump)
        {
            if (isGrounded && !Input.GetButton("Jump"))
            {
                doubleJump = false;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded || doubleJump)
                {
                    rg.velocity = new Vector2(rg.velocity.x, force);
                    doubleJump = !doubleJump;
                    countJumping++;
                }
            }

            if (isGrounded || isSliding)
            {
                countJumping = 0;
            }
        }

        // Wall Slide
        isWallR = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.right, .1f, groundLayer);
        isWallL = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.left, .1f, groundLayer);
        isWallU = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, .1f, groundLayer);
        if (isWallR && rg.velocity.y < 0 && dirX != 0 || isWallL && rg.velocity.y < 0 && dirX != 0)
        {
            if (rg.velocity.y < slideSpeed)
            {
                rg.velocity = new Vector2(rg.velocity.x, -slideSpeed);
            }
        }

        isWallCheck();
        //JumpWall
        if (rg.velocity.y == -slideSpeed)
        {
            isGrounded = true;
            if (isGrounded && !Input.GetButton("Jump"))
            {
                doubleJump = false;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded || doubleJump)
                {
                    rg.velocity = new Vector2(rg.velocity.x, force);
                    doubleJump = !doubleJump;
                }
            }
        }

        UpdateAnim();
    }

    private void FixedUpdate()
    {
        if (canRun)
        {
            rg.velocity = new Vector2(dirX * speed, rg.velocity.y);
        }
    }

    private void UpdateAnim()
    {
        if (dirX != 0 && isGrounded && !isWallL && !isWall && canRun ||
            dirX != 0 && isGrounded && !isWallR && !isWall && canRun)
        {
            anim.Play("Run");
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (rg.velocity.y > 0 && !isGrounded && countJumping < 1 && !isHitting)
        {
            anim.Play("Jump");
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        if (rg.velocity.y < 0 && !isGrounded && !isHitting)
        {
            anim.Play("Fall");
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        if (isWall)
        {
            anim.Play("WallSlide");
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        if (isGrounded && !isWall && !isRunning && !isHitting)
        {
            anim.Play("Idle");
        }

        if (countJumping > 0 && !isFalling && !isSliding)
        {
            anim.Play("DoubleJump");
        }

        if (isHitting)
        {
            anim.Play("Hit");
        }
    }

    private void isWallCheck()
    {
        if (isWallL && dirX != 0 && !isWallU && !isGrounded && rg.velocity.y == -slideSpeed ||
            isWallR && dirX != 0 && !isWallU && !isGrounded && rg.velocity.y == -slideSpeed)
        {
            isWall = true;
        }
        else
        {
            isWall = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<EnemyBase>())
        {
            countJumping = 0;
            if (isFalling)
            {
                isHit = true;
                rg.velocity = new Vector2(rg.velocity.x, 10);
                other.gameObject.GetComponent<EnemyBase>().EnemyHurt();
                return;
            }
            if (transform.position.x < other.transform.position.x)
            {
                rg.velocity = new Vector2(-7, 10);
                isHitting = true;
            }
            else
            {
                rg.velocity = new Vector2(7, 10);
                isHitting = true;
            }
            canRun = false;
            canJump = false;
            StartCoroutine(WaitStun(0.5f));
            StartCoroutine(WaitStun2(0.3f));
        }
    }

    private IEnumerator WaitStun(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canRun = true;
        canJump = true;
    }

    private IEnumerator WaitStun2(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isHitting = false;
    }

  
}