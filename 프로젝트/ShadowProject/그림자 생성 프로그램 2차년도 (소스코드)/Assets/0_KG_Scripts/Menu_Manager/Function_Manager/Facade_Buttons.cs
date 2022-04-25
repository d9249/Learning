using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facade_Buttons : MonoBehaviour
{
    public GameObject Player;
    public Vector3 perp;
    float perp_speed = 1f;
    public GameObject TMP_Camera;
    public GameObject Facade_Group;
    void Update()
    {
        // far close button
        if (far)
            Far_On_Click();
        if (close)
            Close_On_Click();

        // rotatino buttons
        if (ZPlus)
            ZPlus_Click();
        if (ZMinus)
            ZMinus_Click();

    }

    
    public void Far_On_Click()
    {
        // Far Button click

        TMP_Camera.transform.position = new Vector3((float)TMP_Camera.transform.position.x + (float)perp.x * perp_speed, (float)TMP_Camera.transform.position.y + (float)perp.y * perp_speed, (float)TMP_Camera.transform.position.z + (float)perp.z * perp_speed);
    }

    
    public void Close_On_Click()
    {
        // close Button Click

        TMP_Camera.transform.position = new Vector3((float)TMP_Camera.transform.position.x - (float)perp.x * perp_speed, (float)TMP_Camera.transform.position.y - (float)perp.y * perp_speed, (float)TMP_Camera.transform.position.z - (float)perp.z * perp_speed);
    }

    bool far, close;
    public void FarDown() { far = true; }
    public void FarUp() { far = false; }
    public void CloseDown() { close = true; }
    public void CloseUp() { close = false; }

    public void ZPlus_Click()
    {
        TMP_Camera.transform.Rotate(0f, 0f, +1.0f, Space.Self);
    }

    public void ZMinus_Click()
    {
        TMP_Camera.transform.Rotate(0f, 0f, -1.0f, Space.Self);
    }
    bool ZPlus;
    bool ZMinus;
    public void ZPlus_Down() { ZPlus = true; }
    public void ZPlus_Up() { ZPlus = false; }
    public void ZMinus_Down() { ZMinus = true; }
    public void ZMinus_Up() { ZMinus = false; }

    // for UI
    public GameObject ReturnButton;
    public void Facade_UI(bool isOn)
    {
        Facade_Group.SetActive(isOn); // set on Facade UI
        ReturnButton.SetActive(!isOn);
    }
}
