using UnityEngine;
using UnityEngine.UI;

namespace Game.Level
{
    public class SceneLoadAnimation : MonoBehaviour
    {
        [SerializeField]
        Image loadBar;

        [SerializeField]
        float speed = 1f;

        #if UNITY_EDITOR
        [SerializeField, ReadOnly]
        #endif
        float currentValue;

        void Update() => UpdateLoadBar();
        void UpdateLoadBar()
        {
            if (GameScene.LoadOperation == null) return;
            
            // get load progress
            float loadProgress = GameScene.LoadOperation.progress / 0.9f;
            currentValue = Mathf.MoveTowards(currentValue, loadProgress, speed * Time.deltaTime);

            // update load bar
            loadBar.fillAmount = currentValue;

            // exit, animation is still playing or scene is loading
            if (!(currentValue >= 1)) return;

            // animation is done and the next scene is ready
            // switch to the next scene
            this.enabled = false;
            GameScene.LoadOperation.allowSceneActivation = true;
        }
    }
}
