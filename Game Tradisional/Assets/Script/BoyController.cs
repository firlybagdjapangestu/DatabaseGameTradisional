using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyController : MonoBehaviour
{
    public float moveSpeed = 5f; // Kecepatan karakter
    public float jumpForce = 10f; // Kekuatan lompatan
    public Transform groundCheck; // Posisi yang akan diperiksa untuk mengetahui apakah karakter menyentuh tanah
    public LayerMask groundLayer; // Layer yang akan dianggap sebagai tanah

    private Rigidbody2D rb;
    private bool isGrounded;
    private float groundCheckRadius = 0.1f;
    private bool facingRight = true; // Mengindikasikan apakah karakter menghadap ke kanan atau kiri

    private bool touchMove;
    private bool touchJump;
    private float moveInput;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        // Cek apakah karakter menyentuh tanah
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Menggerakkan karakter horizontal
        /*moveInput = Input.GetAxis("Horizontal");*/

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Mengubah arah karakter berdasarkan input
        if ((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight))
        {
            Flip();
        }

        // Lompat jika karakter menyentuh tanah dan tombol lompat ditekan
        if (isGrounded && touchJump)
        {
            animator.SetTrigger("isJumping");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

    }

    // Method untuk membalik arah karakter
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1;
        transform.localScale = characterScale;
    }

    public void TouchDown(float _direction)
    {
        animator.SetBool("IsWalk", true);
        moveInput = _direction;
        touchMove = true;
    }

    public void TouchUp()
    {
        animator.SetBool("IsWalk", false);
        moveInput = 0;
        touchMove = false;
    }

    public void TouchJumpDown()
    {
        touchJump = true;
    }
    public void TouchJumpUp()
    {
        touchJump = false;
    }
}
