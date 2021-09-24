using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Something : MonoBehaviour
{
    // keybindings
    public static KeyCode moveLeft = KeyCode.A;
    public static KeyCode moveRight = KeyCode.D;
    public static KeyCode jump = KeyCode.Space;
    public static KeyCode interact = KeyCode.Z;

    private Animator anim;
    private Rigidbody2D rb2d;

    private bool facingRight = true;
    public bool grounded = true;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // animation
        if (Input.GetKeyDown(moveLeft) && facingRight)
        {
            Flip();
        }
        else if (Input.GetKeyDown(moveRight) && !facingRight)
        {
            Flip();
        }

        if (Input.GetKeyDown(moveLeft) || Input.GetKeyDown(moveRight))
        {
            if (grounded)
                anim.SetTrigger("walk");
        }
        

        if (Input.GetKey(moveLeft) || Input.GetKeyDown(moveRight))
            return;
        else
        {
            if (Input.GetKeyUp(moveLeft) || Input.GetKeyUp(moveRight))
            {
                if (grounded)
                    anim.SetTrigger("idle");
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(moveLeft))
        {
            transform.Translate(-0.08f, 0, 0);
        }
        if (Input.GetKey(moveRight))
        {
            transform.Translate(0.08f, 0, 0);
        }

        if (Input.GetKeyDown(jump) && grounded)
        {
            anim.SetTrigger("jump");
            Vector2 jumpForce = new Vector2(0, 250);
            rb2d.AddForce(jumpForce);
            grounded = false;
        }
    }

    private void Flip ()
    {
        Vector3 scaleThing = transform.localScale;
        scaleThing.x *= -1;
        transform.localScale = scaleThing;

        facingRight = !facingRight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            grounded = true;
            anim.SetTrigger(Input.GetKey(moveLeft) || Input.GetKey(moveRight) ? "walk" : "idle");
        }
    }
}
