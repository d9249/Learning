using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Get_Face : MonoBehaviour
{
    public Toggle Facade_Toggle; // Make_Facade Toggle 참조 변수
    public GameObject Player; // Player 참조 변수
    float hit_lat;
    float hit_lon;
    // Update is called once per frame

    Vector3 Origin_Player_Pos;
    Vector3 Origin_Player_Rot;
    public string Origin_hit;
    Vector3 Origin_pos;
    Vector3 Origin_rot;
    public Camera skyboxcamera; // 배경 끄기 위해 참조

    // 건물과 타일 끄기 위해 참조
    MeshRenderer[] Tile_Object;
    MeshRenderer[] Build_Object;
    MeshCollider[] Tile_Collider;
    MeshCollider[] Build_Collider;
    public float perp_speed = 10.0f; // facade 면에서 얼만큼 떨어질 건지에 대한 거리. ** 이것을 이용하여 물체의 정면과 카메라의 거리를 조절할 예정 ( 버튼식)
    
    public void Replace_All()
    {
        Renderer_off(false);
        Collider_off(false);
        GameObject.Find(Origin_hit).GetComponent<Transform>().position = Origin_pos;
        skyboxcamera.clearFlags = CameraClearFlags.Skybox;
        GameObject.Find(Origin_hit).GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard");
        Collider_off(false);
        Renderer_off(false);
        GameObject.Find("Player").GetComponent<KG_PlayerMove>().isSCActivated = false;
        GameObject.Find("Facade_Manager").GetComponent<Turn_on_off_functions>().turn_on_func();
        Player.transform.position = Origin_Player_Pos;
        Player.transform.eulerAngles = Origin_Player_Rot;
        Main_Camera.SetActive(true);
    }

    public void Save_Origin()
    {
        Renderer_off(true);
        Collider_off(true);
        GameObject.Find(Origin_hit).GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find(Origin_hit).GetComponent<MeshCollider>().enabled = true;
    }

    public void Renderer_off(bool is_true)
    {
        Tile_Object = GameObject.Find("TileObject").GetComponentsInChildren<MeshRenderer>(); // Object를 찾아 그 자식 Objcet들의 MeshRenderer를 담음
        Build_Object = GameObject.Find("BuildObject").GetComponentsInChildren<MeshRenderer>();

        if (is_true)
        {
            foreach (MeshRenderer Tile in Tile_Object)
                Tile.enabled = false;
            foreach (MeshRenderer Build in Build_Object)
                Build.enabled = false;
        }
        else
        {
            foreach (MeshRenderer Tile in Tile_Object)
                Tile.enabled = true;
            foreach (MeshRenderer Build in Build_Object)
                Build.enabled = true;
        }

    }

    public void Collider_off(bool is_true)
    {
        Tile_Collider = GameObject.Find("TileObject").GetComponentsInChildren<MeshCollider>(); // Object를 찾아 그 자식 Objcet들의 MeshRenderer를 담음
        Build_Collider = GameObject.Find("BuildObject").GetComponentsInChildren<MeshCollider>();

        if (is_true)
        {
            foreach (MeshCollider Tile in Tile_Collider)
                Tile.enabled = false;
            foreach (MeshCollider Build in Build_Collider)
                Build.enabled = false;
        }
        else
        {
            foreach (MeshCollider Tile in Tile_Collider)
                Tile.enabled = true;
            foreach (MeshCollider Build in Build_Collider)
                Build.enabled = true;
        }
        
    }

    public void Convex_off(bool is_true)
    {
        // 선택 물체의 convex를 끄기 위해서 사용
        Build_Collider = GameObject.Find("BuildObject").GetComponentsInChildren<MeshCollider>();
        if (is_true)
            foreach (MeshCollider Build in Build_Collider)
                Build.convex = false;
        else
            foreach (MeshCollider Build in Build_Collider)
                Build.convex = true;

        // 물체를 선택한 후, Z축을 회전하기 위해 Tile Object의 삼각형을 얻기 위해 사용
        Tile_Collider = GameObject.Find("TileObject").GetComponentsInChildren<MeshCollider>();
        if (is_true)
            foreach (MeshCollider Tile in Tile_Collider)
                Tile.convex = false;
        else
            foreach (MeshCollider Tile in Tile_Collider)
                Tile.convex = true;

    }

    // Make_Facade 클릭 시, 다른 UI를 끄기 위해서 선정
    public Camera TMP_Camera;
    public GameObject Parallel_Cube;
    public InputField ZInput_Field;
    public GameObject Main_Camera;
    void Update()
    {
        if(Facade_Toggle.isOn) // 만일 사용자가 토글을 클릭하였다면
        {
            // 다른 것들을 클릭하지 못하도록 꺼야함
            GameObject.Find("BD_Info").GetComponent<Toggle>().isOn = false;
            GameObject.Find("Sun_Location_Predict_toggle").GetComponent<Toggle>().isOn = false;
            if (GameObject.Find("KG_Manager").GetComponent<KG_RayShot>().isRayOn == true)
                GameObject.Find("KG_Manager").GetComponent<KG_RayShot>().RayOn();



            Convex_off(true); // convex 기능을 비활성화하여 세밀한 메쉬 표현

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Input.GetMouseButtonDown(0)) // 마우스 왼 클릭 시
            {
                if (Physics.Raycast(ray, out hit, 10000000) == true && EventSystem.current.IsPointerOverGameObject() == false &&  hit.transform.parent.name.Contains("TileObject") == false) // 거리 10000정도에 맞았는 데, 그게 UI 쪽이면 클릭 못함
                {
                    
                    hit.transform.GetComponent<MeshCollider>().convex = false;
                    
                    MeshCollider meshCollider = hit.collider as MeshCollider;
                    if (meshCollider == null || meshCollider.sharedMesh == null)
                        return;


                    // 원상복귀 할 때, 카메라도 원상복귀 시키기 위한 코드
                    Origin_Player_Pos = Player.transform.position;
                    Origin_Player_Rot= Player.transform.eulerAngles;

                    // 삼각형 좌표를 얻기 위한 코드
                    Mesh mesh = meshCollider.sharedMesh;
                    Vector3[] vertices = mesh.vertices;
                    int[] triangles = mesh.triangles;
                    Debug.Log(hit.triangleIndex);
                    
                    Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
                    Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
                    Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
                    Transform hitTransform = hit.collider.transform;

                    
                    // 선택 삼각형의 세 점
                    p0 = hitTransform.TransformPoint(p0);
                    p1 = hitTransform.TransformPoint(p1);
                    p2 = hitTransform.TransformPoint(p2);

                    Debug.Log("p0" + p0);
                    Debug.Log("p1" + p1);
                    Debug.Log("p2" + p2);

                    Vector3 side1 = p1 - p0;
                    Vector3 side2 = p2 - p0;
                    Vector3 perp = Vector3.Cross(side1, side2); // 삼각형에 대한 수직 벡터 구함
                    float perp_length = perp.magnitude;
                    perp /= perp_length; // 노멀라이징
                    
                    Debug.Log("perp " + perp);

                    Vector3 mesh_middle = new Vector3((p0.x + p1.x + p2.x) / 3, (p0.y + p1.y + p2.y) / 3, (p0.z + p1.z + p2.z) / 3); // 삼각형의 중심점을 구함
                    Debug.Log("mesh_middle " + mesh_middle);

                    Debug.Log("Player.transform.eoulera : " + Player.transform.eulerAngles);
                    // 삼각형 중앙에서 수직 벡터 방향으로 카메라를 위치
                    Player.transform.position = mesh_middle; // 삼각형 중심으로 이동해야함 원래는
                    Player.transform.position = new Vector3((float)Player.transform.position.x + (float)perp.x * perp_speed, (float)Player.transform.position.y + (float)perp.y * perp_speed, (float)Player.transform.position.z + (float)perp.z * perp_speed);
                    Player.transform.LookAt(mesh_middle); // 삼각형의 중앙을 쳐다본다.

                    Debug.Log("Player.transform.eoulera : " + Player.transform.eulerAngles);

                    //삼각형의 밑변과 평행하게 쳐다봐야함.
                    Ray Parallel_Ray = new Ray(Parallel_Cube.transform.position, Parallel_Cube.transform.forward);
                    RaycastHit tile_hit;
                    float camera_tile_ray_angle = 0;
                    Ray earth_ray;
                    if (Physics.Raycast(Parallel_Ray, out tile_hit,10000))
                    {
                        earth_ray = new Ray(hit.point, GameObject.Find("TileObject").GetComponent<Earth>().origin / 1000);
                        camera_tile_ray_angle = Vector3.Angle(-1 * earth_ray.direction, Parallel_Cube.transform.forward);

                        // 각도만큼 Z축 회전시켜줌
                        // 대신 Player의 방향에 따라 달라지는 특성상 forward와 up을 통해 직각좌표를 구하고, 그 좌표와 earth_ray의 각도를 참고삼아, 카메라의 왼쪽에 위치했는 지 오른쪽에 위치했는 지 파악하여 각도 회전한다.
                        Vector3 left_vector = Vector3.Cross(Parallel_Cube.transform.forward, Parallel_Cube.transform.up);
                        float left_angle = Vector3.Angle(left_vector, earth_ray.direction);

                        if (left_angle < 90.0f) // 왼쪽에 있다느 의미는 더해준다.
                            Player.transform.Rotate(0f, 0f, camera_tile_ray_angle);
                        else if (left_angle > 90.0f) // 오른쪽에 있으면 빼준다.
                            Player.transform.Rotate(0f, 0f, -1 * camera_tile_ray_angle);
                        else// 직각이면 완벽하게 평행이라는 의미.
                            Debug.Log("perp");

                    }


                    // 좀더 뒤로 빼줌.
                    Player.transform.position = new Vector3((float)Player.transform.position.x + (float)perp.x * perp_speed, (float)Player.transform.position.y + (float)perp.y * perp_speed, (float)Player.transform.position.z + (float)perp.z * perp_speed);
                    Player.transform.position = new Vector3((float)Player.transform.position.x + (float)perp.x * perp_speed, (float)Player.transform.position.y + (float)perp.y * perp_speed, (float)Player.transform.position.z + (float)perp.z * perp_speed);
                    Player.transform.position = new Vector3((float)Player.transform.position.x + (float)perp.x * perp_speed, (float)Player.transform.position.y + (float)perp.y * perp_speed, (float)Player.transform.position.z + (float)perp.z * perp_speed);



                    //테스팅용
                    TMP_Camera.transform.position = Player.transform.position;
                    TMP_Camera.transform.eulerAngles = Player.transform.eulerAngles;
                    TMP_Camera.nearClipPlane = 0.01f;
                    TMP_Camera.farClipPlane = 10000f;
                    TMP_Camera.fieldOfView = Camera.main.fieldOfView;
                    Main_Camera.SetActive(false);


                    var hit_obj = hit.transform; // hit한 object의 transform
                    // 나중에 다시 되돌아올 Vector3 저장
                    Origin_hit = hit_obj.name;
                    Origin_pos = hit_obj.position;
                    Origin_rot = hit_obj.eulerAngles;

                    skyboxcamera.clearFlags = CameraClearFlags.SolidColor;
                    //skyboxcamera.backgroundColor = new Color(255f, 255f, 255f); //특정 배경색으로 바꾸고 싶다면
                    hit_obj.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
                    Save_Origin(); // 다른 거 다 끄고, 선택 물체만 살리기
                    GameObject.Find("Facade_Manager").GetComponent<Far_Close_Button>().perp = perp;
                    GameObject.Find("Facade_Manager").GetComponent<Far_Close_Button>().selected_object = hit_obj.name;
                    
                    // 필요없는 UI 제거
                    GameObject.Find("Facade_Manager").GetComponent<Turn_on_off_functions>().turn_off_func();
                    // 한번만 클릭하기를 허용하기 위해 이제 기능을 끔
                    Facade_Toggle.isOn = false;
                    // 마우스로 인한 회전과 키보드 조작 끄기
                    GameObject.Find("Player").GetComponent<KG_PlayerMove>().isSCActivated = true;
                    
                }
            }
            }
        }


    
    

}
