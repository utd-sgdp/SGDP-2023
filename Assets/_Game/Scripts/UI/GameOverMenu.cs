using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    // Makes menu dissapear.
    public void HideMenu()
    {
        gameObject.SetActive(false);
    }

    // Makes menu appear on screen.
    public void ShowMenu()
    {
        gameObject.SetActive(true);
    }
}
