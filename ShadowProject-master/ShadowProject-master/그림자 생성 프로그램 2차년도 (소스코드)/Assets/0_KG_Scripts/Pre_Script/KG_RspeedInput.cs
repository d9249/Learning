using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 자동스크린샷 수행 시, 스크린샷 하나를 찍는 속도를 조절하는 클래스
public class KG_RspeedInput : MonoBehaviour {
    
    public InputField ScSpeedInputField; // 스크린샷의 속도 입력을 받기 위한 InputField 참조 변수. ( KG_RspeedInputField 속에 있는 KG_ScSpeedInputField를 참조 )

    public void ScreenShotSpeedChange() // InputField에 값을 넣거나 바꿀때마다 계속 수행되는 함수
    {
        gameObject.GetComponent<KG_RayShot>().ScreenShotSpeed = float.Parse(ScSpeedInputField.text); // InputText의 string값 text를 float형으로 형변환을 하고 KG_RayShot의 ScreenShotSpeed에 넣어준다
    }
}
