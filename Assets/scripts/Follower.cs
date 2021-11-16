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
        public static Follower FollowerFromNPC(NPC npc)
        {
            return npc.BecomeFollower();
        }
    }
}
