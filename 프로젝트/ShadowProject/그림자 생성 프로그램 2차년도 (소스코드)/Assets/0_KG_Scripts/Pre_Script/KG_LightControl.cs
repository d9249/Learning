using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 임의태양과 원래 태양의 빛의 세기를 조절하는 Slider를 조작하는 Class
public class KG_LightControl : MonoBehaviour {

    public Slider LightSlider = null; // KG_LightControl 이라는 Slider를 참조하는 변수
    public Light LightIntencity; // 원래 태양을 참조하는 변수
    public Light KG_ControlLight; // 임의 태양을 참조하는 변수
    public Text LightIntencityText; // 슬라이더 Value에 따른 값을 표현해주는 KG_LightControl 내부에 있는 LightControlText의 Text를 변하게 해주기 위한 참조 변수

   
    public void LightSliderChange() // 슬라이더에 적용될 함수 ( 슬라이더의 값이 바뀔 때마다 적용되는 함수 )
    {
        LightIntencity.intensity = LightSlider.value; // 원래 태양의 Intensity를 슬라이더의 Value로 바꿈
        KG_ControlLight.intensity = LightSlider.value; // 임의 조작 태양의 Intensity를 슬라이더의 Value로 바꿈
        LightIntencityText.text = "빛의 세기 : " + LightSlider.value.ToString("N2"); // 참조한 Text의 text를 Slider의 Value를 String으로 만들어서 표현
    }
}
