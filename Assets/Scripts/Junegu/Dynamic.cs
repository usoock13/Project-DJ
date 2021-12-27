using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic : MonoBehaviour
{
    public float movePower = 100f;
    public float jumpPower = 100f;
    public bool isJumping = false;
    public int jumpCount = 0;

    Rigidbody2D rigidbody2D;
    Animator playerAnimator;
    Collider2D collider2D;
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        playerAnimator = GameObject.Find("Player_Sprite").GetComponent<Animator>();
        collider2D = gameObject.GetComponent<Collider2D>();
    }

    void Move()
    {
        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        if(moveDirection != Vector2.zero)
        {
            if (!Physics2D.BoxCast(transform.position, collider2D.bounds.size*0.99f, 0, moveDirection, 0.05f, 192))
            {
                transform.Translate(moveDirection * movePower * Time.deltaTime);
                transform.localScale = new Vector3(moveDirection.x, 1, 1);
                playerAnimator.SetBool("Move", true);
            }
        } else
        {
            playerAnimator.SetBool("Move", false);
        }
    }
    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            if(jumpCount < 2)
            {
                rigidbody2D.velocity = Vector3.zero;
                rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                playerAnimator.SetBool("Jump", true);
                jumpCount++;
            }
        }
        Debug.DrawRay(transform.position + new Vector3(0, -1.3f, 0), Vector2.down * 0.078f, Color.red);
    }
    void GroundCheck()
    {
        Vector2 collidersize = collider2D.bounds.size;
        RaycastHit2D boxcastHit = Physics2D.BoxCast(transform.position + new Vector3(0, -1.3f, 0), collidersize, 0, Vector2.down, 0.078f, 192);
        if(boxcastHit && rigidbody2D.velocity.y < 0)
        {
            playerAnimator.SetBool("Jump", false);
            jumpCount = 0;
        }
    }

    void Update()
    {
        Move();
        Jump();
        GroundCheck();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

}
