using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public class NPC : Interactable
    {
        public Animator anim;

        public string idleAnimation = "idle";
        public string walkAnimation = "walk";

        private bool facingRight = true;

        private void Awake()
        {
            // turns out unity overloads == so i can't use ??=, that sucks
            if (anim != null)
                anim = GetComponent<Animator>();
            anim.Play(idleAnimation);
        }

        public void MoveTo(Vector2 position)
        {
            StartCoroutine("InternalMoveTo", position);
        }

        private IEnumerator InternalMoveTo(Vector2 position)
        {
            // flip if the npc isn't facing the position 
            if ((position.x > transform.position.x && !facingRight) 
                || (position.x < transform.position.x && facingRight))
                Flip();

            anim.Play(walkAnimation); // play walk animation
            // move toward position
            while ((Vector2)transform.position != position)
            {
                transform.position = Vector3.MoveTowards(transform.position, position, 5 * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            anim.Play(idleAnimation); // play idle animation
        }

        private void Flip()
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
            facingRight = !facingRight;
        }
    }
}
