using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sun_Intensity : MonoBehaviour
{   

    public Slider LightSlider = null;
    public Light Real_Sun; 
    public Light Virtual_Sun; 
    public Text Light_Text; 

    public void Intensity_Change()
    {
        // change light intensity

        Real_Sun.intensity = LightSlider.value; // put intensity in real light
        Virtual_Sun.intensity = LightSlider.value; // put intensity in virtual light
        Light_Text.text = "Intensity : " + LightSlider.value.ToString("N2"); // change text
    }
}
