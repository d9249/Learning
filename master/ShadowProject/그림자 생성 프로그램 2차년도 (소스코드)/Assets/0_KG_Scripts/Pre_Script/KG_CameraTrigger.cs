using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KG_CameraTrigger : MonoBehaviour {

    // 카메라가 건물 내부로 들어갔을 시, 그 사진은 사실 상 현존할 수 없는 사진이므로 어떤 Object의 Trigger를 통하여 

    // 건물에 대한 Trigger에 접촉시 OnTrigger를 바꿔주고. 이것은 즉 스크린샷을 못찍게하기 위한 Kg 스크립트 중에서 쓰임.
    public bool OnTrigger = false;
	void OnTriggerEnter() // 어떠한 물체 안에 들어갔을 때를 탐지하는 내장함수
    {
        // KG_RayShot 스크립트에서 들어갔을 경우 ScreenShot 기능을 할 수 없게 하게 만든 변수 ( KG_RayShot에서 참조함)
        OnTrigger = true; // 들어갔다고 표시한다.
        
    }


    void OnTriggerExit() // 어떤 물체에 들어간 이후에 나간 것을 탐지하는 내장함수
    {
        OnTrigger = false; // 나갔다고 표시한다
    }
}
