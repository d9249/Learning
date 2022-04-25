using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Facade_Rotation_Button : MonoBehaviour
{

    

    // Update is called once per frame
    void Update()
    {
        if (ZPlus)
            ZPlus_Click();
        if (ZMinus)
            ZMinus_Click();
    }

    bool ZPlus;
    bool ZMinus;
    public void ZPlus_Down() { ZPlus = true; }
    public void ZPlus_Up() { ZPlus = false; }
    public void ZMinus_Down() { ZMinus = true; }
    public void ZMinus_Up() { ZMinus = false; }

    public GameObject Player;
    public void ZPlus_Click()
    {
        Player.transform.Rotate(0f, 0f, +1.0f, Space.Self);
        Z_input.text = ((int)Player.transform.eulerAngles.z).ToString();
    }

    public void ZMinus_Click()
    {
        Player.transform.Rotate(0f, 0f, -1.0f, Space.Self);
        Z_input.text = ((int)Player.transform.eulerAngles.z).ToString();
    }

    public InputField Z_input;
    public void Z_InputField()
    {
        Quaternion Player_euler_angle = new Quaternion(0, 0, 0,0);
        Player.transform.rotation = Player_euler_angle;
        Player.transform.Rotate(0,0,float.Parse(Z_input.text),Space.Self);
    }
}
