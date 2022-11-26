using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Objective_View : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text details;

    public void SetTitle(string str)
    {
        title.text = str;
    }

    public void SetDetail(string str)
    {
        details.text = str;
    }
}
