using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Options : MonoBehaviour
{
    public GameObject image_option, sun_option, function_option,save_option, capture_option;
    void Allow_Other_Options(bool is_true)
    {
        image_option.GetComponent<Dropdown>().interactable = is_true;
        sun_option.GetComponent<Dropdown>().interactable = is_true;
        function_option.GetComponent<Dropdown>().interactable = is_true;
        capture_option.GetComponent<Button>().interactable = is_true;
    }

}
