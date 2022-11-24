using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Utility.Patterns;

namespace Game.Play.Camera
{
    public class LevelCameraController : LazySingleton<LevelCameraController>
    {
        CinemachineConfiner _confiner;

        void Awake()
        {
            _confiner = GetComponent<CinemachineConfiner>();
        }

        /// <summary>
        /// Confines <see cref="CinemachineVirtualCamera"/> to the provided room.
        /// </summary>
        /// <param name="colision"> The new confining volume. </param>
        public void SetConfiner(Collider collision)
        {
            _confiner.m_BoundingVolume = collision;
        }
    }
}
