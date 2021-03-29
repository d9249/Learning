using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;
using UnityEngine.UI;

public class Deep_Learning_Module : MonoBehaviour
{
    // Deep Learning directory
    string deep_learning_dir_path;

    string D_Test_Image_dir_path;
    string D_Test_Image_Height_dir_path;
    string D_Test_Image_Info_dir_path;
    string D_Test_Image_GT_Mask_dir_path;
    string D_Test_Image_GT_Texture_dir_path;
    string D_Detection_Result_CPD_dir_path;
    string D_Detection_Result_Unet_dir_path;
    string D_Removal_Result_CGAN_dir_path;
    

    void Start()
    {
        // set Deep Learning directory path
        deep_learning_dir_path = string.Format(Application.dataPath + "/../Deep_Learning_Image");

        D_Test_Image_dir_path = deep_learning_dir_path + "/Test_Image";
        D_Test_Image_Height_dir_path = deep_learning_dir_path + "/Test_Image_Height";
        D_Test_Image_Info_dir_path = deep_learning_dir_path + "/Test_Image_Info";
        D_Test_Image_GT_Mask_dir_path = deep_learning_dir_path + "/Test_Image_GT_Mask";
        D_Test_Image_GT_Texture_dir_path = deep_learning_dir_path + "/Test_Image_GT_Texture";
        D_Detection_Result_CPD_dir_path = deep_learning_dir_path + "/Detection_Result_CPD";
        D_Detection_Result_Unet_dir_path = deep_learning_dir_path + "/Detection_Result_UNet";
        D_Removal_Result_CGAN_dir_path = deep_learning_dir_path + "/Removal_Result_GAN";
    }

    string file_number;
    int x_res = 400, y_res = 400;
  
