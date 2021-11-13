using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        [SerializeField] private Interactable interactable; // the interactable component of the game object the player is colliding with that they can interact with
        public bool canInteract
        {
            get
            {
                return interactable != null;
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

            // add movement hooks
            ToggleMovementHooks(true);
        }

        public void ToggleMovementHooks(bool add)
        {
            if (add)
            {
                InputManager.instance.horizontalKeyHeld += HorizontalMoveAnimation;
                InputManager.instance.horizontalKeyReleased += StopHorizontalAnimation;
                InputManager.instance.fixedHorizontalKeyHeld += HorizontalMovement;
            }
            else
            {
                InputManager.instance.horizontalKeyHeld -= HorizontalMoveAnimation;
                InputManager.instance.horizontalKeyReleased -= StopHorizontalAnimation;
                InputManager.instance.fixedHorizontalKeyHeld -= HorizontalMovement;
            }
        }

        // Update is called once per frame
        void Update()
        {
            ProcessInteraction();
        }

        private void HorizontalMoveAnimation(object sender, InputManager.KeyPressedEventArgs e)
        {
            if (!canMove) return;

            // walk animation
            animator.SetTrigger("walking");

            // flip
            if (e.key == InputManager.instance.right && !facingRight)
                Flip();
            else if (e.key == InputManager.instance.left && facingRight)
                Flip();
        }

        private void StopHorizontalAnimation(object sender, InputManager.KeyPressedEventArgs e)
        {
            if (!canMove) return;

            animator.ResetTrigger("walking");
            animator.SetTrigger("idle");
        }

        private void HorizontalMovement(object sender, InputManager.KeyPressedEventArgs e)
        {
            int horizontalVelocity = e.key == InputManager.instance.left ? -1 : 1;
            transform.Translate(horizontalVelocity * movementSpeed * Time.deltaTime, 0, 0);
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
            if (collision.CompareTag("interactable") && collision.TryGetComponent(out Interactable i))
            {
                interactable = i;
            }
            //Debug.Log("on trigger enter " + collision.gameObject.name);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // compare the object we're leaving the trigger of, and if it's the interactable, set it to null
            if (interactable != null && collision.gameObject == interactable.gameObject)
                interactable = null;
        }

        private void ProcessInteraction()
        {
            if (!canMove || !canInteract) return;

            // process actual interaction
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (interactable.destroyAfter) Destroy(interactable.gameObject);
                // great! let's let the event manager do the rest of the work
                EventManager.ProcessEvent(interactable.doOnInteract);
            }
        }
    }

}