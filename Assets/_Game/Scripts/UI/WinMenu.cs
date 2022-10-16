using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinMenu : MonoBehaviour
{
      // Makes menu dissapear.
      public void hideMenu()
      {
          gameObject.SetActive(false);
      }

      // Makes menu appear on screen.
      public void showMenu()
      {
          gameObject.SetActive(true);
      }

        // Sends player back to starting level.
      public void restartLevel()
      {
          // Will load the next scene when it's ready.
          // SceneManager.LoadScene("");
          Debug.Log("Restarting run");
      }

      // Sends player back to HUB.
      public void returnToHUB()
      {
          // Will load the next scene when it's ready.
          // SceneManager.LoadScene("");
          Debug.Log("Returning to HUB");
      }
}
    

