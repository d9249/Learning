using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자동 스크린 샷 수행 시, 취소 버튼 UI 클릭을 맡는 클래스
public class KG_CancleButton : MonoBehaviour {

    public void OnCancle() // 취소 버튼 UI를 누를 시 수행되는 함수
    {
        GameObject.Find("KG_Manager").GetComponent<KG_RayShot>().isCancleOn = true; // KG_RayShot 스크립트 속 자동스크립트 반복을 바로 취소해버림
    }

}
