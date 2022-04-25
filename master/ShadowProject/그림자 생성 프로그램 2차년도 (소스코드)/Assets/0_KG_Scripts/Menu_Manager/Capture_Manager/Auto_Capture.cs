using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Auto_Capture : MonoBehaviour
{
    // 프로그래스 바는 다시 추가해볼것 ( 세이브 옵션과 오토 옵션을 동시에 생각하고 해줘야함.)

    public GameObject function_manager;
    public GameObject Touch_Disable_Panel;
    public GameObject Player;
    Vector3 Player_Position;
    Quaternion Player_Rotation;
    public GameObject Target_Object;
    public void Auto_Shot(Capture_Class capture_options, Auto_Class auto_options)
    {
        // activate auto shot
        
        Target_Object = function_manager.GetComponent<Auto_Screenshot>().hit_obj;
        function_manager.GetComponent<Auto_Screenshot>().Cancel_Button();
        Trigger_On(true); // turn on all triggers
        Touch_Disable_Panel.SetActive(true); // activate disable panel ==> can touch nothing in program
        Player.GetComponent<KG_PlayerMove>().isSCActivated = true; // cannot push any keyword

        Player_Position = Player.transform.position; // this needs when come back
        Player_Rotation = Player.transform.rotation;

        switch (auto_options.rotation_direction)
        {
            case 0: //horizontal
                StartCoroutine(Horizontal_Run(Target_Object,auto_options,capture_options));
                break;
            case 1: //vertical
                StartCoroutine(Vertical_Run(Target_Object,auto_options,capture_options));
                break;
        }
    }

    public GameObject Build_Object;
    MeshCollider[] Build_Meshs;
    void Trigger_On(bool isOn) 
    {
        // activate all building trigger on

        Build_Meshs = Build_Object.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider mesh in Build_Meshs) // turn on the trigger
            mesh.isTrigger = isOn;
    }

    public GameObject Capture_Object;
    public bool Cancel = true;
    IEnumerator Horizontal_Run(GameObject Target_Object, Auto_Class auto_options, Capture_Class capture_options)
    {
        // this function performs auto shot
        float StopRot = 0.0f;

        Capture_Object.GetComponent<Capture>().common_capture(capture_options, false);
        float lat = GetLat(Player_Position);
        float lon = GetLon(Player_Position);

        // set angle
        float angle = Get_Angle(auto_options.rotation_angle);

        Capture_Object.GetComponent<Capture>().common_capture(capture_options, false);

        while (Cancel)
        {
            if (Player.transform.GetComponent<KG_CameraTrigger>().OnTrigger == false)
            {
                Player.transform.RotateAround(Target_Object.transform.position, GetSurfacePosDeg(lat, lon), angle);
                Capture_Object.GetComponent<Capture>().common_capture(capture_options, false);
                yield return new WaitForSeconds(0.1f);
            }
            StopRot += angle;
            if (StopRot + angle >= 360.0f) break;
        }
        Auto_End();
        yield return null;
    }

    public Dropdown Rotation_Option;
    IEnumerator Vertical_Run(GameObject Target_Object, Auto_Class auto_options, Capture_Class capture_options)
    {
        function_manager.GetComponent<Auto_Screenshot>().On_Vertical(); // off rotation cube
        float Rotation_Angle = function_manager.GetComponent<Auto_Screenshot>().Rotation_Angle;
        Rotation_Option.value = 0; // set horizontal => vertical off => rotation cube activation off

        float StopRot = 0.0f;

        Capture_Object.GetComponent<Capture>().common_capture(capture_options, false);
        float lat = GetLat(Player_Position);
        float lon = GetLon(Player_Position);
        float angle = Get_Angle(auto_options.rotation_angle);

        // vertical axis
        Vector3 tempsur = GetSurfacePosDeg(lat, lon);
        Vector3 tempfor = Player.transform.forward;
        Vector3 VerticalRot = Vector3.Cross(tempsur, tempfor);
        Quaternion mtemp = Quaternion.Euler(function_manager.GetComponent<Auto_Screenshot>().Rotation_Angle, 0f, 0f);

        Capture_Object.GetComponent<Capture>().common_capture(capture_options, false); // one shot for curren position


        yield return new WaitForSeconds(0.1f);

        while (Cancel)
        {
            if (Player.transform.GetComponent<KG_CameraTrigger>().OnTrigger == false)
            {
                Player.transform.RotateAround(Target_Object.transform.position, mtemp * VerticalRot, angle);
                Capture_Object.GetComponent<Capture>().common_capture(capture_options, false);
                yield return new WaitForSeconds(0.1f);
            }
            StopRot += angle;
            if (StopRot + angle >= 120.0f) break;
        }
        Auto_End();
        function_manager.GetComponent<Auto_Screenshot>().On_Vertical();
        yield return null;
    }
    
    
    public void Cancel_Button()
    {
        // set cancel on 

        Cancel = false;
    }

    Vector3 GetSurfacePosDeg(float lat, float lon)
    {
        var height = 50.0f;
        var rotate = Quaternion.Euler(0f, -lon, lat);
        var v = rotate * new Vector3((6378137f + height), 0f, 0f);
        return v;
    }

    public GameObject Tile_Object;
    float GetLat(Vector3 currPos)
    {
        // return latitude

        float lat = 0.0f;
        Vector3 pos = currPos + Tile_Object.GetComponent<Earth>().origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);
        lat = Vector3.Angle(projVec, pos);
        return lat;
    }
    float GetLon(Vector3 currPos)
    {
        // return longitude

        float lon = 0.0f;
        Vector3 pos = currPos + Tile_Object.GetComponent<Earth>().origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);
        lon = Vector3.Angle(new Vector3(1f, 0f, 0f), projVec);
        return lon;
    }
    int Get_Angle(int value)
    {
        // return angle

        return (value+1) * 10;
    }

    void Auto_End()
    {
        Player.transform.position = Player_Position;
        Player.transform.rotation = Player_Rotation; 

        Touch_Disable_Panel.SetActive(false);
        Player.GetComponent<KG_PlayerMove>().isSCActivated = false; // activate keyboard
        //ProgressBar.value = 0; // set progress value to zero
    }

    
}
