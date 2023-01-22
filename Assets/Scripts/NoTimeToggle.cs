using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NoTimeToggle : MonoBehaviour
{    
    public void ToggleonValueChanged(Toggle change) 
    {
        Debug.Log(change.isOn);
        MazeRenderer.noTime = change.isOn;
    }      
}
