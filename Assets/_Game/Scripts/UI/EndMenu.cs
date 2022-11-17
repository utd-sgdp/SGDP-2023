using System.Collections;
using System.Collections.Generic;
using Game.Level;
using Game.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    static Scene _currentScene => SceneManager.GetActiveScene();

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

   public void RestartRun()
   {
       GameScene.Load(SceneIndex.Game);
   }

   public void ReturnToHUB()
   {
       GameScene.Load(SceneIndex.Start);
   }
}  

