// 이미지 옵션에 따른 기능 설정
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_Option : MonoBehaviour
{

    // 타일 이동시에 해당하는 것들도 생각해야함.
    // UI 간격 맞춰야함

    int current_state = 0;
    public GameObject image_option_dropdown;
    public void Image_Option_Value_Changed() 
    {
        // Image_Option value changed function

        int value = image_option_dropdown.GetComponent<Dropdown>().value; // get value
        manage_options(value);
        Back_to_Origin(current_state); // back to original mode
        Change_Environment(value); // change the mode which obtained above : value
        current_state = value; // change current state 
    }
    public GameObject image_option, sun_option, function_option, save_option, capture_option;
    public void manage_options(int value)
    {
        // disable other options when mask, texture image

        if (value > 1) // 0,1 is nothing
            allow_option_click(false);
        else
            allow_option_click(true);
    }

    void allow_option_click(bool is_true)
    {
        // this function disable or allow clicking options
        if (is_true == false)
        {
            function_option.GetComponent<Dropdown>().value = 0;
        }
        function_option.GetComponent<Dropdown>().interactable = is_true;
        save_option.GetComponent<Button>().interactable = is_true;
        capture_option.GetComponent<Button>().interactable = is_true;
    }
    
    void Back_to_Origin(int current)
    {
        // get current state and go back to original mode

        switch (current)
        {
            case 0: // current is nothing => nothing to do
                break;
            case 1: // current is origin mode => nothing to do
                break;
            case 2: // current is mask mode=> back to origin
                Mask_Change(false);
                break;
            case 3: // current is texture mode => back to origin
                Texture_Change("Standard");
                break;
            
        }
    }

    void Change_Environment(int value)
    {
        // get value state and change mode

        switch (value)
        {
            case 0: // value is nothing => nothing to do. already back_to_origin
                break;
            case 1: // value is origin mode => nothing to do. already back_to_origin 
                break;
            case 2: // value is mask mode
                Mask_Change(true);
                break;
            case 3: // value is texture mode
                Texture_Change("Unlit/Texture");
                break;
        }
    }


    public GameObject Tile_Object, Build_Object;
    private MeshRenderer[] Tile_Meshs;
    private MeshRenderer[] Build_Meshs;

    public Material mask_material;
    public Dictionary<string, Material> MatDic = new Dictionary<string, Material>();
    public void Mask_Change(bool isOn)
    {
        // change the mask mode 

        // get all mesh renderer
        Tile_Meshs = Tile_Object.GetComponentsInChildren<MeshRenderer>(); 
        Build_Meshs = Build_Object.GetComponentsInChildren<MeshRenderer>();
        
        // change mask if isOn is True
        foreach (MeshRenderer mesh in Tile_Meshs)
        {
            if (!MatDic.ContainsKey(mesh.gameObject.name))
                MatDic.Add(mesh.gameObject.name, mesh.material); // add current material and name

            if (isOn)
                mesh.material = mask_material;
            else
                mesh.material = MatDic[mesh.gameObject.name];
        }
        foreach (MeshRenderer mesh in Build_Meshs)
        {

            if (!MatDic.ContainsKey(mesh.gameObject.name))
                MatDic.Add(mesh.gameObject.name, mesh.material);
            if (isOn)
                mesh.material = mask_material;
            else
                mesh.material = MatDic[mesh.gameObject.name];
        }        
    }
    
    public void Texture_Change(string Texture_Type)
    {
        // get renderes of object childs
        Tile_Meshs = Tile_Object.GetComponentsInChildren<MeshRenderer>();
        Build_Meshs = Build_Object.GetComponentsInChildren<MeshRenderer>();

        // change shader type of all object in unity
        foreach (MeshRenderer mesh in Tile_Meshs)
        {
            mesh.material.shader = Shader.Find(Texture_Type);
        }
        foreach (MeshRenderer mesh in Build_Meshs) 
        {
            mesh.material.shader = Shader.Find(Texture_Type);
        }
    }

}
