using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

// PixelToggle에 설정에 따라 INFO 텍스트 파일 생성을 하는 클래스. 다만... 이것을 설정시 조금 느린 것은 감안해야한다.
public class KG_PixelOption : MonoBehaviour {
    public Toggle PixelToggle; // INFO 정보의 선택여부를 붙는 PixelToggle을 참조
    int countX=0, countY = 0; // 2중 포문을 돌리기 위함
    int Stride = 1; // 모든 픽셀을 1개씩 찍을 건지, 10칸 단위로 쪼갤건지 결정하는 Stride. 보통 실험결과 10~11정도가 적당했다. 하지만 모든 픽셀에 대해 수행할 경우 1로 해야함. 현재는 연산량을 줄이기 위해 11로 설정
    Ray ray; // 화면에서 빔을 쏘기 위한 변수
    float maxDistance = 1000000; // 빔을 쏘는 최대 거리를 설정. 적을수록 가까운 범위에서 있는 객체만 맞는다. 클 수록 먼 거리까지도 탐지 가능.
    RaycastHit hit; // 빔을 쏴서 맞은 물체를 담기 위한 변수
    FileStream f=null; // 파일 스트림 변수
    StreamWriter wr=null; // Write 변수

    public void PixelOption(string path) // 파일 저장 경로를 받아와 그 파일에 INFO.TXT 파일을 저장하는 함수
    {
        var lat = PlayerPrefs.GetFloat("LATITUDE");
        var lon = PlayerPrefs.GetFloat("LONGITUDE");

        GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Convex_off(true);
        if (PixelToggle.isOn) // PixelToggle이 클릭되어 있을 때만 허용한다
        {
            f = new FileStream(path + "_INFO.txt", FileMode.Create, FileAccess.Write); // 새로 만들고, 쓰기로 접근하여 원래 파일명에서 INFO를 붙여 TXT 파일 포멧으로 설정
            wr = new StreamWriter(f, System.Text.Encoding.Unicode); // 인코딩 방식 설정을 한 Writer
            wr.WriteLine("name : " + path + "_INFO.text"); // 파일명 추가 ( 일반적으로 path 변수안에 Player 객체의 모든 위치 정보가 있다. 필요시 여기에 추가)
            wr.WriteLine("Lat: " + lat+", Lon:" +lon);
            wr.WriteLine("#Camera.Position : " + Camera.main.transform.position + ", Camera.Rotation : " + Camera.main.transform.eulerAngles); // 카메라 위치와 각도 추가 (Player의 위치, 회전 정보와 같다)
            wr.WriteLine("#" + Screen.width + "x" + Screen.height); //화면의 가로 세로
            wr.WriteLine("# 픽셀 좌표별 Raycast hit좌표"); // 픽셀별 빔을 쏴서 맞은 곳의 좌표를 출력
            for (countY = Screen.height - 1; countY >= 0; countY -= Stride)
            {
                for (countX = 0; countX < Screen.width; countX += Stride) // x가 0일때 , y는 0~Screen.height 만큼 출력하는 방식으로 이중 포문 구성
                {
                    ray = Camera.main.ScreenPointToRay(new Vector3(countX, countY, 0)); // 픽셀 좌표에서 빔을 쏜다
                    if (Physics.Raycast(ray, out hit, maxDistance)) // 10000거리 만큼 빔을 쏴서 맞춘 것을 hit에 넣는다. 
                        wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + "), hitPoint" + hit.point); // 픽셀별로 맞은 위치를 넣는다.
                    else
                        wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + "), hitPoint (sky)"); // 만약에 맞은 것이 없다면 빈 공간을 한 것이므로, 현실에서는 하늘이니 sky로 출력하게 했다.
                }
            }
            countX = countY = 0; // 다시 0으로 전역변수를 초기화 해준다. int i, j로 할 경우 매번 돌릴때마다 생성하고 지우는 연산을 안하는 것이 더 이득이라 판단하여 이렇게 했다.
            wr.Close(); // Writer를 닫아준다.
            f.Close(); // 파일 스트림을 닫아준다.
            GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Convex_off(false);
        }
    }


    public GameObject TMP_Camera;
    public void Pixel_height_Option(string path,int screen_width, int screen_height) // 파일 저장 경로를 받아와 그 파일에 INFO.TXT 파일을 저장하는 함수
    {
        var lat = PlayerPrefs.GetFloat("LATITUDE");
        var lon = PlayerPrefs.GetFloat("LONGITUDE");
        
        if (PixelToggle.isOn) // PixelToggle이 클릭되어 있을 때만 허용한다
        {
            f = new FileStream(path + ".txt", FileMode.Create, FileAccess.Write); // 새로 만들고, 쓰기로 접근하여 원래 파일명에서 INFO를 붙여 TXT 파일 포멧으로 설정
            wr = new StreamWriter(f, System.Text.Encoding.Unicode); // 인코딩 방식 설정을 한 Writer
            /*
            wr.WriteLine("name : " + path + "_INFO.text"); // 파일명 추가 ( 일반적으로 path 변수안에 Player 객체의 모든 위치 정보가 있다. 필요시 여기에 추가)
            wr.WriteLine("Lat: " + lat + ", Lon:" + lon);
            wr.WriteLine("#Camera.Position : " + TMP_Camera.transform.position + ", Camera.Rotation : " + TMP_Camera.transform.eulerAngles); // 카메라 위치와 각도 추가 (Player의 위치, 회전 정보와 같다)
            wr.WriteLine("#" + Screen.width + "x" + Screen.height); //화면의 가로 세로
            wr.WriteLine("# 픽셀 좌표별 Raycast hit (y ,x)좌표"); // 픽셀별 빔을 쏴서 맞은 곳의 좌표를 출력
             */
            
            
            // 우선 Tile 객체의 자식 Tile을 모두 Tile layer에 넣어준다. 현재 9가 tile layer
            int child_count = GameObject.Find("TileObject").transform.childCount;
            for (int i = 0; i < child_count; i++)
            {
                GameObject.Find("TileObject").transform.GetChild(i).gameObject.layer = 9;
            }
            Vector3 earth_center = GameObject.Find("TileObject").GetComponent<Earth>().origin; // 지구 중심 참조
            
            
            GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Save_Origin();

            // 우선 빔을 쏘기 전에 모든 Tile만 Collider를 킨다
            MeshCollider[] Tile_Collider;
            Tile_Collider = GameObject.Find("TileObject").GetComponentsInChildren<MeshCollider>(); // Object를 찾아 그 자식 Objcet들의 MeshRenderer를 담음
            foreach (MeshCollider Tile in Tile_Collider)
            {
                Tile.enabled = true;
                Tile.convex = false;
            }

            // Facade 이름도 알아놓는다.
            string Origin_name = GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Origin_hit;

            for (countY = Screen.height - 1; countY >= 0; countY -= Stride)
            {
                for (countX = 0; countX < Screen.width; countX += Stride) // x가 0일때 , y는 0~Screen.height 만큼 출력하는 방식으로 이중 포문 구성
                {
                    ray = TMP_Camera.GetComponent<Camera>().ScreenPointToRay(new Vector3(countX, countY, 0)); // 픽셀 좌표에서 빔을 쏜다

                    if (Physics.Raycast(ray, out hit, maxDistance)) // 10000거리 만큼 빔을 쏴서 맞춘 것을 hit에 넣는다.
                    {

                        // 경우 1 : 맞은 곳이 있어도 Facade 이름이 아니면 모두 하늘 처리 ( 타일일 경우 해당 )
                        if (hit.transform.name != Origin_name)
                        {
                            wr.WriteLine(int.MaxValue.ToString());
                            //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + ")_" + int.MaxValue.ToString());
                            continue;
                        }

                        // 경우 2 : 맞은 곳이 Facade 일 경우, 높이 값을 기록
                        else if (hit.transform.name == Origin_name)
                        {

                            // 높이를 얻기 위한 ray 쏘기.
                            Ray Height_Ray = new Ray(hit.point, -1 * (earth_center / 1000)); // 지구 중심이 거꾸로 되어있으므로 -1을 통하여 뒤집어진 곳으로 빔을 쏴서 맞은 곳이 Tile layer일 경우.
                            RaycastHit tile_hit;
                            if (Physics.Raycast(Height_Ray, out tile_hit, maxDistance, 1 << 9)) // 맞은 곳이 타일일 경우 ( 타일은 layer 9)
                            {
                                // 우선 맞은 곳과 원점간의 거리를 구함.
                                float height_distance = Vector3.Distance(hit.point, tile_hit.point);
                                wr.WriteLine(height_distance);
                                //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX  + ")_" + height_distance); // 픽셀별로 맞은 위치를 넣는다.
                            }
                            else
                            {
                                wr.WriteLine("0");
                                //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + ")_" + "0"); // 픽셀별로 맞은 위치를 넣는다.

                            }
                        }

                    }
                    else
                        // 경우 3 : 맞은 곳이 없는 경우 하늘로 처리
                        wr.WriteLine(int.MaxValue.ToString());
                        //wr.WriteLine("픽셀좌표(" + (Screen.height - countY - 1) + "," + countX + ")_" + int.MaxValue.ToString()); // 만약에 맞은 것이 없다면 하늘이므로 max값을 줘야함.
                }
            }
            countX = countY = 0; // 다시 0으로 전역변수를 초기화 해준다. int i, j로 할 경우 매번 돌릴때마다 생성하고 지우는 연산을 안하는 것이 더 이득이라 판단하여 이렇게 했다.
            wr.Close(); // Writer를 닫아준다.
            f.Close(); // 파일 스트림을 닫아준다.

            // 혹시 모르니 모든 Collider를 다시 꺼줌.
            GameObject.Find("Facade_Manager").GetComponent<Get_Face>().Convex_off(false);
            // 우선 빔을 쏘기 전에 모든 Tile만 Collider를 킨다
            foreach (MeshCollider Tile in Tile_Collider)
            {
                Tile.enabled = true;
                Tile.convex = true;
            }

        }
    }
}
