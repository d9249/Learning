using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Three_Shot : MonoBehaviour
{
    void Awake()
    {
        // 초기 해상도 설정
        Screen.SetResolution(1600, 1600, true);
    }

    // 각 저장 디렉토리 경로명
    string Origin_dir_path ;
    string Texture_dir_path;
    string Mask_dir_path ;
    //string Depth_dir_path ;
    string Height_dir_path;
    string Distance_dir_path;

    static string file_number;
    int resWidth, resHeight;
    void Start()
    {
        // 해상도 설정
        Origin_dir_path = string.Format(Application.dataPath + "/../SC_Origin");
        Texture_dir_path = string.Format(Application.dataPath + "/../SC_Texture");
        Mask_dir_path = string.Format(Application.dataPath + "/../SC_Mask");
        //Depth_dir_path = string.Format(Application.dataPath + "/../SC_Depth");
        Height_dir_path = string.Format(Application.dataPath + "/../SC_Height");
        Distance_dir_path = string.Format(Application.dataPath + "/../SC_Distance");
    }

    public void getshot()
    {
        /*
         * 1. 디렉토리 체크
         * 2. 파일명에 쓰일 file number 얻는다
         * 3. 각각의 스크린샷을 찍음
         */ 
        check_dir();
        file_number = check_file_number(Origin_dir_path);
        getshot_origin(Origin_dir_path,file_number); // 일반
        getshot_mask(Mask_dir_path, file_number); // 마스크
        getshot_texture(Texture_dir_path, file_number); // 텍스쳐
        //getshot_depth(Depth_dir_path, file_number);
        getshot_height(Height_dir_path, file_number); // 높이값
        getshot_distance(Distance_dir_path, file_number); // 절대 높이값 ( 지구 원점으로부터의 거리 값 )
    }

    void check_dir()
    {
        /*
         * Check if directory is exists. if not, make coreesponding directory
         */
        System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(Origin_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Texture_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Mask_dir_path);
        if (!d.Exists) d.Create();
        //d = new System.IO.DirectoryInfo(Depth_dir_path);
        //if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Height_dir_path);
        if (!d.Exists) d.Create();
        d = new System.IO.DirectoryInfo(Distance_dir_path);
        if (!d.Exists) d.Create();

    }
    string check_file_number(string dir_path)
    {
        // 현존하는 파일들의 개수를 세어, 다음에 찍을 번호를 반환 ( 기준은 SC_Origin )
        int count = 0;
        System.IO.FileInfo fi;
        while (true)
        {
            fi = new System.IO.FileInfo(dir_path +"/"+ count.ToString()+".png");
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

    void getshot_origin(string dir_path, string file_num)
    {
        // 일반 스크린샷
        ScreenShot(dir_path, file_num);
    }


    void getshot_texture(string dir_path, string file_num)
    {
        // 텍스쳐 스크린 샷
        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Texture_Change("Unlit/Texture");
        ScreenShot(dir_path, file_num);
        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Texture_Change("Standard");

    }

    void getshot_mask(string dir_path, string file_num)
    {
        // 마스크 스크린 샷
        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
        ScreenShot(dir_path, file_num);
        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();

    }

    public Depth depth_script;
    void getshot_depth(string dir_path, string file_num)
    {
        // 카메라부터의 거리에 따른 거리 스크린 샷
        depth_script.enabled = true;
        ScreenShot(dir_path, file_num);
        depth_script.enabled = false;
    }

    void getshot_height(string dir_path, string file_number)
    {

        int countX = 0, countY = 0; // 2중 포문을 돌리기 위함
        int Stride = 1; // 모든 픽셀을 1개씩 찍을 건지, 10칸 단위로 쪼갤건지 결정하는 Stride. 보통 실험결과 10~11정도가 적당했다. 하지만 모든 픽셀에 대해 수행할 경우 1로 해야함. 현재는 연산량을 줄이기 위해 11로 설정
        Ray ray; // 화면에서 빔을 쏘기 위한 변수
        float maxDistance = 1000000; // 빔을 쏘는 최대 거리를 설정. 적을수록 가까운 범위에서 있는 객체만 맞는다. 클 수록 먼 거리까지도 탐지 가능.
        RaycastHit hit; // 빔을 쏴서 맞은 물체를 담기 위한 변수
        FileStream f = null; // 파일 스트림 변수
        StreamWriter wr = null; // Write 변수

        
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);

        //우선 convex를 꺼버림.
        GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Convex_off(true);

        // 우선 Tile 객체의 자식 Tile을 모두 Tile layer에 넣어준다. 현재 9가 tile layer
        int child_count = GameObject.Find("TileObject").transform.childCount;
        for(int i = 0;i<child_count;i++)
        {
            GameObject.Find("TileObject").transform.GetChild(i).gameObject.layer = 9;
        }
        Vector3 earth_center = GameObject.Find("TileObject").GetComponent<Earth>().origin; // 지구 중심 참조

        f = new FileStream(dir_path + "/" + file_number+".txt", FileMode.Create, FileAccess.Write); // 새로 만들고, 쓰기로 접근하여 원래 파일명에서 INFO를 붙여 TXT 파일 포멧으로 설정
        wr = new StreamWriter(f, System.Text.Encoding.Unicode); // 인코딩 방식 설정을 한 Writer
        //wr.WriteLine("Screen(" + Screen.height + "," + Screen.width + ")"); // TIle일 경우, 0 으로  설정해야함..
        
        int count = 0;
        int tile_count = 0;
        int build_count = 0;
        int sky_count = 0;
        int empty_count = 0;
        for (countY = Screen.height - 1; countY >= 0; countY -= Stride)
                {
                    for (countX = 0; countX <Screen.width; countX += Stride) // x가 0일때 , y는 0~Screen.height 만큼 출력하는 방식으로 이중 포문 구성
                    {
                        count += 1;
                        ray = Camera.main.ScreenPointToRay(new Vector3(countX, countY, 0)); // 픽셀 좌표에서 빔을 쏜다

                        if (Physics.Raycast(ray, out hit, maxDistance)) // 10000거리 만큼 빔을 쏴서 맞춘 것을 hit에 넣는다.
                        {
                            
                            // 경우 1 : 맞은 곳이 타일일 경우, 0으로 처리
                            if (hit.transform.parent.name == "TileObject")
                            {

                                tile_count += 1;
                                wr.WriteLine("0");
                                //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX+ ")_0"); // TIle일 경우, 0 으로  설정해야함..
                                continue;
                            }
                            // 경우 2 : 맞은 곳이 일반 물체일 경우, 높이 값을 기록
                            else if (hit.transform.parent.name == "BuildObject")
                            {

                                // 높이를 얻기 위한 ray 쏘기.
                                Ray Height_Ray = new Ray(hit.point, -1 * (earth_center/ 1000)); // 지구 중심이 거꾸로 되어있으므로 -1을 통하여 뒤집어진 곳으로 빔을 쏴서 맞은 곳이 Tile layer일 경우.
                                RaycastHit tile_hit;
                                if (Physics.Raycast(Height_Ray, out tile_hit, maxDistance, 1 << 9)) // 맞은 곳이 타일일 경우 ( 타일은 layer 9)
                                {
                                    build_count += 1;
                                    // 우선 맞은 곳과 원점간의 거리를 구함.
                                    float height_distance = Vector3.Distance(hit.point, tile_hit.point);
                                    //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," +countX+ ")_" + height_distance); // 픽셀별로 맞은 위치를 넣는다.
                                    wr.WriteLine(height_distance);
                                }

                                else // 건물이 있는 데도 없는 경우는 보통 바닥에 박혀있는 것이다. 우선 0으로 처리
                                {
                                    wr.WriteLine("0");
                                    Debug.Log("hit transform name : " + hit.transform.name);
                                    Debug.Log("hit transform parant name : " + hit.transform.parent.name);
                                    empty_count += 1;
                                }
                            }
                        }
                    else
                    {
                    sky_count += 1;
                    wr.WriteLine(int.MaxValue.ToString());
                    // 경우 3 : 맞은 곳이 없는 경우 하늘로 처리
                    //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX+ ")_" + int.MaxValue.ToString()); // 만약에 맞은 것이 없다면 하늘이므로 max값을 줘야함.
                    }

            }
            }
        
        // 확인을 위한 변수
        Debug.Log("count : " + count);
        Debug.Log("tile count : " + tile_count);
        Debug.Log("build count : " + build_count);
        Debug.Log("sky count : " + sky_count);
        Debug.Log("empty count : " + empty_count);
        countX = countY = 0; // 다시 0으로 전역변수를 초기화 해준다. int i, j로 할 경우 매번 돌릴때마다 생성하고 지우는 연산을 안하는 것이 더 이득이라 판단하여 이렇게 했다.
        wr.Close(); // Writer를 닫아준다.
        f.Close(); // 파일 스트림을 닫아준다.
        GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Convex_off(false); // convex 원상복귀


    }

    void getshot_distance(string dir_path, string file_number)
    {

        int countX = 0, countY = 0; // 2중 포문을 돌리기 위함
        int Stride = 1; // 모든 픽셀을 1개씩 찍을 건지, 10칸 단위로 쪼갤건지 결정하는 Stride. 보통 실험결과 10~11정도가 적당했다. 하지만 모든 픽셀에 대해 수행할 경우 1로 해야함. 현재는 연산량을 줄이기 위해 11로 설정
        Ray ray; // 화면에서 빔을 쏘기 위한 변수
        float maxDistance = 1000000; // 빔을 쏘는 최대 거리를 설정. 적을수록 가까운 범위에서 있는 객체만 맞는다. 클 수록 먼 거리까지도 탐지 가능.
        RaycastHit hit; // 빔을 쏴서 맞은 물체를 담기 위한 변수
        FileStream f = null; // 파일 스트림 변수
        StreamWriter wr = null; // Write 변수


        Debug.Log(Screen.width);
        Debug.Log(Screen.height);

        //우선 convex를 꺼버림.
        GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Convex_off(true);
        Vector3 earth_center = GameObject.Find("TileObject").GetComponent<Earth>().origin; // 지구 중심 참조

        f = new FileStream(dir_path + "/" + file_number + ".txt", FileMode.Create, FileAccess.Write); // 새로 만들고, 쓰기로 접근하여 원래 파일명에서 INFO를 붙여 TXT 파일 포멧으로 설정
        wr = new StreamWriter(f, System.Text.Encoding.Unicode); // 인코딩 방식 설정을 한 Writer
                                                                //wr.WriteLine("Screen(" + Screen.height + "," + Screen.width + ")"); // TIle일 경우, 0 으로  설정해야함..

        int count = 0;
        int hit_count = 0;
        int sky_count = 0;
        for (countY = Screen.height - 1; countY >= 0; countY -= Stride)
        {
            for (countX = 0; countX < Screen.width; countX += Stride) // x가 0일때 , y는 0~Screen.height 만큼 출력하는 방식으로 이중 포문 구성
            {
                count += 1;
                ray = Camera.main.ScreenPointToRay(new Vector3(countX, countY, 0)); // 픽셀 좌표에서 빔을 쏜다

                if (Physics.Raycast(ray, out hit, maxDistance)) // 10000거리 만큼 빔을 쏴서 맞춘 것을 hit에 넣는다.
                {
                    // 경우 1 : 맞은 곳이 있다면 지구 원점으로부터의 거리를 측정하여 집어넣음
                    float Distance = Vector3.Distance(hit.point, earth_center);
                    wr.WriteLine(Distance);
                    hit_count += 1;

                }
                else
                {
                    // 경우 2 : 맞은 곳이 없는 경우 하늘로 처리
                    wr.WriteLine(int.MaxValue.ToString()); 
                    sky_count += 1;
                    //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX+ ")_" + int.MaxValue.ToString()); // 만약에 맞은 것이 없다면 하늘이므로 max값을 줘야함.
                }

            }
        }

        // 확인을 위한 변수
        Debug.Log("count : " + count);
        Debug.Log("hit_count : " + hit_count);
        Debug.Log("sky count : " + sky_count);
        countX = countY = 0; // 다시 0으로 전역변수를 초기화 해준다. int i, j로 할 경우 매번 돌릴때마다 생성하고 지우는 연산을 안하는 것이 더 이득이라 판단하여 이렇게 했다.
        wr.Close(); // Writer를 닫아준다.
        f.Close(); // 파일 스트림을 닫아준다.
        GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Convex_off(false); // convex 원상복귀
    }

    public Camera skycamera;
    public Camera camera;
    void ScreenShot(string dir_path, string file_number) // 현재 Player 내부 Camera에서 보는 화면의 사진 한장을 저장하는 함수
    {
        resWidth = Screen.width;
        resHeight = Screen.width;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        skycamera.targetTexture = rt;
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight,TextureFormat.RGB24, false);
        skycamera.Render(); // 배경을 먼저 렌더링한 후에 camera 렌더링
        camera.Render(); // 순서가 바뀌면 하늘만 보인다
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        skycamera.targetTexture = null;
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        Destroy(screenShot); 
        string filename = null;
        filename = string.Format(dir_path+"/" + file_number+".png");
        System.IO.File.WriteAllBytes(filename, bytes);
        GameObject.Find("KG_Manager").GetComponent<KG_PixelOption>().PixelOption(filename);
        GameObject.Find("KG_Manager").GetComponent<KG_RayShot>().ProgressBar.value += 1.0f; // ProgressBar의 value를 올려 1장을 찍었다는 것을 표시
        
    }
}
