using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Function_Option : MonoBehaviour
{
    int current_state = 0;
    public GameObject function_option_dropdown;
    public void Function_Option_Value_Changed()
    {
        // Function_Option value changed function

        int value = function_option_dropdown.GetComponent<Dropdown>().value;
        Stop_Function(current_state); // stop current state and off
        Start_Function(value); // start the function of value
        current_state = value; // change current state
    }

    void Stop_Function(int current)
    {
        // Stop certain function which running now
        
        switch (current)
        {
            case 0: // nothing to do
                break;
            case 1: // stop sun predict location function
                Predict_Case(true);
                gameObject.GetComponent<Predict_Sun_Location>().isOn = false;
                break;
            case 2:
                gameObject.GetComponent<Building_Information>().isOn = false;
                break;
            case 3:
                Facade_Case(true);
                gameObject.GetComponent<Get_Facade_Image>().isOn = false;
                break;
            case 4:
                Auto_Case(true);
                gameObject.GetComponent<Auto_Screenshot>().isOn = false;
                break;
            case 5: // auto sun
                Auto_Sun_Case(true);
                break;
            case 6: // detection cpd
                Deep_Learning_Module_Case(true);
                break;
            case 7: // detection unet
                Deep_Learning_Module_Case(true);
                break;
            case 8: // removal cgan
                Deep_Learning_Module_Case(true);
                break;
            case 9:
                break;


        }
    }

    void Start_Function(int value)
    {
        // Start certain function from now

        switch (value)
        {
            case 0: // nothing to do
                break;
            case 1: // start sun predict location function
                Predict_Case(false);
                gameObject.GetComponent<Predict_Sun_Location>().isOn = true;
                break;
            case 2:
                gameObject.GetComponent<Building_Information>().isOn = true;
                break;
            case 3:
                Facade_Case(false);
                gameObject.GetComponent<Get_Facade_Image>().isOn = true;
                break;
            case 4:
                Auto_Case(false);
                gameObject.GetComponent<Auto_Screenshot>().isOn = true;
                break;
            case 5:
                Auto_Sun_Case(false);
                break;
            case 6: // detection cpd
                Deep_Learning_Module_Case(false);
                break;
            case 7: // detection unet
                Deep_Learning_Module_Case(false);
                break;
            case 8: // removal cgan
                Deep_Learning_Module_Case(false);
                break;
            case 9: 
                break;
        }
    }

    void Predict_Case(bool isOn)
    {
        sun_option.GetComponent<Dropdown>().value = 1;
        sun_option.GetComponent<Dropdown>().interactable = isOn;
    }

    public GameObject image_option, sun_option, func_option,capture_button,save_option;
    public GameObject orig_image, mask_image, texture_image;
    int sun_state = 0;
    void Facade_Case(bool isOn)
    {
        // set some functions disable or not 

        // menu options
        image_option.GetComponent<Dropdown>().interactable = isOn;
        sun_option.GetComponent<Dropdown>().interactable = isOn;
        capture_button.GetComponent<Button>().interactable = isOn;

        // save options
        mask_image.GetComponent<Toggle>().interactable = isOn;
        texture_image.GetComponent<Toggle>().interactable = isOn;
        save_option.GetComponent<Button>().interactable = isOn;
        if (isOn == false)
        {
            // off Toggle
            mask_image.GetComponent<Toggle>().isOn = isOn;
            texture_image.GetComponent<Toggle>().isOn = isOn;

            sun_state = sun_option.GetComponent<Dropdown>().value;
            sun_option.GetComponent<Dropdown>().value = 0;
        }
        else
        {
            gameObject.GetComponent<Get_Facade_Image>().Convex_On(isOn);
            sun_option.GetComponent<Dropdown>().value = sun_state;
        }
    }
    
    void Auto_Case(bool isOn)
    {
        //off menu and button

        image_option.GetComponent<Dropdown>().value = 1;
        image_option.GetComponent<Dropdown>().interactable = isOn;
        capture_button.GetComponent<Button>().interactable = isOn;
        save_option.GetComponent<Button>().interactable = isOn;
        gameObject.GetComponent<Get_Facade_Image>().Convex_On(!isOn);
    }

    public GameObject Auto_Sun_Group;
    void Auto_Sun_Case(bool isOn)
    {
        Auto_Sun_Group.SetActive(!isOn);
        image_option.GetComponent<Dropdown>().value = 1;
        image_option.GetComponent<Dropdown>().interactable = isOn;
        sun_option.GetComponent<Dropdown>().interactable = isOn;
        sun_option.GetComponent<Dropdown>().value = 2;
    }

    void Deep_Learning_Module_Case(bool isOn)
    {
        image_option.GetComponent<Dropdown>().value = 1;
        image_option.GetComponent<Dropdown>().interactable = isOn;
        save_option.GetComponent<Button>().interactable = isOn;
    }
}
