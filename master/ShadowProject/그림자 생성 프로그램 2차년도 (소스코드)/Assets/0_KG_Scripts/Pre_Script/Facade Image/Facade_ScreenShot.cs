using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Facade_ScreenShot : MonoBehaviour
{
    public Text FacadeWidth;
    public Text FacadeHeight;
    public Text FacadeName;

    bool Check() // 사이즈, 이름 제대로 되어있는 지 확인하는 함수
    {
        // 각 에러마다 에러 메세지 출력
        int result = 0;
        if (FacadeWidth.text == "" || FacadeWidth.text =="0" || int.TryParse(FacadeWidth.text, out result) == false) // 0을 체크하고 없는 것을 체크하고, 문자가 아닌 것을 체크한다.
        {
            Error.SetActive(true);
            StartCoroutine(error("WIDTH IS MISSED"));
            return false;
        }
        else if (FacadeHeight.text == ""|| FacadeHeight.text == "0" || int.TryParse(FacadeHeight.text, out result) == false)
        {
            Error.SetActive(true);
            StartCoroutine(error("HEIGHT IS MISSED"));
            return false;
        }
        else if (FacadeName.text == "")
        {
            Error.SetActive(true);
            StartCoroutine(error("FILE NAME IS MISSED"));
            return false;
        }
        else
            return true;
        

    }

    public Camera skycamera;
    public Camera camera;
    public Toggle PixelOption;

    public Camera TMP_Camera; // 테스팅용
    public void ScreenShot()
    {
        if (Check())
        {
            // 한번 더 모든 물체를 꺼줌. 혹시 모르니까
            /*
            GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Save_Origin();// 다 끄고 hit_obj만 살려야함.
            
            int resWidth = int.Parse(FacadeWidth.text);//Screen.width; // 해상도 설정 
            int resHeight = int.Parse(FacadeHeight.text); // Screen.height;
            string file_path = Application.dataPath + "/../Facade_Image"; // Asset 밖 Facade 폴더
            System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(file_path); // 디렉토리 없으면 생성
            if (!d.Exists)
                d.Create();
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
            filename = string.Format("{0}/{1}.jpg", file_path, FacadeName.text);
            System.IO.File.WriteAllBytes(filename, bytes);
            if (PixelOption.isOn) // 픽셀 옵션이 켜져 있다면 픽셀 옵션함.
                GameObject.Find("KG_Manager").GetComponent<KG_PixelOption>().Pixel_height_Option(filename,resWidth,resHeight);
            */
            // 테스팅 카메라
            GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Save_Origin();// 다 끄고 hit_obj만 살려야함.

            int resWidth = int.Parse(FacadeWidth.text);//Screen.width; // 해상도 설정 
            int resHeight = int.Parse(FacadeHeight.text); // Screen.height;
            string file_path = Application.dataPath + "/../Facade_Image"; // Asset 밖 Facade 폴더
            System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(file_path); // 디렉토리 없으면 생성
            if (!d.Exists)
                d.Create();
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            skycamera.targetTexture = rt;
            TMP_Camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            skycamera.Render();
            TMP_Camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            skycamera.targetTexture = null;
            TMP_Camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);

            byte[] bytes = screenShot.EncodeToJPG();
            Destroy(screenShot);
            string filename = null;
            filename = string.Format("{0}/{1}.jpg", file_path, FacadeName.text);
            System.IO.File.WriteAllBytes(filename, bytes);
            if (PixelOption.isOn) // 픽셀 옵션이 켜져 있다면 픽셀 옵션함.
            {
                filename = string.Format("{0}/{1}", file_path, FacadeName.text);
                GameObject.Find("KG_Manager").GetComponent<KG_PixelOption>().Pixel_height_Option(filename, resWidth, resHeight);

            }




            GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Replace_All();
            // 마지막으로 원상복귀 코드

        }
    }

    public GameObject Error;
    IEnumerator error(string error) // 에러 메세지 출력 함수
    {
        
        float fading = 0.0f; // 서서히 사라지게 하기 위한 fade 변수
        float fadingTime = 0.001f; // 서서히 사라지게 하기 위해 fade 변수를 빼는 시간 변수
        while (true)
        {
            Error.GetComponent<Text>().text = error;
            Error.GetComponent<Text>().color = new Color(255, 255, 255, 1 - fading); // 에러메세지 객체의 Color의 투명도 값에서 fading 값을 뺀다
            fading += 0.01f; // fading 변수 값을 올려서 다음 반복때 좀 더 투명하게 하게끔 빼줌
            yield return new WaitForSeconds(fadingTime); // fadingTime만큼 기다렸다가 반복
            if (Error.GetComponent<Text>().color.a < 0.0f) break; //만일 투명도가 0.0f보다 떨어졌을 경우에는 그만 반복
        }
        Error.GetComponent<Text>().color = new Color(255, 255, 255, 1); // 다음에 호출할 에러메세지를 위해 감마값을 원상태로 복구
        Error.SetActive(false); // 수행을 끝냈기에 처음처럼 비활성화시킨다
        yield return null;
    }
}

