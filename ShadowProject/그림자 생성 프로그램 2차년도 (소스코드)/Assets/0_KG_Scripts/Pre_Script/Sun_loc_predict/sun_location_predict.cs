using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class sun_location_predict : MonoBehaviour {

    public bool isOn = false; // Sun_loc_predict 토글 클릭 확인 여부

    public void ToggleClick()
    {
        // 클릭 시, OnGUI 함수 활성화
        if (isOn == false)
        {
            GameObject.Find("BD_Info").GetComponent<Toggle>().isOn = false;
            GameObject.Find("Make_Facade").GetComponent<Toggle>().isOn = false;
            if (GameObject.Find("KG_Manager").GetComponent<KG_RayShot>().isRayOn == true)
                GameObject.Find("KG_Manager").GetComponent<KG_RayShot>().RayOn();
            isOn = true;
        }
        else
            isOn = false;
    }

    
    Event e = null;
    public GameObject Player;
    float distance = 5.0f;
    void OnGUI()
    {
        if (isOn)
        {
            e = Event.current; // 현재 이벤트를 받아옴
            if (e.isMouse) // 만일 그게 마우스이고
            {
                if (e.clickCount == 2) // 더블 클릭이라면
                {           
                    Debug.Log("double click");
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 10000) == true && EventSystem.current.IsPointerOverGameObject() == false) // UI가 아닌 어떤 것이 맞았다면,
                    {
                        if (!hit.transform.parent.name.Contains("TileObject")) // 그게 바닥면이 아닌 건물만 선택하도록 설정
                        {
                            Debug.Log("Did Hit");
                            Debug.Log(hit.transform.name);
                            Debug.Log(hit.point);
                            Player.transform.position = hit.point; // 맞은 Point로 이동.
                            Player.transform.Translate(Vector3.back * distance); // 거기서 distance 거리만큼 뒤로 빠짐.
                            //Player.transform.LookAt(hit.point);
                        }
                    }
                }
            }
        }
    }
 
}
