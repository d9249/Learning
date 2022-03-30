using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Get_Pixel_Value : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000) == true && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Debug.Log(hit.point);

        }
    }
}
