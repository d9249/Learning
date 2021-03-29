using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KG_GetPixel : MonoBehaviour
{
    public Camera cam;
    public Camera scam;
    Texture2D texture;
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) // 좌 클릭시 수행
        {
            TestGetPixel();
        }
        
    }


    // Texture의 한 Pixel을 가져오는 함수
    private void TestGetPixel()
    {
        Vector3 viewPos = Input.mousePosition;
        Debug.Log(viewPos);
        texture = RTImage(cam);
        Color _color = texture.GetPixel((int)viewPos.x, (int)viewPos.y);
        Debug.Log(_color);
        scam.targetTexture = null;
        cam.targetTexture = null;
        RenderTexture.active = null;
    }

    Texture2D RTImage(Camera cam)
    {
        int rw = Screen.width;
        int  rh = Screen.height;
        RenderTexture rt = new RenderTexture(rw, rh, 24);
        scam.targetTexture = rt;
        cam.targetTexture = rt;
        Texture2D sh = new Texture2D(rw, rh, TextureFormat.RGB24, false);
        
        scam.Render();
        cam.Render();
        RenderTexture.active = rt;
        sh.ReadPixels(new Rect(0, 0, rw, rh), 0, 0);
        sh.Apply();
        byte[] bytes = sh.EncodeToJPG();
        System.IO.File.WriteAllBytes("Testing_Pixel_RGB", bytes);
        return sh;
    }
}
