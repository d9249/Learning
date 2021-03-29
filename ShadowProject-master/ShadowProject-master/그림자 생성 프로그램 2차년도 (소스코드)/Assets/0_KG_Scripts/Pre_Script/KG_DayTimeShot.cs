using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// KG_DayTimeShot 토글 UI 조작 클래스
public class KG_DayTimeShot : MonoBehaviour {

    public InputField TimeInputField; // 토글 클릭 시, 시간 입력을 할 TextInputField를 키게 하기 위한 참조변수
    public Toggle DayTimeShot; // 자동스크린샷 수행 시, 시간 옵션 UI를 보여주거나 끄기 위한 참조변수
    public bool isOn; // KG_DayTimeShot 토글을 클릭 했는 지, 안했는 지 확인하는 변수

    public void TimeToggleClicked() //KG_DayTimeShot 토글을 클릭 시, 시간 입력 TextInputField를 키고 끄는 함수
    {
        isOn = DayTimeShot.isOn; // 토글이 클릭된 현 상황을 isOn 변수에 담는다. ( 사실 DayTimeShot.isOn으로 해도 되는 데, 이상하게 안되서 이렇게 우회적으로 접근했음 )

        if (isOn) // 만약 클릭 시, 토글이 켜져 있었다면
            TimeInputField.gameObject.SetActive(true); // 시간 입력 TextInputField를 활성화하여 보여준다 
        else
            TimeInputField.gameObject.SetActive(false);
    }
}
