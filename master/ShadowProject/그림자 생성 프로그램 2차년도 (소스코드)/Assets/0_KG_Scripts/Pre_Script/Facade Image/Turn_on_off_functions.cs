using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_on_off_functions : MonoBehaviour
{
    //필요없는 UI 들.
    public GameObject KG_Panel;
    public GameObject Image;
    public GameObject btnScreenShot;
    public GameObject btnReturn;
    public GameObject KG_LightControl;
    public GameObject KG_ShadowControl;
    public GameObject KG_LightPositionControl;
    public GameObject KG_RayOn;
    public GameObject LatText;
    public GameObject LonText;
    public GameObject KG_PixelToggle;
    public GameObject Sun_Location_Predict_toggle;
    public GameObject Make_Facde;
    public GameObject Building_Info;
    public GameObject Three_Shot;
    // Far, Close Button
    public GameObject Facade_Group; // 거리조절 버튼을 포함한 facade 관련 모든 것.

    public void turn_off_func() // Facade를 만들 때 필요없는 UI를 제거
    {
        KG_Panel.SetActive(false);
        Image.SetActive(false);
        btnReturn.SetActive(false);
        btnScreenShot.SetActive(false);
        KG_LightControl.SetActive(false);
        KG_ShadowControl.SetActive(false);
        KG_LightPositionControl.SetActive(false);
        KG_RayOn.SetActive(false);
        LatText.SetActive(false);
        LonText.SetActive(false);
        KG_PixelToggle.SetActive(true); //pixel toggle은 켜줌.
        Sun_Location_Predict_toggle.SetActive(false);
        Make_Facde.SetActive(false);
        Building_Info.SetActive(false);
        Three_Shot.SetActive(false);
        Facade_Group.SetActive(true); // 다른건 다 끄고 Facade만 켜준다.

    }

    public void turn_on_func() // Facade를 만들 때 필요없는 UI를 제거
    {
        KG_Panel.SetActive(true);
        Image.SetActive(true);
        btnReturn.SetActive(true);
        btnScreenShot.SetActive(true);
        KG_LightControl.SetActive(true);
        KG_ShadowControl.SetActive(true);
        KG_LightPositionControl.SetActive(true);
        KG_RayOn.SetActive(true);
        LatText.SetActive(true);
        LonText.SetActive(true);
        KG_PixelToggle.SetActive(true);
        Sun_Location_Predict_toggle.SetActive(true);
        Make_Facde.SetActive(true);
        Building_Info.SetActive(true);
        Three_Shot.SetActive(true);
        Facade_Group.SetActive(false);

    }
}
