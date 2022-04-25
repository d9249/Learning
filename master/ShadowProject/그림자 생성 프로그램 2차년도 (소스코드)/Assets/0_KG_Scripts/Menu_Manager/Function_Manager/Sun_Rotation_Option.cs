using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sun_Rotation_Option : MonoBehaviour
{
    public Dropdown sun_option; // menu sun option
    public Dropdown sun; // rotating target sun
    public Dropdown virtual_angle;


    public InputField real_sun_time_interval;
    public Dropdown real_sun_interval_type, real_sun_interval_count;
    public void on_sun_value_changed()
    {
        int value = sun.value;
        switch(value)
        {
            case 0: // virtual sun
                sun_option.value = 2; // set on virtual sun
                virtual_angle.gameObject.SetActive(true);
                real_sun_time_interval.gameObject.SetActive(false);
                real_sun_interval_type.gameObject.SetActive(false);
                real_sun_interval_count.gameObject.SetActive(false);
                break;
            case 1: // real sun
                sun_option.value = 1;
                virtual_angle.gameObject.SetActive(false);
                real_sun_time_interval.gameObject.SetActive(true);
                real_sun_interval_type.gameObject.SetActive(true);
                real_sun_interval_count.gameObject.SetActive(true);
                break;
        }
    }
}
