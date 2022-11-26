using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public Objective_View view;
    public bool completed;
   

    void Awake()
    {
        view.SetTitle("None");
        view.SetDetail("Zilch");
    }
}
