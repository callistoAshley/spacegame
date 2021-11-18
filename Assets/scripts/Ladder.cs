using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public class Ladder : Interactable
    {
        public override void OnInteract()
        {
            if (Controller.instance.onLadder) return;
            Controller.instance.onLadder = true;

            // allow vertical movement
            InputManager.instance.fixedVerticalKeyHeld += Controller.instance.VerticalMovement;

            // allow leaving the ladder
            InputManager.instance.fixedHorizontalKeyHeld += LeaveLadder;

            // set the player's gravity to 0 so they can move up the ladder
            Controller.instance.SetGravity(0);
        }

        private void LeaveLadder(object sender, InputManager.KeyPressedEventArgs e)
            => LeaveLadder();

        private void LeaveLadder()
        { 
            if (!Controller.instance.onLadder) return;
            Controller.instance.onLadder = false;

            // disallow vertical movement and leaving the ladder
            InputManager.instance.fixedHorizontalKeyHeld -= LeaveLadder;
            InputManager.instance.fixedVerticalKeyHeld -= Controller.instance.VerticalMovement;

            // reset the gravity back to 1
            Controller.instance.SetGravity(1);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                LeaveLadder();
        }
    }
}