    string check_file_number(string dir_path)
    {
        // Check file number of recent ( standard : original image )
        int count = 0;
        System.IO.FileInfo fi;
        while (true)
        {
            fi = new System.IO.FileInfo(dir_path + "/" + count.ToString() + ".png");
            if (!fi.Exists)
                break;
            else
            {
                count += 1;
            }
        }
        return count.ToString();
    }
    void check_dir()
    {
        // Check if directory is exists. if not, make coreesponding directory

        // Deep learning directory check
        System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(deep_learning_dir_path);
        if (!d.Exists) d.Create();

        d = new System.IO.DirectoryInfo(D_Test_Image_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(D_Test_Image_Height_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(D_Test_Image_GT_Mask_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(D_Test_Image_GT_Texture_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(D_Test_Image_Info_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(D_Detection_Result_CPD_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(D_Detection_Result_Unet_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(D_Removal_Result_CGAN_dir_path);
        if (!d.Exists) d.Create();

    }
    
    public void Activate_Detection_CPD() // detection cpd
    {
        check_dir();
        file_number = check_file_number(D_Test_Image_dir_path);
        getshot_image(D_Test_Image_dir_path,file_number,1);
        getshot_image(D_Test_Image_GT_Mask_dir_path, file_number, 2);
        Image_Option.GetComponent<Dropdown>().value = 1;
        getshot_height(D_Test_Image_Height_dir_path, file_number);
        getshot_information(D_Test_Image_Info_dir_path, file_number);
        Height_Map_Run(D_Test_Image_Height_dir_path ,file_number);
        Network_Run("Detection_CPD", file_number + ".png");
        Image_Option.GetComponent<Dropdown>().value = 0;
    }

    public void Activate_Detection_UNet() // detection unet
    {
        // need to be activate Detection CPD
        check_dir();
        file_number = check_file_number(D_Test_Image_dir_path);
        getshot_image(D_Test_Image_dir_path, file_number, 1);
        getshot_image(D_Test_Image_GT_Mask_dir_path, file_number, 2);
        getshot_height(D_Test_Image_Height_dir_path, file_number);
        getshot_information(D_Test_Image_Info_dir_path, file_number);
        Height_Map_Run(D_Test_Image_Height_dir_path, file_number);
        Network_Run("Detection_CPD", file_number + ".png");
        Network_Run("Detection_Unet", file_number + ".png");
        Image_Option.GetComponent<Dropdown>().value = 0;
    }

    public void Activate_Removal_CGAN() // removal cgan ( input is test image , unet result)
    {
        // need to be activate Detection CPD and UNEt

        check_dir();
        file_number = check_file_number(D_Test_Image_dir_path);
        getshot_image(D_Test_Image_dir_path, file_number, 1);
        getshot_image(D_Test_Image_GT_Mask_dir_path, file_number, 2);
        getshot_image(D_Test_Image_GT_Texture_dir_path, file_number, 3);
        getshot_height(D_Test_Image_Height_dir_path, file_number);
        getshot_information(D_Test_Image_Info_dir_path, file_number);
        Height_Map_Run(D_Test_Image_Height_dir_path, file_number);
        Network_Run("Detection_CPD", file_number + ".png");
        Network_Run("Detection_Unet", file_number + ".png");
        Network_Run("Removal_CGAN", file_number + ".png");
        Image_Option.GetComponent<Dropdown>().value = 0;
    }


    public GameObject Player;
    public GameObject Real_Sun, Virtual_Sun;
    public Dropdown sun_option;
    public Camera skycamera;
    public Camera camera;
    public GameObject Image_Option;
    public GameObject Function_Option;
    public GameObject Tile_Object, Build_Object;
    void ScreenShot(string dir_path, string file_number) // 현재 Player 내부 Camera에서 보는 화면의 사진 한장을 저장하는 함수
    {
        RenderTexture rt = new RenderTexture(x_res, y_res, 24);
        skycamera.targetTexture = rt;
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(x_res, y_res, TextureFormat.RGB24, false);
        skycamera.Render(); // 배경을 먼저 렌더링한 후에 camera 렌더링
        camera.Render(); // 순서가 바뀌면 하늘만 보인다
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, x_res, y_res), 0, 0);
        skycamera.targetTexture = null;
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        Destroy(screenShot);
        string filename = null;
        filename = string.Format(dir_path + "/" + file_number + ".png");
        System.IO.File.WriteAllBytes(filename, bytes);
    }
    void getshot_image(string dir_path, string file_num, int value)
    {
        // capture image
        int current_value = 0;
        current_value = Image_Option.GetComponent<Dropdown>().value;
        Image_Option.GetComponent<Dropdown>().value = value;
        ScreenShot(dir_path, file_num);
        Image_Option.GetComponent<Dropdown>().value = current_value;
    }
    void getshot_information(string dir_path, string file_num)
    {
        // get information of capture situation

        FileStream f = null;
        StreamWriter wr = null;
        f = new FileStream(dir_path + "/" + file_num + ".txt", FileMode.Create, FileAccess.Write);
        wr = new StreamWriter(f, System.Text.Encoding.Unicode);
        wr.WriteLine("name : " + file_num);
        wr.WriteLine("Lat: " + PlayerPrefs.GetFloat("LATITUDE") + ", Lon: " + PlayerPrefs.GetFloat("LONGITUDE"));
        wr.WriteLine("Player.Position : " + camera.transform.position + ", Player.Rotation : " + camera.transform.eulerAngles);
        wr.WriteLine("Resolution Width X Height : " + x_res + "x" + y_res); //set resolution
        wr.WriteLine("Screen Width X Height : " + Screen.width + "x" + Screen.height); //real screen
        wr.Close();
        f.Close();
    }
    void getshot_height(string dir_path, string file_number)
    {

        int countX = 0, countY = 0;
        int Stride = 1;
        Ray ray;
        float maxDistance = 1000000;
        RaycastHit hit;
        FileStream f = null;
        StreamWriter wr = null;

        Function_Option.GetComponent<Get_Facade_Image>().Convex_On(false);

        int child_count = Tile_Object.transform.childCount;
        for (int i = 0; i < child_count; i++) /// set layer 9 to tile
            Tile_Object.transform.GetChild(i).gameObject.layer = 9;

        Vector3 earth_center = Tile_Object.GetComponent<Earth>().origin;

        f = new FileStream(dir_path + "/" + file_number + ".txt", FileMode.Create, FileAccess.Write);
        wr = new StreamWriter(f, System.Text.Encoding.Unicode);

        int count = 0;
        int tile_count = 0;
        int build_count = 0;
        int sky_count = 0;
        int empty_count = 0;
        for (countY = Screen.height - 1; countY >= 0; countY -= 1)
        {
            for (countX = 0; countX < Screen.width; countX += 1)
            {
                count += 1;
                ray = Camera.main.ScreenPointToRay(new Vector3(countX, countY, 0));

                if (Physics.Raycast(ray, out hit, maxDistance)) // raycast and distance is 100000
                {
                    if (hit.transform.parent.gameObject == Tile_Object.gameObject) // if it hits a tile , set zero
                    {
                        tile_count += 1;
                        wr.WriteLine("0");
                        //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX+ ")_0");
                        continue;
                    }
                    else if (hit.transform.parent.gameObject == Build_Object.gameObject) // case 2 : if it hits building , calculate height
                    {
                        Ray Height_Ray = new Ray(hit.point, -1 * (earth_center / 1000)); // Center to hit.point ray ** center is on opposit side => -1 , and so far  => /1000
                        RaycastHit tile_hit;
                        if (Physics.Raycast(Height_Ray, out tile_hit, maxDistance, 1 << 9)) // it tile hit. Tile is layer 9        
                        {
                            build_count += 1;
                            float height_distance = Vector3.Distance(hit.point, tile_hit.point);
                            wr.WriteLine(height_distance);
                            //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," +countX+ ")_" + height_distance);
                        }
                        else // set zero if not. it is under the tile
                        {
                            wr.WriteLine("0");
                            empty_count += 1;
                        }
                    }
                }
                else
                {
                    // case 3 : if no hits, it is a sky => max value of int
                    sky_count += 1;
                    wr.WriteLine(int.MaxValue.ToString());
                    //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX+ ")_" + int.MaxValue.ToString()); // 만약에 맞은 것이 없다면 하늘이므로 max값을 줘야함.
                }
            }
        }
        countX = countY = 0;
        wr.Close();
        f.Close();
        Function_Option.GetComponent<Get_Facade_Image>().Convex_On(true); // return to convex


    }
    
    void getshot_distance(string dir_path, string file_number)
    {

        int countX = 0, countY = 0;
        Ray ray;
        float maxDistance = 1000000;
        RaycastHit hit;
        FileStream f = null;
        StreamWriter wr = null;

        Function_Option.GetComponent<Get_Facade_Image>().Convex_On(false);
        Vector3 earth_center = Tile_Object.GetComponent<Earth>().origin; // earth center
        f = new FileStream(dir_path + "/" + file_number + ".txt", FileMode.Create, FileAccess.Write);
        wr = new StreamWriter(f, System.Text.Encoding.Unicode);

        int count = 0;
        int hit_count = 0;
        int sky_count = 0;
        for (countY = Screen.height - 1; countY >= 0; countY -= 1)
        {
            for (countX = 0; countX < Screen.width; countX += 1)
            {
                count += 1;
                ray = Camera.main.ScreenPointToRay(new Vector3(countX, countY, 0));
                if (Physics.Raycast(ray, out hit, maxDistance))
                {
                    // cast 1 : if hit, caclulate distance
                    float Distance = Vector3.Distance(hit.point, earth_center);
                    wr.WriteLine(Distance);
                    hit_count += 1;

                }
                else
                {
                    // case 2 : if no hit => sky
                    wr.WriteLine(int.MaxValue.ToString());
                    sky_count += 1;
                    //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX+ ")_" + int.MaxValue.ToString()); 
                }
            }
        }
        countX = countY = 0;
        wr.Close();
        f.Close();
        Function_Option.GetComponent<Get_Facade_Image>().Convex_On(true);
    }
    void Height_Map_Run(string dir_path, string file)
    {
        ProcessStartInfo cmd = new ProcessStartInfo();
        Process process = new Process();

        // 실행할 파일명 입력
        cmd.FileName = @"cmd";

        // cmd 창 띄우기 -- 
        cmd.CreateNoWindow = true; // 창 띄울지 말지
        cmd.UseShellExecute = false;

        // cmd 데이터 받기
        cmd.RedirectStandardOutput = false;
        // cmd 데이터 보내기
        cmd.RedirectStandardInput = true;
        // cmd 오류내용 받기 
        cmd.RedirectStandardError = true;

        process.EnableRaisingEvents = false;
        process.StartInfo = cmd;

        process.Start();

        // real datapath when executing python script file
        string deep_learning_dir_path = string.Format("../../Deep_Learning_Image");
        string D_Test_Image_dir_path = deep_learning_dir_path + "/Test_Image";
        string D_Test_Image_Height_dir_path = deep_learning_dir_path + "/Test_Image_Height";
        string D_Test_Image_Info_dir_path = deep_learning_dir_path + "/Test_Image_Info";
        string D_Test_Image_GT_Mask_dir_path = deep_learning_dir_path + "/Test_Image_GT_Mask";
        string D_Test_Image_GT_Texture_dir_path = deep_learning_dir_path + "/Test_Image_GT_Texture";
        string D_Detection_Result_CPD_dir_path = deep_learning_dir_path + "/Detection_Result_CPD";
        string D_Detection_Result_Unet_dir_path = deep_learning_dir_path + "/Detection_Result_UNet";
        string D_Removal_Result_CGAN_dir_path = deep_learning_dir_path + "/Removal_Result_GAN";

        string test_folder = " --test_folder " + D_Test_Image_Height_dir_path + "/";
        string save_foler = " --save_folder " + D_Test_Image_Height_dir_path + "/";
        string file_name = " --file_name " + file;
        string screen_width = " --screen_width " + Screen.width;
        string screen_height = " --screen_height " + Screen.height;
        string command = test_folder + save_foler + file_name + screen_height + screen_width;
        process.StandardInput.Write(@"cd Shadow_Create_Data" + Environment.NewLine); // 명령어 수행
        process.StandardInput.Write(@"cd StreamingAssets" + Environment.NewLine);
        process.StandardInput.Write(@"python HeightMap_Grayscale.py" + command + Environment.NewLine);
        process.StandardInput.Close();
        string error_result = process.StandardError.ReadToEnd();
        UnityEngine.Debug.Log(error_result);
        process.WaitForExit();
        process.Close();

    }
    public void Network_Run(string type, string file_name)
    {

        ProcessStartInfo cmd = new ProcessStartInfo();
        Process process = new Process();

        // 실행할 파일명 입력
        cmd.FileName = @"cmd";

        // cmd 창 띄우기 -- 
        cmd.CreateNoWindow = true; // 창 띄울지 말지
        cmd.UseShellExecute = false;

        // cmd 데이터 받기
        cmd.RedirectStandardOutput = false;
        // cmd 데이터 보내기
        cmd.RedirectStandardInput = true;
        // cmd 오류내용 받기 
        cmd.RedirectStandardError = true;

        process.EnableRaisingEvents = false;
        process.StartInfo = cmd;

        process.Start();

        // real datapath when executing python script file
        string deep_learning_dir_path = string.Format("../../Deep_Learning_Image");
        string D_Test_Image_dir_path = deep_learning_dir_path + "/Test_Image";
        string D_Test_Image_Height_dir_path = deep_learning_dir_path + "/Test_Image_Height";
        string D_Test_Image_Info_dir_path = deep_learning_dir_path + "/Test_Image_Info";
        string D_Test_Image_GT_Mask_dir_path = deep_learning_dir_path + "/Test_Image_GT_Mask";
        string D_Test_Image_GT_Texture_dir_path = deep_learning_dir_path + "/Test_Image_GT_Texture";
        string D_Detection_Result_CPD_dir_path = deep_learning_dir_path + "/Detection_Result_CPD";
        string D_Detection_Result_Unet_dir_path = deep_learning_dir_path + "/Detection_Result_UNet";
        string D_Removal_Result_CGAN_dir_path = deep_learning_dir_path + "/Removal_Result_GAN";

        if (type == "Detection_CPD")
        {
            UnityEngine.Debug.Log("Detection CPD");
            string original_path = " --original_path "+D_Test_Image_dir_path+"/";
            string height_path = " --height_path " + D_Test_Image_Height_dir_path + "/";
            string save_path = " --result_save_path " + D_Detection_Result_CPD_dir_path + "/";
            string mask_path = " --mask_path " + D_Test_Image_GT_Mask_dir_path + "/";
            string test_image = " --test_image " + file_name; // ~.png
            string command = original_path + height_path + save_path + mask_path + test_image;
            process.StandardInput.Write(@"cd Shadow_Create_Data" + Environment.NewLine); // 명령어 수행
            process.StandardInput.Write(@"cd StreamingAssets" + Environment.NewLine);
            process.StandardInput.Write(@"cd Final_shadow_detection_2" + Environment.NewLine);
            process.StandardInput.Write(@"mkdir detection_complete" + Environment.NewLine);
            process.StandardInput.Write(@"python test_CPD.py" + command + Environment.NewLine);
        }
        if (type == "Detection_Unet")
        {
            UnityEngine.Debug.Log("Detection UNet");
            string original_path = " --original_path " + D_Test_Image_dir_path + "/";
            string detected_path = " --detected_path " + D_Detection_Result_Unet_dir_path + "/";
            string mask_path = " --mask_path " + D_Test_Image_GT_Mask_dir_path + "/";
            string test_image = " --test_image " + file_name;
            string result_save_path = " --result_save_path" + D_Detection_Result_CPD_dir_path + "/";

            string command = original_path + detected_path + mask_path + test_image + result_save_path;
            process.StandardInput.Write(@"cd Shadow_Create_Data" + Environment.NewLine); // 명령어 수행
            process.StandardInput.Write(@"cd StreamingAssets" + Environment.NewLine);
            process.StandardInput.Write(@"cd Final_shadow_detection_2" + Environment.NewLine); 
            process.StandardInput.Write(@"mkdir detection2_complete" + Environment.NewLine);
            process.StandardInput.Write(@"python MakeSimpleUNetFinetrainResult.py" + command + Environment.NewLine);
        }
        if (type == "Removal_CGAN")
        {
            UnityEngine.Debug.Log("Removal CGAN");
            string test_image = " --test_image " + file_name; // test imag name
            string original_path = " --original_path " + D_Test_Image_dir_path + "/"; // test image dir
            string mask_result_path = " --mask_result_path " + D_Detection_Result_Unet_dir_path + "/"; // input mask of unet output
            string shadow_less_path = " --shadow_less_path " + D_Test_Image_GT_Texture_dir_path + "/"; // ground truth ( texture image)
            string result_save_path = " --result_save_path " + D_Removal_Result_CGAN_dir_path + "/"; // Removal result ( output)

            string command = test_image + original_path + mask_result_path + shadow_less_path + result_save_path;
            process.StandardInput.Write(@"cd Shadow_Create_Data" + Environment.NewLine); // 명령어 수행
            process.StandardInput.Write(@"cd StreamingAssets" + Environment.NewLine);
            process.StandardInput.Write(@"cd Final_shadow_remove" + Environment.NewLine);
            process.StandardInput.Write(@"mkdir Remove_complete" + Environment.NewLine);
            process.StandardInput.Write(@"python test.py" + command  + Environment.NewLine);
        }

        process.StandardInput.Close();
        string error_result = process.StandardError.ReadToEnd();
        UnityEngine.Debug.Log(error_result);
        process.WaitForExit();
        process.Close();
    }

}