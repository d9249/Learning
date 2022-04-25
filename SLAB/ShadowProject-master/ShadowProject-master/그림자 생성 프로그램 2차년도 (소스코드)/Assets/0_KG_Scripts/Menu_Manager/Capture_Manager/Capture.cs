using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Capture : MonoBehaviour
{
    // general image directory
    string general_dir_path;

    string Origin_dir_path;
    string Mask_dir_path;
    string Texture_dir_path;
    string Height_dir_path;
    string Distance_dir_path;
    string Pixel_Location_dir_path;
    string Info_dir_path;

    // facade image directory
    string facade_dir_path;

    string FHeight_dir_path;
    string FDistance_dir_path;
    string Fimage_dir_path;
    string FInfo_dir_path;
    string FPixel_Location_dir_path;



    void Awake()
    {
        // set resolution => this resolution is associated with pixel_location and height map
        Screen.SetResolution(1600, 1600, true);
    }

    void Start()
    {
        // general image directory
        general_dir_path = string.Format(Application.dataPath + "/../General_Image");

        Origin_dir_path = general_dir_path + "/SC_Origin";
        Texture_dir_path = general_dir_path + "/SC_Texture";
        Mask_dir_path = general_dir_path + "/SC_Mask";
        Height_dir_path = general_dir_path + "/SC_Height";
        Distance_dir_path = general_dir_path + "/SC_Distance";
        Pixel_Location_dir_path = general_dir_path + "/SC_Pixel_Location";
        Info_dir_path = general_dir_path + "/SC_Info";

        // facade image directory
        facade_dir_path = string.Format(Application.dataPath + "/../Facade_Image");

        Fimage_dir_path = facade_dir_path + "/SC_Facade_Image";
        FHeight_dir_path = facade_dir_path + "/SC_Facade_Height";
        FDistance_dir_path = facade_dir_path + "/SC_Facade_Distance";
        FInfo_dir_path = facade_dir_path + "/SC_Facade_Info";
        FPixel_Location_dir_path = facade_dir_path + "/SC_Facade_Pixel_Location";
        
    }

    static string file_number;
    int x_res=400, y_res=400;
    public void common_capture(Capture_Class capture_option, bool is_facade)
    {
        // check options and capture if it is on

        // value 3 is facade , 4 is auto screenshot

        // set resolution of image ( only for just images...  doesnt imclude height,height2,pixel_location)
        x_res = capture_option.x_res;
        y_res = capture_option.y_res;
        
        check_dir();

        if (is_facade) // Facade case
        {
            file_number = check_file_number(Fimage_dir_path);
            Function_Option.GetComponent<Get_Facade_Image>().Save_Origin();
            ScreenShot(Fimage_dir_path, file_number, TMP_Camera.GetComponent<Camera>());
            getshot_information(FInfo_dir_path, file_number, TMP_Camera); // information
            if (capture_option.height)
                getshot_facade_height(FHeight_dir_path, file_number); // facade height
            if (capture_option.height2)
                getshot_facade_distance(FDistance_dir_path, file_number); // facade distance
            if (capture_option.pixel_location)
                getshot_pixel_location(FPixel_Location_dir_path, file_number, TMP_Camera.GetComponent<Camera>()); // facade pixel location

            Function_Option.GetComponent<Get_Facade_Image>().Replace_All(); // replace
        }
        else // common case
        {
            file_number = check_file_number(Origin_dir_path);
            // information and original image must be saved
            getshot_image(Origin_dir_path, file_number, 1); // original
            getshot_information(Info_dir_path, file_number, Player); // information

            if (capture_option.mask_image)
                getshot_image(Mask_dir_path, file_number, 2); // mask
            if (capture_option.texture_image)
                getshot_image(Texture_dir_path, file_number, 3); // texture
            if (capture_option.height)
                getshot_height(Height_dir_path, file_number); // height
            if (capture_option.height)
                getshot_distance(Distance_dir_path, file_number); // distance
            if (capture_option.pixel_location)
                getshot_pixel_location(Pixel_Location_dir_path, file_number, camera); // pixel location
        }
    }

    void check_dir()
    {

        // Check if directory is exists. if not, make coreesponding directory

        // general directory check

        System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(general_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Origin_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Texture_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Mask_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Height_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Distance_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Pixel_Location_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Info_dir_path);
        if (!d.Exists) d.Create();

        // facade directory check
        d = new System.IO.DirectoryInfo(facade_dir_path);
        if (!d.Exists) d.Create();

        d = new System.IO.DirectoryInfo(Fimage_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(FHeight_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(FDistance_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(FInfo_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(FPixel_Location_dir_path);
        if (!d.Exists) d.Create();

    }
    string check_file_number(string dir_path)
    {
        // Check file number of recent ( standard : original image )
        int count = 0;
        System.IO.FileInfo fi;
        while (true)
        {
            fi = new System.IO.FileInfo(dir_path + "/" + count.ToString() + ".png");
            Debug.Log(fi);
            if (!fi.Exists)
                break;
            else
            {
                count += 1;
            }
        }
        return count.ToString();
    }

    public GameObject Player;
    public GameObject Real_Sun, Virtual_Sun;
    public Dropdown sun_option;
    void getshot_information(string dir_path,string file_num,GameObject camera)
    {
        // get information of capture situation

        FileStream f = null;
        StreamWriter wr = null;
        f = new FileStream(dir_path + "/" + file_num + ".txt", FileMode.Create, FileAccess.Write);
        wr = new StreamWriter(f, System.Text.Encoding.Unicode);
        wr.WriteLine("name : " + file_num);
        wr.WriteLine("Lat: " + PlayerPrefs.GetFloat("LATITUDE") + ", Lon: " + PlayerPrefs.GetFloat("LONGITUDE"));
        wr.WriteLine("Player.Position : " + camera.transform.position + ", Player.Rotation : " + camera.transform.eulerAngles);
        wr.WriteLine("Resolution Width X Height : " + x_res+ "x" + y_res); //set resolution
        wr.WriteLine("Screen Width X Height : " + Screen.width + "x" + Screen.height); //real screen
        if(sun_option.value == 1)
            wr.WriteLine("Real Sun Rotation : " + Real_Sun.transform.eulerAngles); //real sun
        if(sun_option.value == 2)
            wr.WriteLine("Virtual Sun Rotation : " + Virtual_Sun.transform.eulerAngles); //real screen
        wr.Close();
        f.Close();
    }

    public GameObject Image_Option;
    void getshot_image(string dir_path, string file_num , int value)
    {
        // capture image
        int current_value = 0;
        current_value = Image_Option.GetComponent<Dropdown>().value;
        Image_Option.GetComponent<Dropdown>().value = value;
        ScreenShot(dir_path, file_num,camera);
        Image_Option.GetComponent<Dropdown>().value = current_value;
    }

    public GameObject Function_Option;
    public GameObject Tile_Object, Build_Object;
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
                            Debug.Log(hit.transform.name);
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

        Debug.Log("count" +count);
        Debug.Log("tile_count" + tile_count);
        Debug.Log("build_count" + build_count);
        Debug.Log("sky_count" + sky_count);
        Debug.Log("empty_count" + empty_count);

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
    void getshot_pixel_location(string dir_path, string file_num,Camera camera)
    {
        int countX = 0, countY = 0; 
        Ray ray;
        float maxDistance = 1000000;
        RaycastHit hit; 
        FileStream f = null;
        StreamWriter wr = null;

        Function_Option.GetComponent<Get_Facade_Image>().Convex_On(false);
        f = new FileStream(dir_path + "/" + file_num + ".txt", FileMode.Create, FileAccess.Write);
        wr = new StreamWriter(f, System.Text.Encoding.Unicode);
        for (countY = Screen.height - 1; countY >= 0; countY -= 1)        {
            for (countX = 0; countX < Screen.width; countX += 1)
            {
                ray = camera.ScreenPointToRay(new Vector3(countX, countY, 0));
                if (Physics.Raycast(ray, out hit, maxDistance))
                    wr.WriteLine("(" + (Screen.height - countY - 1) + "," + countX + ") : " + hit.point);
                else // if no hit
                    wr.WriteLine("(" + (Screen.height - countY - 1) + "," + countX + ") : sky");
                //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + "), hitPoint (sky)"); 
            }
        }
        countX = countY = 0;
        wr.Close(); 
        f.Close();
        Function_Option.GetComponent<Get_Facade_Image>().Convex_On(true);
        
    }

    public GameObject TMP_Camera;
    void getshot_facade(string dir_path,string  file_number)
    {
        RenderTexture rt = new RenderTexture(x_res, y_res, 24);
        skycamera.targetTexture = rt;
        TMP_Camera.GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(x_res, y_res, TextureFormat.RGB24, false);
        skycamera.Render(); // 배경을 먼저 렌더링한 후에 camera 렌더링
        TMP_Camera.GetComponent<Camera>().Render(); // 순서가 바뀌면 하늘만 보인다
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, x_res, y_res), 0, 0);
        skycamera.targetTexture = null;
        TMP_Camera.GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        Destroy(screenShot);
        string filename = null;
        filename = string.Format(dir_path + "/" + file_number + ".png");
        System.IO.File.WriteAllBytes(filename, bytes);
    }

    void getshot_facade_height(string dir_path,string file_num)
    {
        FileStream f = null; 
        StreamWriter wr = null;
        int countX = 0, countY = 0;
        Ray ray; 
        float maxDistance = 1000000; 
        RaycastHit hit;

        Function_Option.GetComponent<Get_Facade_Image>().Convex_On(false);
        f = new FileStream(dir_path + "/"+ file_num+".txt", FileMode.Create, FileAccess.Write);
        wr = new StreamWriter(f, System.Text.Encoding.Unicode); 

        int child_count = Tile_Object.transform.childCount;
        for (int i = 0; i < child_count; i++)
            Tile_Object.transform.GetChild(i).gameObject.layer = 9;
        Vector3 earth_center = Tile_Object.GetComponent<Earth>().origin; // earth center

        Function_Option.GetComponent<Get_Facade_Image>().Save_Origin();
        
        MeshCollider[] Tile_Collider;
        Tile_Collider = Tile_Object.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider Tile in Tile_Collider)
        {
            Tile.enabled = true;
            Tile.convex = false;
        }

        string Origin_name = Function_Option.GetComponent<Get_Facade_Image>().Origin_hit;

        for (countY = Screen.height - 1; countY >= 0; countY -= 1)
        {
            for (countX = 0; countX < Screen.width; countX += 1) 
            {
                ray = TMP_Camera.GetComponent<Camera>().ScreenPointToRay(new Vector3(countX, countY, 0)); 

                if (Physics.Raycast(ray, out hit, maxDistance)) 
                {

                    // case 1 : if it's name is not origin_name , think as sky
                    if (hit.transform.name != Origin_name)
                    {
                        wr.WriteLine(int.MaxValue.ToString());
                        //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + ")_" + int.MaxValue.ToString());
                        continue;
                    }
                    // case 2 : 맞은 곳이 Facade 일 경우, 높이 값을 기록
                    else if (hit.transform.name == Origin_name)
                    {
                        // ray to get height
                        Ray Height_Ray = new Ray(hit.point, -1 * (earth_center / 1000));
                        RaycastHit tile_hit;
                        if (Physics.Raycast(Height_Ray, out tile_hit, maxDistance, 1 << 9)) // if hits tile
                        {
                            float height_distance = Vector3.Distance(hit.point, tile_hit.point);
                            wr.WriteLine(height_distance);
                            //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX  + ")_" + height_distance); // 픽셀별로 맞은 위치를 넣는다.
                        }
                        else // if hits nothing
                        {
                            wr.WriteLine("0");
                            //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + ")_" + "0"); // 픽셀별로 맞은 위치를 넣는다.
                        }
                    }

                }
                else
                    // case 3 : nothing hit
                    wr.WriteLine(int.MaxValue.ToString());
                //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + ")_" + int.MaxValue.ToString()); // 만약에 맞은 것이 없다면 하늘이므로 max값을 줘야함.
            }
        }
        countX = countY = 0;
        wr.Close();
        f.Close();

        Function_Option.GetComponent<Get_Facade_Image>().Convex_On(true);
    }

    void getshot_facade_distance(string dir_path, string file_num)
    {
        FileStream f = null;
        StreamWriter wr = null;
        int countX = 0, countY = 0;
        Ray ray;
        float maxDistance = 1000000;
        RaycastHit hit;
        Function_Option.GetComponent<Get_Facade_Image>().Convex_On(false);
        f = new FileStream(dir_path + "/" + file_num + ".txt", FileMode.Create, FileAccess.Write);
        wr = new StreamWriter(f, System.Text.Encoding.Unicode);

        int child_count = Tile_Object.transform.childCount;
        for (int i = 0; i < child_count; i++)
            Tile_Object.transform.GetChild(i).gameObject.layer = 9;
        Vector3 earth_center = Tile_Object.GetComponent<Earth>().origin; // earth center

        Function_Option.GetComponent<Get_Facade_Image>().Save_Origin();

        MeshCollider[] Tile_Collider;
        Tile_Collider = Tile_Object.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider Tile in Tile_Collider)
        {
            Tile.enabled = true;
            Tile.convex = false;
        }

        string Origin_name = Function_Option.GetComponent<Get_Facade_Image>().Origin_hit;

        for (countY = Screen.height - 1; countY >= 0; countY -= 1)
        {
            for (countX = 0; countX < Screen.width; countX += 1)
            {
                ray = TMP_Camera.GetComponent<Camera>().ScreenPointToRay(new Vector3(countX, countY, 0));

                if (Physics.Raycast(ray, out hit, maxDistance))
                {

                    // case 1 : if it's name is not origin_name , think as sky
                    if (hit.transform.name != Origin_name)
                    {
                        wr.WriteLine(int.MaxValue.ToString());
                        //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + ")_" + int.MaxValue.ToString());
                        continue;
                    }
                    // case 2 : if hits facade => put distance of earth center and hit point
                    else if (hit.transform.name == Origin_name)
                    {
                        float distance = Vector3.Distance(hit.point, earth_center);
                        wr.WriteLine(distance);
                    }
                    else // if hits nothing
                    {
                        wr.WriteLine("0");
                        //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + ")_" + "0"); // 픽셀별로 맞은 위치를 넣는다.
                    }
                }
                else
                    // case 3 : nothing hit
                    wr.WriteLine(int.MaxValue.ToString());
                    //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + ")_" + int.MaxValue.ToString()); // 만약에 맞은 것이 없다면 하늘이므로 max값을 줘야함.
            }
        }
        countX = countY = 0;
        wr.Close();
        f.Close();

        Function_Option.GetComponent<Get_Facade_Image>().Convex_On(true);
    }


    public Camera skycamera;
    public Camera camera;
    void ScreenShot(string dir_path, string file_number, Camera camera) // 현재 Player 내부 Camera에서 보는 화면의 사진 한장을 저장하는 함수
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

}
