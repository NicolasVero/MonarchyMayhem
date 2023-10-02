using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHB : MonoBehaviour
{

    [SerializeField] private Slider slider;
    public void UpdateHealthBar(Slider slider, float currentValue, float maxValue){
        slider.value = currentValue / maxValue;
    }

}
