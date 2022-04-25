using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lat_Lon_Text : MonoBehaviour
{
    public Text Lat, Lon;
    float lat, lon;
    public GameObject Player;
    public GameObject TileObject;
    void LateUpdate() 
    {
        // update lat and lon

        Lat.text = "Lat : " + GetLat(Player.transform.position); 
        Lon.text = "Lon : " + GetLon(Player.transform.position);
    }

    float GetLat(Vector3 currPos) // 현재위치의 위도를 반환 ( Earth.cs 에서 그대로 가져온 것 )
    {
        float lat = 0.0f;
        Vector3 pos = currPos + TileObject.GetComponent<Earth>().origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);
        lat = Vector3.Angle(projVec, pos);
        return lat;
    }
    float GetLon(Vector3 currPos) // 현재위치의 경도를 반환하는 함수 ( Earth.cs 에서 그대로 가져 온 것 )
    {
        float lon = 0.0f;
        Vector3 pos = currPos + TileObject.GetComponent<Earth>().origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);
        //lat = Vector3.Angle(projVec, pos);
        lon = Vector3.Angle(new Vector3(1f, 0f, 0f), projVec);
        return lon;
    }
}
