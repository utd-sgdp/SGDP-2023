using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Level
{
    public class SceneLoadAnimation : MonoBehaviour
    {
        [SerializeField]
        public Image LoadBar;

        private float currentValue;

        private void Start()
        {
            LoadBar.fillAmount = 0;

            //tell the GameScene script to load the scene
            GameScene.Load(SceneIndex.Game);

            //prevent the GameScene from switching to the new scene once it's loaded
            GameScene.LoadOperation.allowSceneActivation = false;
        }
        void Update() => UpdateLoadBar();
        void UpdateLoadBar()
        {
            //get the loading progress
            currentValue = GameScene.LoadOperation.progress / .9f;
            print(currentValue);

            //set the bar's fill amount to the loading progress
            LoadBar.fillAmount = currentValue;

            //if it has loaded, allow the GameScene to switch to the scene
            if (Mathf.Approximately(currentValue, 1))
            {
                GameScene.LoadOperation.allowSceneActivation = true;
            }
        }
    }
}
