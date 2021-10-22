using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spacegame
{
    public class Controller : MonoBehaviour
    {
        // speed stuff
        public float speedMultiplier = 1; // run/walk speed is always multiplied by speedMultiplier
        private float walkSpeed => 5 * speedMultiplier;
        private float runSpeed => 7 * speedMultiplier;
        public float movementSpeed => Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // movement speed

        // other movement
        public bool canMove = true;
        private bool facingRight = true;

        // components
        [HideInInspector] public Animator animator;
        [HideInInspector] public BoxCollider2D coll;

        // singleton
        public static Controller instance;

        // Start is called before the first frame update
        void Awake()
        {
            animator = GetComponent<Animator>();
            coll = GetComponent<BoxCollider2D>();
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            // walk animation
            if (Input.GetAxis("Horizontal") == 0 || !canMove)
                animator.SetTrigger("idle");
            else
                animator.SetTrigger("walking");

            // flip
            if (Input.GetAxis("Horizontal") > 0 && !facingRight)
                Flip();
            else if (Input.GetAxis("Horizontal") < 0 && facingRight)
                Flip();
        }

        private void FixedUpdate()
        {
            // move left/right
            if (canMove)
                transform.Translate(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0, 0);
        }

        private void Flip()
        {
            if (!canMove) return;

            // flip sprite by inverting x value of scale
            facingRight = !facingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

}