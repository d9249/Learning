using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Far_Close_Button : MonoBehaviour
{
    public GameObject Player;
    public Vector3 perp;
    public float perp_speed = 0.1f;
    public string selected_object;
    public GameObject TMP_Camera;
    void Update()
    {
        if (far)
            Far_On_Click();
        if (close)
            Close_On_Click();
    }

    bool far;
    // Far Button
    public void Far_On_Click()
    {
        //GameObject.Find(selected_object).transform.position = new Vector3((float)GameObject.Find(selected_object).transform.position.x - (float)perp.x * perp_speed, (float)GameObject.Find(selected_object).transform.position.y - (float)perp.y * perp_speed, (float)GameObject.Find(selected_object).transform.position.z - (float)perp.z * perp_speed);
        TMP_Camera.transform.position = new Vector3((float)TMP_Camera.transform.position.x + (float)perp.x * perp_speed, (float)TMP_Camera.transform.position.y + (float)perp.y * perp_speed, (float)TMP_Camera.transform.position.z + (float)perp.z * perp_speed);
        //Player.transform.position = new Vector3((float)Player.transform.position.x + (float)perp.x * perp_speed, (float)Player.transform.position.y + (float)perp.y * perp_speed, (float)Player.transform.position.z + (float)perp.z * perp_speed);
    }

    bool close;
    public void Close_On_Click()
    {
        //GameObject.Find(selected_object).transform.position = new Vector3((float)GameObject.Find(selected_object).transform.position.x + (float)perp.x * perp_speed, (float)GameObject.Find(selected_object).transform.position.y + (float)perp.y * perp_speed, (float)GameObject.Find(selected_object).transform.position.z + (float)perp.z * perp_speed);
        TMP_Camera.transform.position = new Vector3((float)TMP_Camera.transform.position.x - (float)perp.x * perp_speed, (float)TMP_Camera.transform.position.y - (float)perp.y * perp_speed, (float)TMP_Camera.transform.position.z - (float)perp.z * perp_speed);
        //Player.transform.position = new Vector3((float)Player.transform.position.x - (float)perp.x * perp_speed, (float)Player.transform.position.y - (float)perp.y * perp_speed, (float)Player.transform.position.z - (float)perp.z * perp_speed);
    }


    public void FarDown() { far = true; }
    public void FarUp() { far = false; }
    public void CloseDown() { close = true; }
    public void CloseUp() { close = false; }
}
