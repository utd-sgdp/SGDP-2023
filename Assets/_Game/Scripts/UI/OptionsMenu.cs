using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class OptionsMenu : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _group;
    	
		GameObject previousMenu;
        
		private void OnValidate()
		{
			if (!_group) _group = GetComponent<CanvasGroup>();
		}
		
		private void Start()
        {
            HideMenu();
        }

        public void HideMenu()
		{
			SetGroupActive(false);
        }

        public void ShowMenu(GameObject previousMenu)
		{
			SetGroupActive(true);
            this.previousMenu = previousMenu;
        }
        
		private void SetGroupActive(bool active)
		{
			_group.alpha = active ? 1 : 0;
			_group.interactable = active;
			_group.blocksRaycasts = active;
		}

        public void backToOtherMenu()
        {
            HideMenu();
            previousMenu.SetActive(true);
        }

    }
}
