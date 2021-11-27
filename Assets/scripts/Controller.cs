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
        [HideInInspector] public bool facingRight = true;
        [HideInInspector] public bool onLadder;

        // followers
        public static List<Follower> followers = new List<Follower>();

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
        [HideInInspector] public Rigidbody2D rigidbody2d;

        // singleton
        public static Controller instance;

        // Start is called before the first frame update
        void Awake()
        {
            animator = GetComponent<Animator>();
            coll = GetComponent<BoxCollider2D>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            instance = this;

            // add movement hooks
            ToggleMovementHooks(true);
        }

        public void ToggleMovementHooks(bool add)
        {
            if (add)
            {
                InputManager.instance.AddEvent("horizontalKeyHeld", HorizontalMoveAnimation);
                InputManager.instance.AddEvent("horizontalKeyReleased", StopHorizontalAnimation);
                InputManager.instance.AddEvent("selectKeyDown", ProcessInteraction);
                InputManager.fixedHorizontalKeyHeld += HorizontalMovement;
            }
            else
            {
                InputManager.instance.RemoveEvent("horizontalKeyHeld", HorizontalMoveAnimation);
                InputManager.instance.RemoveEvent("horizontalKeyReleased", StopHorizontalAnimation);
                InputManager.instance.RemoveEvent("selectKeyDown", ProcessInteraction);
                InputManager.fixedHorizontalKeyHeld -= HorizontalMovement;
            }
        }

        private void HorizontalMoveAnimation(InputManager.KeyPressedEventArgs e)
        {
            Debug.Log("horizontal animation");
            if (!canMove) return;

            // walk animation
            animator.SetTrigger("walking");

            // flip
            if (e.key == InputManager.right && !facingRight)
                Flip();
            else if (e.key == InputManager.left && facingRight)
                Flip();
        }

        private void StopHorizontalAnimation(InputManager.KeyPressedEventArgs e)
        {
            if (!canMove) return;

            animator.ResetTrigger("walking");
            animator.SetTrigger("idle");

            // stop the followers from walking
            foreach (Follower f in followers) 
                f.StopWalking();
        }

        // horizontal movement
        public void HorizontalMovement(InputManager.KeyPressedEventArgs e)
        {
            if (!canMove) return;
            UpdateFollowers();

            int horizontalVelocity = e.key == InputManager.left ? -1 : 1;
            transform.Translate(horizontalVelocity * movementSpeed * Time.deltaTime, 0, 0);
        }

        // vertical movement
        public void VerticalMovement(InputManager.KeyPressedEventArgs e)
        {
            if (!canMove) return;
            UpdateFollowers();

            int verticalVelocity = e.key == InputManager.down ? -1 : 1;
            transform.Translate(0, verticalVelocity * movementSpeed * Time.deltaTime, 0);
        }

        private void Flip()
        {
            if (!canMove) return;

            // flip sprite by inverting x value of scale
            facingRight = !facingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        public void SetGravity(float gravity)
        {
            rigidbody2d.gravityScale = gravity;
        }

        private void UpdateFollowers()
        {
            // use i as a parameter to determine the offset from the last follower
            for (int i = 0; i < followers.Count; i++)
                followers[i].UpdatePosition(i);
        }

        // interaction
        private void ProcessInteraction(InputManager.KeyPressedEventArgs e)
        {
            if (!canMove || !canInteract || alisonscript.Interpreter.interpreterRunning) return;

            interactable.OnInteract();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((collision.CompareTag("interactable") || collision.CompareTag("npc"))
                && collision.TryGetComponent(out Interactable i) && i.doOnInteract != null)
            {
                interactable = i;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // compare the object we're leaving the trigger of, and if it's the interactable, set it to null
            if (interactable != null && collision.gameObject == interactable.gameObject)
                interactable = null;
        }
    }
}