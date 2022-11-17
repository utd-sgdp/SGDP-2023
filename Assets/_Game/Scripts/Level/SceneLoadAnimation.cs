using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SceneLoadAnimation : MonoBehaviour
    {
        [SerializeField]
        public Image LoadBar;

        private float currentValue;

        private void Start()
        {
            LoadBar.fillAmount = 0;
            GameScene.Load(SceneIndex.Game);
            GameScene.LoadOperation.allowSceneActivation = false;
        }
        void Update() => UpdateLoadBar();
        void UpdateLoadBar()
        {
            currentValue = GameScene.LoadOperation.progress / .9f;
            print(currentValue);
            LoadBar.fillAmount = currentValue;
            if (Mathf.Approximately(currentValue, 1))
            {
                GameScene.LoadOperation.allowSceneActivation = true;
            }
        }
    }
}
