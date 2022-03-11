using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public class Stairs : MonoBehaviour
    {
        [SerializeField] private Player player; // the player component if the player is on the stairs
        private EdgeCollider2D stairsCollider; // the collider of the stairs, which is stored in a child game object

        public void EnterStairs(Player player)
        {
            // this is our player
            this.player = player;
            player.onStairs = true;

            // set the player's gravity to 0
            player.SetGravity(0);
        }

        public void LeaveStairs(Player player)
        {
            // reset the player's gravity and set the player reference to null
            player.SetGravity(1);
            this.player.onStairs = false;
            this.player = null;
        }

        private void Awake()
        {
            stairsCollider = GetComponentInChildren<EdgeCollider2D>();
        }

        private void FixedUpdate()
        {
            if (player is null) return;

            // apply x/y constraints on the player while they aren't holding the movement keys to prevent sliding down the stairs
            if (!player.holdingMovementKeys)
                player.rigidbody2d.constraints |= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            else
                // disable the constraints
                player.rigidbody2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

            List<Rigidbody2D> thingsThatAreOnTheStairs = new List<Rigidbody2D> { player.rigidbody2d };
            thingsThatAreOnTheStairs.AddRange(Player.followers.Select(f => f.rigidbody2d));

            // fire a raycast down from each thing that is on the stairs
            // if the raycast hit the stairs, move the thing to the bottom of the ray hit
            foreach (Rigidbody2D r in thingsThatAreOnTheStairs)
            {
                foreach (RaycastHit2D ray in Physics2D.RaycastAll(r.position, Vector2.down))
                {
                    if (ray.transform == stairsCollider.transform)
                    {
                        r.position = new Vector3(r.position.x,
                            ray.point.y + 1,
                            r.transform.position.z);
                        break;
                    }
                }
            }
        }

        // enter/leave stairs thru the trigger
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                EnterStairs(collision.gameObject.GetComponent<Player>());
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision == player?.coll)
                LeaveStairs(player);
        }
    }
}
