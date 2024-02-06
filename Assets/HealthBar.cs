using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;


    public void SetMaxHealth(float Health)
    {
        slider.maxValue = Health;
        slider.value = Health;
    }


    public void setHealth (float Health)
    {
        slider.value = Health;
    }
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
