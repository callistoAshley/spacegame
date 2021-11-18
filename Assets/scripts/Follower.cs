using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public class Follower : NPC
    {
        private BoxCollider2D coll;
        public override void Awake()
        {
            base.Awake();
            coll = GetComponent<BoxCollider2D>();
        }

        public void UpdatePosition(int index)
        {
            bool right = Controller.instance.facingRight;
            Vector2 position = new Vector2(Controller.instance.onLadder 
                ? Controller.instance.transform.position.x : (right ? Controller.instance.transform.position.x - index - 1
                : Controller.instance.transform.position.x + index + 1), Controller.instance.onLadder
                ? Controller.instance.transform.position.y - index - 1 : Controller.instance.transform.position.y);

            if (coll.OverlapPoint(position)) return;
            MoveTo(position);
        }

        public static Follower FollowerFromNPC(NPC npc)
        {
            return npc.BecomeFollower();
        }
    }
}
