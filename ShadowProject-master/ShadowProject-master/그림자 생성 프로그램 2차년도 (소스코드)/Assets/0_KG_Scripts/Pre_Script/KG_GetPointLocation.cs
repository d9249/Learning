using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KG_GetPointLocation : MonoBehaviour
{
    public Camera cam;
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭을 했을 때
        {
            if (Physics.Raycast(ray, out hit, 100) == true) // 빔에 어떤 객체가 맞고 && 어떠한 UI 위에 있지 않을 때
            {
                Debug.Log("hit.point = " +hit.point);
                Debug.Log("hit.transform.position = " + hit.transform.position);

                Debug.Log("뺀 결과 = " + (hit.transform.position - hit.point));
            }
        }
    }
}
