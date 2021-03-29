using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//임의 태양 조절하는 버튼 UI 와 회전 UI를 조작하는 Class
public class KG_LightPositionControl : MonoBehaviour{ 

    public Light DL; // 원래 태양
    public Light CL; // 임의 조작 태양

    public bool isOn = false; // 임의 태양이 현재 켜져 있는 지 확인하기 위한 변수

    GameObject DLDate; // 날짜를 조정하는 UI인 Image를 끄기 위한 변수

    public GameObject XPlus,YPlus,XMinus,YMinus; // 임의 조작 태양의 회전 기능을 담당하는 버튼 참조 ( 토글에 따라 키고 끄기 위해 )
    public GameObject timeshot; // KG_DayTimeShot을 수행할 수 없으므로, 임의 조작 기능을 켤 시에 TimeShot Toggle을 끄기 위한 참조

    void Start()
    {
        CL.color = DL.color; // 임의 조작 태양의 색과 원래 태양의 색을 초기에 똑같이 만들어 주기 위함
        DLDate = GameObject.Find("Image"); // DLDate 변수에 Image UI를 초기에 담아둔다
    }

    public void ToggleClick() // 임의 태양 토글 클릭시
    {
       
        if (isOn == false) // 임의 태양이 꺼진 상태에서 클릭했을 경우 ( 즉, 임의 태양을 켰을 경우 )
        {
            DL.gameObject.SetActive(false); // 원래 태양을 끈다
            CL.gameObject.SetActive(true); // 임의 태양을 킨다

            // X+, X- , Y+ , Y- 버튼 UI를 킨다
            XPlus.SetActive(true);
            YPlus.SetActive(true);
            XMinus.SetActive(true);
            YMinus.SetActive(true);

            DLDate.SetActive(false); // 시간 조작 UI를 끈다
            timeshot.SetActive(false); // 시간 조절 자동 스크린샷 옵션을 끈다
            isOn = true; // true로 만들어 임의 태양이 켜져있다고 바꾼다
        }
        else // 임의 태양이 켜진 상태에서 클릭 했을 경우 ( 즉 , 임의 태양을 껏을 경우 )
        {
            // 위와 반대로 적용한다.
            DL.gameObject.SetActive(true);
            CL.gameObject.SetActive(false);
            XPlus.SetActive(false);
            YPlus.SetActive(false);
            XMinus.SetActive(false);
            YMinus.SetActive(false);
            DLDate.SetActive(true);

            // 시간 조절 옵션은 만일 자동 스크린 샷 기능을 수행하는 버튼이 켜져있다면 켜지고, 아니면 끄게 한다
            if(GameObject.Find("KG_Manager").GetComponent<KG_RayShot>().isRayOn == true)
                timeshot.SetActive(true);
            else
                timeshot.SetActive(false);

            isOn = false;
        }
    }




    float CL_Rotate_value = 10.0f; // 회전 값
    void Update()
    {
        // 버튼을 누르는 것을 감시하고, 누르면 그에 맞는 회전을 하는 조건문
        if (xb) //x+를 눌렀다면
            // 임의 태양의 transform을 Rotate 함수로 회전을 시키는 데, 현재 x값을 CL_Rotate_value만큼 곱하여 회전시킨다. Position과 달리 Rotation은 곱셈으로 해야함.
            // 다만 여기서 Time.deltaTime은 일정한 CL_Rotate_value가 아닌 계속 누르고 있으면 시간의 경과에 따라 더 빨리 돌게 하기 위해 더 곱해주었다. 즉, 계속 누르면 회전이 점점 더 빨라진다.
            CL.transform.Rotate(CL_Rotate_value * Time.deltaTime, 0.0f, 0.0f);
        // 이하 동일
        if (yb)
            CL.transform.Rotate(0.0f, -CL_Rotate_value * Time.deltaTime, 0.0f);
        if (_xb)
            CL.transform.Rotate(-CL_Rotate_value * Time.deltaTime, 0.0f, 0.0f);
        if (_yb)
            CL.transform.Rotate(0.0f, CL_Rotate_value * Time.deltaTime, 0.0f);
    }


    bool xb,yb,_xb,_yb = false; // 각 x,y의 UI 버튼의 클릭 여부 체크 변수
    // 버튼이 눌렸다면 true, 올려져있다면 false로 바꾸는 함수들 ( 이하 동일 )
    public void OnxbtDown(){ xb = true; }
    public void OnxbtUp() { xb = false; }
    public void OnybtDown(){ yb = true; }
    public void OnybtUp(){ yb = false; }
    public void On_xbtDown(){ _xb = true; }
    public void On_xbtUp(){ _xb = false; }
    public void On_ybtDown(){ _yb = true; }
    public void On_ybtUp(){ _yb = false;}
}
