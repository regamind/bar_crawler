using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrunkMeter : MonoBehaviour
{
    public Slider slider;
  
    public void SetMaxHealth(float Health)
    {
        slider.maxValue = Health;
        slider.value = Health;
    }

    public void setMinDrunk(float drunkness)
    {
        slider.minValue = drunkness;
        slider.value = drunkness;
    }

    public void setDrunk(float drunk)
    {
      //  Debug.Log("set drunk called");
      //  Debug.Log(drunk);

        slider.value = drunk;
    }

    public void setHealth(float Health)
    {
        Debug.Log("set Health called");
        slider.value = Health;
    }
}
