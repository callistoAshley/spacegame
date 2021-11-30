﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace spacegame
{
    public class Parallax : MonoBehaviour
    {
        public ParallaxData data;
        /*[HideInInspector] */public float moveSpeedMultiplier;

        public void Init()
        {
            SpriteRenderer rend = GetComponent<SpriteRenderer>();
            rend.sprite = data.sprite;

            // stole this :p https://answers.unity.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
            if (data.scaleToScreen)
            {
                float worldScreenHeight = Camera.main.orthographicSize * 2;
                float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
                transform.localScale = new Vector3(
                    worldScreenWidth / rend.sprite.bounds.size.x,
                    worldScreenHeight / rend.sprite.bounds.size.y, 1);
            }
        }

        public void UpdateX(Controller player)
        {
            if (data.followPlayerX)
            {
                transform.position = player.transform.position;
            }
            // only move the parallax further from the player if the player has moved
            else if (player.transform.hasChanged && data.moveX)
            {
                int distanceX = player.facingRight ? 1 : -1;
                transform.position -= new Vector3(distanceX * player.movementSpeed * moveSpeedMultiplier * Time.deltaTime, 0);
            }
        }

        public void UpdateY(Controller player)
        {
            if (data.followPlayerY)
            {
                transform.position = player.transform.position;
            }
            // only move the parallax further from the player if the player has moved
            /*
            else if (player.transform.hasChanged && data.moveX)
            {
                int distanceX = player.facingRight ? 1 : -1;
                transform.position -= new Vector3(distanceX * player.movementSpeed * moveSpeedMultiplier * Time.deltaTime, 0);
            }*/
        }

        [Serializable]
        public class ParallaxData
        {
            public Sprite sprite;

            public bool followPlayerX;
            public bool followPlayerY;

            public bool moveX;
            public bool moveY;

            // add this layer (set draw mode to tiled)
            //public bool dynamicResizeX;
            //public bool dynamicResizeY;

            public bool scaleToScreen;
        }
    }
}
