using System.Collections;
using System.Collections.Generic;
using Game.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class StartMenu : MonoBehaviour
    {
        public OptionsMenu options;

        void Start()
        {
            ShowMenu();
        }

        public void HideMenu()
        {
            gameObject.SetActive(false);
        }

        public void ShowMenu()
        {
            gameObject.SetActive(true);
        }

        public void startGame()
        {
            GameScene.Load(SceneIndex.Game);
        }

        public void callOptions()
        {
            HideMenu();
            options.ShowMenu(previousMenu: gameObject);
        }

        public void QuitGame() => GameScene.Quit();

    }
}
