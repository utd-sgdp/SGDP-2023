using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class OptionsMenu : MonoBehaviour
    {
        GameObject previousMenu;

        void Start()
        {
            HideMenu();
        }

        public void HideMenu()
        {
            gameObject.SetActive(false);
        }

        public void ShowMenu(GameObject previousMenu)
        {
            gameObject.SetActive(true);
            this.previousMenu = previousMenu;
        }

        public void backToOtherMenu()
        {
            HideMenu();
            previousMenu.SetActive(true);
        }

    }
}
