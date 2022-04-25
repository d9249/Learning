using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Predict_Sun_Location : MonoBehaviour
{

    public GameObject Player;
    Event e = null;
    float distance = 5.0f;
    public bool isOn = false;

    void OnGUI()
    {
        // get double click and activate function
        if (isOn)
        {
            e = Event.current;
            if (e.isMouse && e.clickCount == 2) // have to be mouse and double clicked
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 10000) == true && EventSystem.current.IsPointerOverGameObject() == false) // if hits and if it is not on ui
                {
                    if (!hit.transform.parent.name.Contains("TileObject")) // cannot select tile objects
                    {
                        Player.transform.position = hit.point; // move to point
                        Player.transform.Translate(Vector3.back * distance); // move backword
                    }
                }
            }

        }
    }    
}
