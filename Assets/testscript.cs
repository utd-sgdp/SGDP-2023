using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    public EndMenu endMenu;

    // Start is called before the first frame update
    void Start()
    {
        endMenu.HideMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            endMenu.ShowMenu();
        }

        if (Input.GetKeyDown("g"))
        {
            endMenu.HideMenu();
        }
        
    }
}
