using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Capture_Option : MonoBehaviour
{
    // 특이한 경우는 자동 스크린샷 / Facade 샷
    
    int value = 0;
    public GameObject Function_Option;
    public Dropdown Auto_Sun_Option;
    public void Button_Clicked()
    {
        Capture_Class capture_options = new Capture_Class();
        fill_options(capture_options);
        capture_options.print_all(); // just for debuging

        value = Function_Option.GetComponent<Dropdown>().value;// check if it is facade or auto screenshot
        switch (value)
        {
            case 3: // facade shot
                gameObject.GetComponent<Capture>().common_capture(capture_options, true);
                break;
            case 4: // auto screenshot
                Auto_Class auto_options = new Auto_Class();
                fill_auto_options(auto_options);
                gameObject.GetComponent<Auto_Capture>().Auto_Shot(capture_options,auto_options);
                break;
            case 5: // auto sun capture
                if(Auto_Sun_Option.value == 0)
                    gameObject.GetComponent<Auto_Sun_Capture>().Sun_Shot("Virtual",capture_options);
                else
                    gameObject.GetComponent<Auto_Sun_Capture>().Sun_Shot("Real", capture_options);
                break;
            case 6: // detection activate
                gameObject.GetComponent<Deep_Learning_Module>().Activate_Detection_CPD();
                break;
            case 7: // detection activate
                gameObject.GetComponent<Deep_Learning_Module>().Activate_Detection_UNet();
                break;
            case 8: // removal activate
                gameObject.GetComponent<Deep_Learning_Module>().Activate_Removal_CGAN();
                break;
            default: //general shot
                gameObject.GetComponent<Capture>().common_capture(capture_options,false);
                break;
        }
    } 

    public GameObject orig,mask,texture,hmap,hmap2,pix,xres,yres;
    void fill_options(Capture_Class save)
    {
        // fill save options into class

        save.original_image = orig.GetComponent<Toggle>().isOn;
        save.mask_image = mask.GetComponent<Toggle>().isOn;
        save.texture_image = texture.GetComponent<Toggle>().isOn;
        save.height = hmap.GetComponent<Toggle>().isOn;
        save.height2 = hmap2.GetComponent<Toggle>().isOn;
        save.pixel_location = pix.GetComponent<Toggle>().isOn;
        save.x_res = resolution_option(xres.GetComponent<Dropdown>().value);
        save.y_res = resolution_option(yres.GetComponent<Dropdown>().value);
    }


    public GameObject rot_dir, angle;
    void fill_auto_options(Auto_Class auto)
    {
        auto.rotation_direction = rot_dir.GetComponent<Dropdown>().value;
        auto.rotation_angle = angle.GetComponent<Dropdown>().value;
    }

    int resolution_option(int value)
    {
        return 400 + (value*200);
    }

}
