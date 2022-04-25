using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Save_Option : MonoBehaviour
{
    // 경우의 수가 몇가지 있음. 고려해서 해야함.... 
    // 체크는 무조건 여기서 하자.

    public GameObject Save_Panel;
    public void Save_Option_Clicked()
    {

        // when " save option " button clicked]
        if (function_option.GetComponent<Dropdown>().value >= 4)
            Save_Panel.gameObject.transform.Translate(0f,-200f,0f,Space.Self);
        save_option.GetComponent<Button>().interactable = false;
        Manage_Options(false);
        Save_Panel.SetActive(true);
    }

    public void Cancel_Button()
    {
        // when cancel or save capture button is clicked when it is on
        if (function_option.GetComponent<Dropdown>().value >= 4)
            Save_Panel.gameObject.transform.Translate(0f, +200f, 0f, Space.Self);
        save_option.GetComponent<Button>().interactable = true;
        Manage_Options(true);
        Save_Panel.SetActive(false);
    }

    public GameObject image_option, sun_option, function_option, save_option, capture_option;
    public Button Auto_Moving_Cancel;
    void Manage_Options(bool is_true)
    {
        
        if (function_option.GetComponent<Dropdown>().value < 3 || function_option.GetComponent<Dropdown>().value == 5) // if not facade
        {

            image_option.GetComponent<Dropdown>().interactable = is_true;
            sun_option.GetComponent<Dropdown>().interactable = is_true;
            if (function_option.GetComponent<Dropdown>().value == 1 && is_true == true) // if predict sun function
                sun_option.GetComponent<Dropdown>().interactable = false;
            if (function_option.GetComponent<Dropdown>().value == 5 && is_true == true) // if auto sun function
            {
                image_option.GetComponent<Dropdown>().interactable = false;
                sun_option.GetComponent<Dropdown>().interactable = false;
            }
            function_option.GetComponent<Dropdown>().interactable = is_true;
        }
        if (function_option.GetComponent<Dropdown>().value == 4) //if automoving
            Auto_Moving_Cancel.interactable = is_true;
        capture_option.GetComponent<Button>().interactable = is_true;
        
    }
    
}
