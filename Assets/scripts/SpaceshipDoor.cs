using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spacegame
{
    public class SpaceshipDoor : MonoBehaviour
    {
        private bool opening;
        private bool closing;

        private BoxCollider2D trigger;

        private void Start()
        {
            trigger = GetComponent<BoxCollider2D>();
        }

        private IEnumerator OnTriggerEnter2D(Collider2D collision)
        {
            if (closing) yield break;

            if (collision.CompareTag("Player"))
            {
                StartCoroutine(ToggleDoor(true));
            }
        }

        private IEnumerator ToggleDoor(bool open)
        {
            Debug.Log("toggle door called");

            if (open)
            {
                Debug.Log("opening door");

                opening = true;
                transform.localScale = new Vector3(1, 1, 1); // reset local scale
                while (transform.localScale.y >= 0)
                {
                    transform.localScale -= new Vector3(0, 0.05f);
                    yield return new WaitForSeconds(0.001f * Time.deltaTime);
                }
                opening = false;

                Debug.Log("finished opening door");

                // check that the player is still in the collider, which they probably aren't, and if they aren't close it
                if (!trigger.IsTouching(Controller.instance.coll))
                {
                    Debug.Log("player is no longer in door, closing");
                    yield return new WaitForSeconds(0.5f);
                    StartCoroutine(ToggleDoor(false));
                }
            }
            else
            {
                Debug.Log("closing door");

                closing = true;
                transform.localScale = new Vector3(1, 0, 1); // reset local scale y back to 0 (do not know why this needs to happen)
                while (transform.localScale.y <= 1)
                {
                    transform.localScale += new Vector3(0, 0.05f);
                    yield return new WaitForSeconds(0.001f * Time.deltaTime);
                }
                closing = false;

                Debug.Log("finished closing door");

                // check that the player is still in the collider, and if they are reopen it
                if (trigger.IsTouching(Controller.instance.coll))
                {
                    Debug.Log("player is still in door, reopening");
                    StartCoroutine(ToggleDoor(true));
                }
            }
        }

        private IEnumerator OnTriggerExit2D(Collider2D collision)
        {
            if (opening) yield break;

            if (collision.CompareTag("Player"))
            {
                StartCoroutine(ToggleDoor(false));
            }
        }
    }

}
