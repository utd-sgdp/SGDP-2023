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
            // get load progress
            float loadProgress = GameScene.LoadOperation.progress / .9f;
            currentValue = Mathf.MoveTowards(currentValue, loadProgress, speed * Time.deltaTime);

            // update load bar
            loadBar.fillAmount = currentValue;

            // if done, open the next scene
            if (Mathf.Approximately(currentValue, 1))
            {
                GameScene.LoadOperation.allowSceneActivation = true;
            }
        }
    }
}
