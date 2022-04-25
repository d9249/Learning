using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capture_Class
{
    public bool original_image = false, mask_image = false, texture_image = false;
    public bool height = false, height2 = false;
    public bool pixel_location = false;
    public int x_res = 400, y_res = 400;
    
    public void print_all()
    {
        Debug.Log("original_image = " +original_image);
        Debug.Log("mask_image : " + mask_image);
        Debug.Log("texture_image : "+ texture_image);
        Debug.Log("height : " + height);
        Debug.Log("height2 : "+height2);
        Debug.Log("pixel_location : " + pixel_location);
        Debug.Log("resolution : " + x_res +" x " + y_res);

    }
}
