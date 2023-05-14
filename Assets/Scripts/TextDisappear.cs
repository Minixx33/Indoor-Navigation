using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisappear : MonoBehaviour
{
    public GameObject WhateverTextThingy;  //Add reference to UI Text here via the inspector
    private float timeToAppear = 2f;
    private float timeWhenDisappear;

    private bool panelEnabled = false;

    //Call to enable the text, which also sets the timer
    public void EnableText()
    {
        WhateverTextThingy.SetActive(true);
        panelEnabled = true;
        timeWhenDisappear = Time.time + timeToAppear;
    }

    //We check every frame if the timer has expired and the text should disappear
    void Update()
    {
        if (panelEnabled && (Time.time >= timeWhenDisappear))
        {
            WhateverTextThingy.SetActive(false);
        }
    }
}
