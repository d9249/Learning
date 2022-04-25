using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 카메라가 비추고 있는 화면은 스크린샷으로 남기는 기능을 구현한 스크립트입니다.
/// 현재(2018.08.16) 하늘이미지(SkyBox)를 서브카메라에서 그려내다보니
/// 이것까지 담아내지 못해 사진에서 하늘이 검게 보이는 문제점이 있습니다.
/// </summary>
public class ScreenShot : MonoBehaviour {

    public Camera camera, skycamera;
    int resWidth, resHeight;

    public Text UIFilePath;
    public bool Is_Shadow = true;

    float lat, lon;
    float first_lat, first_lon;

    void Start () {
        // 사이즈는 알아서 생각해야함. 스크린으로 할 경우 창모드 크기에 따라 달라지기 때문.
        first_lat = PlayerPrefs.GetFloat("LATITUDE");
        first_lon = PlayerPrefs.GetFloat("LONGITUDE");
        resWidth = Screen.width;
        resHeight = Screen.height;
        UIFilePath.text = Application.dataPath + "../ScreenShot";

	}

    public GameObject textFilePath;
    public GameObject Player;
    public Toggle Threeshot;
    public void btnScreenShot()
    {

        // 세 가지 이미지 찍는 토글 킬 시, 그걸로 찍고 끝
        if (Threeshot.isOn)
        {
            GameObject.Find("Three_Shot_Manager").GetComponent<Three_Shot>().getshot();
            return;
        }
        lat = GetLat(Player.transform.position); // 찍은 Player의 현재 위치를 반환
        lon = GetLon(Player.transform.position);

        //ok 2개 고려해야함. 현재 새도우가 켜져있으면 끈것도 찍어야하고, 꺼져있으면 킨 것도 찍어야함.
        // 텍스트 창도 띄웟다가 사라지게 하여 야함.
        // 디렉토리 확인도 해야함.
        // 순서도 정해야함.
        string filePath = string.Format("{0}/../ScreenShot/{1}_{2}", Application.dataPath, lat, lon);
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
        if (!di.Exists) di.Create();
        else if (di.Exists)
        {
            int i = 0;
            while (true)
            {
                filePath = string.Format("{0}/../ScreenShot/{1}_{2}({3})", Application.dataPath, lat, lon, i);
                System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(filePath);
                if (d.Exists)
                {
                    i++;
                    continue;
                }
                else
                {
                    d.Create();
                    break;
                }
            }
        }

        int num = 0;
            string gt = "";
        if (GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().isShadowOn == false)
        {
            gt = "GT";
            SC(num,filePath,gt);
            GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
            num++;
            gt = "";
            SC(num, filePath, gt);
            GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change(); // 원래 대로 끝
        }else
        {
            gt = "";
            SC(num, filePath, gt);
            GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
            num++;
            gt = "GT";
            SC(num, filePath, gt);
            GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change(); // 원래 대로 끝
        }
        textFilePath.SetActive(true);
        StartCoroutine(FadeOff());
        
    }
    
    public Text YearValue;
    public Text MonthValue;
    public Text DayValue;
    public Text HourValue;
    public Text MinuteValue;
    void SC( int num, string filePath, string isGT)
    {
        resWidth = Screen.width; // 해상도 설정 
        resHeight = Screen.height;
        UIFilePath.text = Application.dataPath + "/../ScreenShot"; // Asset 밖 Screenshot 폴더
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        skycamera.targetTexture = rt;
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        skycamera.Render();
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        skycamera.targetTexture = null;
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToJPG();
        Destroy(screenShot);
        string filename = null;
        if (GameObject.Find("KG_Manager").GetComponent<KG_LightPositionControl>().isOn == false) // 임의 태양이 아닐 때 실제 태양의 파일명 모드
        {
            filename = string.Format("{0}/first({10},{11})_current({12},{13})_DL({3}y{4}m{5}d{6}h{7}m)_P{8}_R{9}_num_{2}_{1}.jpg", filePath, num, isGT, YearValue.text, MonthValue.text, DayValue.text, HourValue.text, MinuteValue.text
                , Player.transform.position, Player.transform.eulerAngles, first_lat, first_lon, lat, lon);
        }
        else
        {
            Light CL_Rotation = GameObject.Find("KG_Manager").GetComponent<KG_LightPositionControl>().CL;
            filename = string.Format("{0}/first({6},{7})_current({8},{9})_CL({3})_P{4}_R{5}_num_{2}_{1}.jpg", filePath, num, isGT, CL_Rotation.transform.eulerAngles, Player.transform.position, Player.transform.eulerAngles, first_lat, first_lon, lat, lon);
        }

        GameObject.Find("KG_Manager").GetComponent<KG_PixelOption>().PixelOption(filename);

        System.IO.File.WriteAllBytes(filename, bytes);

      
    }
    
    Color OriginColor = new Color(255, 255, 255, 1.0f);
    IEnumerator FadeOff()
    {

        
        float fading = 0.0f;
        while (true)
        {
            UIFilePath.color = new Color(255,255,255,1-fading);
            fading += 0.01f;
            yield return new WaitForSeconds(0.001f);

            if (UIFilePath.color.a < 0.0f)
                break;
            // 제대로 안되고 있음.
        }

        textFilePath.SetActive(false);
        UIFilePath.color = OriginColor;
        yield return null;
    }

    // 현재 경도 반환
    float GetLat(Vector3 currPos)
    {
        float lat = 0.0f;

        Vector3 pos = currPos + GameObject.Find("TileObject").GetComponent<Earth>().origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);

        lat = Vector3.Angle(projVec, pos);

        return lat;
    }

    // 현재위치의 경도를 반환
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
