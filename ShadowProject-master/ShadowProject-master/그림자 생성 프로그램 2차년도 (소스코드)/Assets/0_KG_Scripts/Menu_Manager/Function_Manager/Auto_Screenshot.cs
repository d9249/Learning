using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Auto_Screenshot : MonoBehaviour
{
    public bool isOn = false;

    int ray_state = 0;
    public GameObject hit_obj;
    public Material hit_obj_mat;
    public Material selected_material;
    void Update()
    {
        if(isOn && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000) == true && EventSystem.current.IsPointerOverGameObject() == false && hit.transform.parent.name.Contains("TileObject") == false)
            {
                switch (ray_state)
                {
                    case 0: // somthing clicked first time
                        hit_obj = hit.transform.gameObject;
                        hit_obj_mat = hit.transform.GetComponent<MeshRenderer>().material;
                        hit.transform.GetComponent<MeshRenderer>().material = selected_material;
                        ray_state = 1;
                        break;
                    case 1:
                        if (hit_obj == null && ray_state > 0) // if player goes far => selected object disappear
                        {
                            Cancel_Button();
                            break;
                        }
                        hit_obj.GetComponent<MeshRenderer>().material = hit_obj_mat;
                        hit_obj = hit.transform.gameObject; // 새로운 객체를 넣음.
                        hit_obj_mat = hit.transform.GetComponent<MeshRenderer>().material; // hit 객체의 Material도 담음.
                        hit.transform.GetComponent<MeshRenderer>().material = selected_material; // 선택됬다는 의미의 색상을 넣어줌.
                        ray_state = 2; // 1과 2를 반복하기 위함. 
                        break;
                    case 2:
                        if (hit_obj == null && ray_state > 0)
                        {
                            Cancel_Button();
                            break;
                        }
                        hit_obj.GetComponent<MeshRenderer>().material = hit_obj_mat;
                        hit_obj = hit.transform.gameObject;
                        hit_obj_mat = hit.transform.GetComponent<MeshRenderer>().material;
                        hit.transform.GetComponent<MeshRenderer>().material = selected_material;
                        ray_state = 1;
                        break;
                }

                Auto_Ready(); // if hits, it is ready
                
            }
        }

        // for vertical option
        if(is_vertical)
        {
            if (Input.GetKey(KeyCode.Q)) // if Q
            {
                Vertical_Cube.transform.Rotate(0, 0, 1.0f, Space.Self);
                Rotation_Angle += 1.0f;
            }
            else if (Input.GetKey(KeyCode.E)) // if E
            {
                Vertical_Cube.transform.Rotate(0, 0, -1.0f, Space.Self);
                Rotation_Angle -= 1.0f;
            }
        }
    }

    public GameObject image_option, sun_option, func_option, capture_button, save_option;
    public GameObject Auto_Group;
    public GameObject capture_manager;
    
    void Auto_Ready()
    {
        image_option.GetComponent<Dropdown>().interactable = false;
        sun_option.GetComponent<Dropdown>().interactable = false;
        func_option.GetComponent<Dropdown>().interactable = false;
        save_option.GetComponent<Button>().interactable = true;
        capture_button.GetComponent<Button>().interactable = true;


        Auto_Group.SetActive(true);
    }

    public void Cancel_Button()
    {
        if (hit_obj_mat != null)
        {
            hit_obj.GetComponent<MeshRenderer>().material = hit_obj_mat;
            hit_obj = null;
            hit_obj_mat = null;
            ray_state = 0;
        }
        image_option.GetComponent<Dropdown>().interactable = false;
        sun_option.GetComponent<Dropdown>().interactable = true;
        func_option.GetComponent<Dropdown>().interactable = true;
        save_option.GetComponent<Button>().interactable = false;
        capture_button.GetComponent<Button>().interactable = false;
        Rotation_Option.GetComponent<Dropdown>().value = 0;
        Auto_Group.SetActive(false);
    }

    public GameObject Rotation_Option;
    bool is_vertical = false;
    public GameObject Vertical_Cube;
    public GameObject Vertical_Text;
    public float Rotation_Angle = 0;
    public void On_Vertical()
    {
        Rotation_Angle = 0;
        int value = Rotation_Option.GetComponent<Dropdown>().value;
        switch (value)
        {
            case 0: // horizontal
                is_vertical = false;
                Vertical_Text.SetActive(false);
                Vertical_Cube.SetActive(false);
                break;
            case 1:// vertical
                is_vertical = true;
                Vertical_Text.SetActive(true);
                Vertical_Cube.SetActive(true);
                break;
        }
    }    
}
