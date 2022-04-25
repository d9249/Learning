using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
public class Depth : MonoBehaviour
{
    public Material mat;
    
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
        Graphics.Blit(source, destination, mat);
    }
}
