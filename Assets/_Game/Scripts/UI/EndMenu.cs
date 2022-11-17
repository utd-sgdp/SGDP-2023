using System.Collections;
using System.Collections.Generic;
using Game.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
       GameScene.Load(Level.Game);
   }

   public void ReturnToHUB()
   {
       GameScene.Load(Level.Start);
   }
}  

