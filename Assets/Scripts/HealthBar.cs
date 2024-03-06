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
}
