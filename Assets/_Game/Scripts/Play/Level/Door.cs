using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Play.Level
{
    public class Door : MonoBehaviour
    {
        void Start()
        {
            Open();
        }

        public void Open()
        {
            gameObject.SetActive(false);
        }

        public void Close()
        {
            gameObject.SetActive(true);
        }
    }
}