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
        public float movementSpeed => Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // actual movement speed

        // other movement
        public bool canMove = true;
        private bool facingRight = true;

        // interaction
        [SerializeField] private GameObject interactableObject; // the game object the player is colliding with that they can interact with
        public bool canInteract
        {
            get
            {
                return interactableObject != null;
            }
        }

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

            ProcessInteraction();
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

        // interaction
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("interactable"))
            {
                interactableObject = collision.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // compare the object we're leaving the trigger of, and if it's the interactable object, set it to null
            if (collision.gameObject == interactableObject)
                interactableObject = null;
        }

        private void ProcessInteraction()
        {
            if (!canMove || !canInteract) return;

            // process actual interaction
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                Interactable interactable = interactableObject.GetComponent<Interactable>();

                // run script
                alisonscript.Interpreter.Run(interactable.script, interactable.scriptStartArgs);
            }
        }
    }

}