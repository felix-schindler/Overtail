﻿using Overtail.PlayerModule;
using UnityEngine;

namespace Overtail.Camera
{
    /// <summary>
    /// Default camera to follow player. Just attach this to main camera and either
    /// a) Assign player GameObject to target field or...
    /// b) Add "Player"-Tag to player GameObject
    /// </summary>
    public class PlayerCamera : BasicCamera
    {
        [Header("Cooldown")]
        [SerializeField] private float refocusCooldown = 5;
        [SerializeField] private float currentCooldown = 0;

        private void Awake()
        {
            FindPlayerTag();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }
        
        /// <summary>
        /// Finds as assigns GameObject with "Player" tag as tracking target.
        /// </summary>
        public void FindPlayerTag()
        {
            DefaultTarget = GameObject.FindObjectOfType<Player>()?.gameObject
                            ?? GameObject.FindGameObjectWithTag("Player");
            if (DefaultTarget == null)
                UnityEngine.Debug.Log("[Camera] setup failed. No GameObject with <Player> tag found.");

        }

        /// <summary>
        /// Use in <see cref="LateUpdate"/>.
        /// Calls <see cref="FindPlayerTag"/> with a cooldown time.
        /// </summary>
        private void ResetPlayerTarget()
        {
            if (DefaultTarget.tag == "Player") return;

            if (currentCooldown > 0)
            {
                currentCooldown = Mathf.Max(currentCooldown - Time.deltaTime, 0);
                return;
            }

            UnityEngine.Debug.Log("[Camera] No default target found.");
            FindPlayerTag();
            currentCooldown = refocusCooldown;
        }
    }
}