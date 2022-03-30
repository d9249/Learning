﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 이 프로젝트는 실시간으로 시간데이터를 입력받아서 플레이어가 위치한 지역의 태양 위치값을 설정하는 것으로 제작되었습니다.
/// 프로젝트 내부를 보면 플레이어의 위치에 따른 지역 좌표값(위도, 경도)을 참조하여 실시간으로 맵을 형성하는 것을 알 수 있습니다.
/// 때문에 사용자가 메인 씬(Scene1)에서 별도로 위도, 경도 값을 입력하게 될 경우 오브젝트 생성/제거에 심각한 오류가 발생하는 것을 확인해볼 수 있었습니다.
/// 이러한 문제점으로 위도, 경도값을 별도로 입력할 수 있는 서브 씬(SetRegion)을 구성하였습니다.
/// 본 스크립트는 해당 서브 씬에서 실행되는 스크립트입니다.
/// </summary>
public class LatLonManager : MonoBehaviour {

    InputField inputLat;
    InputField inputLon;
    float tempLat, tempLon;
    void Awake()
    {
        Screen.SetResolution(1600, 1600, true);
    }
    // Use this for initialization
    void Start () {
        inputLat = GameObject.Find("LatGroup").GetComponentInChildren<InputField>();
        inputLon = GameObject.Find("LonGroup").GetComponentInChildren<InputField>();

        tempLat = PlayerPrefs.GetFloat("LATITUDE");
        tempLon = PlayerPrefs.GetFloat("LONGITUDE");
        inputLat.text = tempLat.ToString();
        inputLon.text = tempLon.ToString();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (tempLat.ToString() != inputLat.text)
        {
            tempLat = System.Single.Parse(inputLat.text);
            PlayerPrefs.SetFloat("LATITUDE", tempLat);
        }
        if (tempLon.ToString() != inputLon.text)
        {
            tempLon = System.Single.Parse(inputLon.text);
            PlayerPrefs.SetFloat("LONGITUDE", tempLon);
        }
    }

    public void GoViewer()
    {
        SceneManager.LoadScene("Main_Scene", LoadSceneMode.Single);
    }
}