using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Get_Facade_Image : MonoBehaviour
{
    // 새로 생기는 물체들도 렌더러 없애고 콜라이더도 없애야함.
    // 연결성 모두 끊어버리기
    // 픽셀 옵션 추가해야함.
    // 파사드 스크린샷은 스크린샷에 넣어줘얗마


    public bool isOn = false;
    public Camera TMP_Camera;
    public GameObject Parallel_Cube;
    float ray_distance = 10000000;
    public GameObject Player;
    public GameObject main_camera;
    public Camera skyboxcamera;
    Vector3 origin_player_pos,origin_player_rot;
    public string Origin_hit;
    float perp_speed = 10.0f;
    public Facade_Buttons facade_button_script;
    public GameObject capture_button,function_option,save_option;
    void Update()
    {
        if (isOn)
        {
            Convex_On(false); // rid off convexes of objects
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0)) // left click
            {
                if (Physics.Raycast(ray, out hit, ray_distance) == true && EventSystem.current.IsPointerOverGameObject() == false && hit.transform.parent.name.Contains("TileObject") == false)
                {

                    // when we return
                    origin_player_pos = Player.transform.position;
                    origin_player_rot = Player.transform.eulerAngles;

                    MeshCollider meshCollider = hit.collider as MeshCollider;
                    if (meshCollider == null || meshCollider.sharedMesh == null)
                        return;

                    // it is for the triangle we clicked
                    // and it will be parallel to player and the clicked triangle
                    Mesh mesh = meshCollider.sharedMesh;
                    Vector3[] vertices = mesh.vertices;
                    int[] triangles = mesh.triangles;
                    Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
                    Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
                    Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
                    Transform hitTransform = hit.collider.transform;                    
                    // three point of an triangle
                    p0 = hitTransform.TransformPoint(p0);
                    p1 = hitTransform.TransformPoint(p1);
                    p2 = hitTransform.TransformPoint(p2);
                    Vector3 side1 = p1 - p0;
                    Vector3 side2 = p2 - p0;
                    Vector3 perp = Vector3.Cross(side1, side2); // Perpendicular vector 
                    float perp_length = perp.magnitude;
                    perp /= perp_length; // normalizing
                    Vector3 mesh_middle = new Vector3((p0.x + p1.x + p2.x) / 3, (p0.y + p1.y + p2.y) / 3, (p0.z + p1.z + p2.z) / 3); // get middle of triangle
                    Player.transform.position = mesh_middle; // go player to the trianlge middle
                    Player.transform.position = new Vector3((float)Player.transform.position.x + (float)perp.x * perp_speed, (float)Player.transform.position.y + (float)perp.y * perp_speed, (float)Player.transform.position.z + (float)perp.z * perp_speed);
                    Player.transform.LookAt(mesh_middle); // look at the middle

                    // this code is for Parallel for z axis
                    Ray Parallel_Ray = new Ray(Parallel_Cube.transform.position, Parallel_Cube.transform.forward);
                    RaycastHit tile_hit;
                    float camera_tile_ray_angle = 0;
                    Ray earth_ray;
                    if (Physics.Raycast(Parallel_Ray, out tile_hit, 10000))
                    {
                        earth_ray = new Ray(hit.point, Tile_Object.GetComponent<Earth>().origin / 1000); // get the center of earth
                        // this is for the parellel angle
                        camera_tile_ray_angle = Vector3.Angle(-1 * earth_ray.direction, Parallel_Cube.transform.forward);
                        
                        // this is for where to rotate the parellel angle. left or right
                        Vector3 left_vector = Vector3.Cross(Parallel_Cube.transform.forward, Parallel_Cube.transform.up);
                        float left_angle = Vector3.Angle(left_vector, earth_ray.direction);

                        if (left_angle < 90.0f) // if it is under 90 => it is left
                            Player.transform.Rotate(0f, 0f, camera_tile_ray_angle);
                        else if (left_angle > 90.0f)
                            Player.transform.Rotate(0f, 0f, -1 * camera_tile_ray_angle);
                        else// perfect parellel
                            Debug.Log("perfect perp");

                    }


                    // get backword 3 times perp speed
                    Player.transform.position = new Vector3((float)Player.transform.position.x + (float)perp.x * perp_speed, (float)Player.transform.position.y + (float)perp.y * perp_speed, (float)Player.transform.position.z + (float)perp.z * perp_speed );
                    Player.transform.position = new Vector3((float)Player.transform.position.x + (float)perp.x * perp_speed, (float)Player.transform.position.y + (float)perp.y * perp_speed, (float)Player.transform.position.z + (float)perp.z * perp_speed);
                    Player.transform.position = new Vector3((float)Player.transform.position.x + (float)perp.x * perp_speed, (float)Player.transform.position.y + (float)perp.y * perp_speed, (float)Player.transform.position.z + (float)perp.z * perp_speed);

                    //put Temporary camera at player position
                    TMP_Camera.transform.position = Player.transform.position;
                    TMP_Camera.transform.eulerAngles = Player.transform.eulerAngles;
                    TMP_Camera.nearClipPlane = 0.01f;
                    TMP_Camera.farClipPlane = 10000f;
                    TMP_Camera.fieldOfView = Camera.main.fieldOfView;
                    main_camera.SetActive(false); // main camera off

                    // we will use this name for facade image
                    var hit_obj = hit.transform;
                    Origin_hit = hit_obj.name;

                    skyboxcamera.clearFlags = CameraClearFlags.SolidColor;
                    skyboxcamera.clearFlags = CameraClearFlags.Skybox;
                    //skyboxcamera.backgroundColor = new Color(255f, 255f, 255f); //if you want some certain color to change background
                    hit_obj.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
                    Save_Origin(); // recover collider and renderer of hit object

                    // give perp vector and hit.name to button script
                    facade_button_script.perp = perp;

                    // 필요없는 UI 제거
                    facade_button_script.Facade_UI(true);

                    function_option.GetComponent<Dropdown>().interactable = false;
                    save_option.GetComponent<Button>().interactable = true;
                    capture_button.GetComponent<Button>().interactable = true; // allow capture button
                    isOn = false; // to click only one time
                    Player.GetComponent<KG_PlayerMove>().isSCActivated = true; // off the function of moving and rotating by mouse and keyboard

                }
            }

        }
    }

    public GameObject Tile_Object, Build_Object;
    MeshCollider[] Tile_Collider,Build_Collider;
    MeshRenderer[] Tile_Renderer,Build_Renderer;
    public void Convex_On(bool isOn)
    {
        // all object's Convex can be on and off

        Tile_Collider = Tile_Object.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider Tile in Tile_Collider)
            Tile.convex = isOn;
        Build_Collider = Build_Object.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider Build in Build_Collider)
            Build.convex = isOn;
        
    }


    public void Save_Origin()
    {
        Renderer_On(false);
        Collider_On(false);
        GameObject.Find(Origin_hit).GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find(Origin_hit).GetComponent<MeshCollider>().enabled = true;
    }

    public void Renderer_On(bool is_true)
    {
        // all object's renderer off => nothing can see

        Tile_Renderer = Tile_Object.GetComponentsInChildren<MeshRenderer>(); // Object를 찾아 그 자식 Objcet들의 MeshRenderer를 담음
        Build_Renderer = Build_Object.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer Tile in Tile_Renderer)
            Tile.enabled = is_true;
        foreach (MeshRenderer Build in Build_Renderer)
            Build.enabled = is_true;
    }

    public void Collider_On(bool is_true)
    {
        // all object's collider_off => nothing can touch
        Tile_Collider = Tile_Object.GetComponentsInChildren<MeshCollider>(); // Object를 찾아 그 자식 Objcet들의 MeshRenderer를 담음
        Build_Collider = Build_Object.GetComponentsInChildren<MeshCollider>();

        foreach (MeshCollider Tile in Tile_Collider)
            Tile.enabled = false;
        foreach (MeshCollider Build in Build_Collider)
            Build.enabled = false;
    }


    public GameObject image_option, sun_option;
    public void Replace_All()
    {
        Renderer_On(true);
        Collider_On(true);
        skyboxcamera.clearFlags = CameraClearFlags.Skybox;
        GameObject.Find(Origin_hit).GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard");
        Player.GetComponent<KG_PlayerMove>().isSCActivated = false;
        facade_button_script.Facade_UI(false);
        Player.transform.position = origin_player_pos;
        Player.transform.eulerAngles = origin_player_rot;
        main_camera.SetActive(true);
        isOn = true;
        function_option.GetComponent<Dropdown>().interactable = true;
        save_option.GetComponent<Button>().interactable = false;
        capture_button.GetComponent<Button>().interactable = false;
    }



}
