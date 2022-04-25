using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Sun_Option : MonoBehaviour
{

    // 시간 조절 기능은 태양 없으면 꺼져야함.
    //  UI 위치등을 수정볼 것

    void Start()
    {
        Dark_Change(); // set default sun mode
        Virtual_Sun.GetComponent<Light>().color = Real_Sun.GetComponent<Light>().color; // put same color of real sun
    }

    int current_state = 0;
    public GameObject sun_option_dropdown;
    public void Sun_Option_Value_Changed()
    {
        // Sun_Option value changed function

        int value = sun_option_dropdown.GetComponent<Dropdown>().value; // get value of dropdown
        Back_to_Origin(current_state); // back to origin mode
        Change_Sun(value); // change sun mode
        current_state = value; // change current state
    }

    public GameObject Real_Sun, Virtual_Sun;
    void Back_to_Origin(int current)
    {
        // turn back to original sun mode with nothing
        switch (current)
        {
            case 0: // nothing to do
                break;
            case 1: // current is Real sun => back to dark
                Real_Sun_Change(false);
                break;
            case 2: // current is virtual sun => back to dark
                Virtual_Sun_Change(false);
                break;
        }

    }
    

    void Change_Sun(int value)
    {
        // change mode of sun

        switch (value)
        {
            case 0: // no sun mode => to dark
                Dark_Change();
                break;
            case 1: // real sun mode
                Real_Sun_Change(true);
                break;
            case 2: // virtual sun mode
                Virtual_Sun_Change(true);
                break;
        }
    }


    void Dark_Change()
    {
        Real_Sun.SetActive(true);
        Real_Sun.GetComponent<Light>().intensity = 0;
    }

    public GameObject Sun_Group;
    public GameObject Intensity_Slider;
    void Real_Sun_Change(bool to_real)
    {
        // turn on or off the real sun
        
        Real_Sun.SetActive(to_real);
        Sun_Group.SetActive(to_real);
        Intensity_Slider.SetActive(to_real);
        Real_Sun.GetComponent<Light>().intensity = Intensity_Slider.GetComponent<Slider>().value;

    }

    public GameObject Virtual_Group;
    void Virtual_Sun_Change(bool to_virtual)
    {
        // turn on or off the virtual sun

        Virtual_Sun.SetActive(to_virtual);
        Virtual_Group.SetActive(to_virtual);
        Intensity_Slider.SetActive(to_virtual);
        Virtual_Sun.GetComponent<Light>().intensity = Intensity_Slider.GetComponent<Slider>().value;
        if (to_virtual)
            // 시간 조절 기능 못하도록 해야함.
            Debug.Log("");

    }


    float rot_speed = 10.0f; // rot speed of virtual sun
    void Update()
    {
        // rotate virtual sun if clicked the button
        if (xb) 
            Virtual_Sun.transform.Rotate(rot_speed* Time.deltaTime, 0.0f, 0.0f);
        if (yb)
            Virtual_Sun.transform.Rotate(0.0f, -rot_speed * Time.deltaTime, 0.0f);
        if (_xb)
            Virtual_Sun.transform.Rotate(-rot_speed * Time.deltaTime, 0.0f, 0.0f);
        if (_yb)
            Virtual_Sun.transform.Rotate(0.0f, rot_speed * Time.deltaTime, 0.0f);
    }

    bool xb, yb, _xb, _yb = false; // virtual sun moving button
    // the botton state functions
    public void OnxbtDown() { xb = true; }
    public void OnxbtUp() { xb = false; }
    public void OnybtDown() { yb = true; }
    public void OnybtUp() { yb = false; }
    public void On_xbtDown() { _xb = true; }
    public void On_xbtUp() { _xb = false; }
    public void On_ybtDown() { _yb = true; }
    public void On_ybtUp() { _yb = false; }
}
