﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KG_ScreenShot : MonoBehaviour {

    public Transform Player;
    private Vector3 PlayerOriginalPosition; // 카메라의 원위치
    private Text CameraAngleText; // 각도 입력 받을 텍스트
    private float DefaultAngle = 30.0f; // 각도 입력 안했을 시에 디폴트 값으로 30도 설정.

    private float lat; // 경도와 위도
    private float lon;

    
    public Text AroundShotText; // 버튼 텍스트를 바꾸기 위해 사용
    public Material SelectedColor; // raycast시 선택한 물체의 색상 변경을 위해 사용하는 재료

    public void OnClick()
    {
        AroundShotText.text = "Select";
        StartCoroutine(OnSelect());

    }

    IEnumerator OnSelect()
    {
        Transform TranTmp=null; // 선택한 물체
        Material MatTmp=null; // 선택한 물체의 원래 색
        bool isSelected = false;

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, 100) == true && isSelected == false )
                {
                    if(TranTmp != null)
                    {
                        TranTmp.gameObject.GetComponent<MeshRenderer>().material = MatTmp;
                    }
                    TranTmp = hit.transform;
                    MatTmp = TranTmp.gameObject.GetComponent<MeshRenderer>().material;
                    TranTmp.gameObject.GetComponent<MeshRenderer>().material = SelectedColor;
                    isSelected = true;
                }
                else if(Physics.Raycast(ray, out hit, 100) == true && isSelected == true){
                    TranTmp.gameObject.GetComponent<MeshRenderer>().material = MatTmp;
                    TranTmp = hit.transform;
                    MatTmp = TranTmp.gameObject.GetComponent<MeshRenderer>().material;
                    TranTmp.gameObject.GetComponent<MeshRenderer>().material = SelectedColor;
                    isSelected = false;
                }

            }
        }
        yield return null;
    }







    void Update()
    {
        lat = GetLat(Player.transform.position);
        lon = GetLon(Player.transform.position);
        //cam.transform.LookAt(GameObject.Find("15_279417_116099_111700000000000156200204").transform.position);
        //Player.LookAt(GameObject.Find("15_279417_116099_111700000000000156200204").transform);
        //        Player.transform.RotateAround(GameObject.Find("15_279417_116099_111700000000000156200204").transform.position, GetSurfacePosDeg(lat,lon), 10.0f * Time.deltaTime);

        //Player.transform.RotateAround(GameObject.Find("15_279417_116099_111700000000000156200204").transform.position, GetSurfacePosDeg(lat,lon), 10.0f * Time.deltaTime);
    }





    public void AroundHorizontal_ScreenShot() // 횡으로 돌면서 스크린샷
    {
        
        if (CameraAngleText.text == null) // 각도 입력 안했을 시
        {
            StartCoroutine(AH_ScreenShot(DefaultAngle));
        }
        else // 각도 입력시
        {
            StartCoroutine(AH_ScreenShot(float.Parse(CameraAngleText.text)));
        }

    }

    IEnumerator AH_ScreenShot(float angle)
    {
        int CameraNumber = 0; // 회전할 때 최소 3번이상 찍게 함. 거리를 잴때 너무 가까이서 멈추지 않기를 위함.
        Debug.Log("Screen Shot activated"); // 확인 로그
        PlayerOriginalPosition = Player.transform.position; // 찍기전에 원래 있었던 위치를 저장

        while (true)
        {
            Debug.Log("CameraNumber is " + CameraNumber);
            

        }

        yield return null;
    }

    // 회전 축 설정하기 위해 쓰는 함수
    Vector3 GetSurfacePosDeg(float lat, float lon)
    {
        var height = 50.0f;
        // Z축(위도) 및 Y축(경도)를 중심으로 회전(위도)
        var rotate = Quaternion.Euler(0f, -lon, lat);
        // 자오선 및 적도 원점 벡터
        var v = rotate * new Vector3((6378137f + height), 0f, 0f);

        return v;
    }

    /// <summary>
    /// 현재위치의 위도를 반환
    /// </summary>
    /// <param name="currPos"></param>
    /// <returns></returns>
    float GetLat(Vector3 currPos)
    {
        float lat = 0.0f;

        Vector3 pos = currPos + GameObject.Find("TileObject").GetComponent<Earth>().origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);

        lat = Vector3.Angle(projVec, pos);

        return lat;
    }

    /// <summary>
    /// 현재위치의 경도를 반환
    /// </summary>
    /// <param name="currPos"></param>
    /// <returns></returns>
    float GetLon(Vector3 currPos)
    {
        float lon = 0.0f;

        Vector3 pos = currPos + GameObject.Find("TileObject").GetComponent<Earth>().origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);

        lat = Vector3.Angle(projVec, pos);

        lon = Vector3.Angle(new Vector3(1f, 0f, 0f), projVec);

        return lon;
    }

}
