using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Utility
{
    public static class GameScene
    {
        public static Scene Current => SceneManager.GetActiveScene();
        public static int CurrentIndex => Current.buildIndex;

        public static void Load(Level lvl)
        {
            SceneManager.LoadScene((int) lvl);
        }

        public static void Reload()
        {
            SceneManager.LoadScene(CurrentIndex);
        }
    }

    public enum Level
    {
        Start = 0, Game = 1, Win = 2, Lose = 3,
    }
}
