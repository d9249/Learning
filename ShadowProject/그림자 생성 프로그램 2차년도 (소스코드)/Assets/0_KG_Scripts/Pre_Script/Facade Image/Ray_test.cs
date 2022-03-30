using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray_test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public GameObject Cube;
    // Update is called once per frame
    void Update()
    {

        
        Ray ray = new Ray(Cube.transform.position, Cube.transform.forward);

        
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000))
        {
            // 물체에서 빔을 쏴서 맞은 곳
            Debug.Log(hit.point);

            // 하기전에 Convex를 꺼야함. 안그러면 삼각형 인식 못함.
            MeshCollider meshCollider = hit.collider as MeshCollider;
            meshCollider.convex = false;
            Mesh mesh = meshCollider.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            // 선택 삼각형의 세 점
            Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
            Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
            Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
            Transform hitTransform = hit.collider.transform;

            p0 = hitTransform.TransformPoint(p0);
            p1 = hitTransform.TransformPoint(p1);
            p2 = hitTransform.TransformPoint(p2);

            Debug.Log("p0" + p0);
            Debug.Log("p1" + p1);
            Debug.Log("p2" + p2);

            Vector3 side1 = p1 - p0;
            Vector3 side2 = p2 - p0;
            Vector3 perp = Vector3.Cross(side1, side2); // 삼각형에 대한 수직 벡터 구함

            Ray tile_ray = new Ray(hit.point, perp);
            //Debug.DrawRay(tile_ray.origin, tile_ray.direction * 50, Color.yellow);

            Ray earth_ray = new Ray(hit.point,GameObject.Find("TileObject").GetComponent<Earth>().origin / 1000);
            //Debug.DrawRay(earth_ray.origin, earth_ray.direction * 50, Color.white);

            Debug.Log("Cube.transform.forward : "+Cube.transform.forward);
            Debug.Log("earth_ray.direction : "+earth_ray.direction);

            // testing
            Vector3 test_cube = new Vector3(Cube.transform.forward.x, Cube.transform.forward.y, Cube.transform.forward.z);
            Vector3 test_earth = new Vector3(earth_ray.direction.x, earth_ray.direction.y, earth_ray.direction.z);

            //Debug.Log("test_cube : " + test_cube);
            //Debug.Log("test_earth : " + test_earth);

            Debug.DrawRay(Cube.transform.position, Cube.transform.up * 50);
            Debug.Log("Cube.transform.up : "+Cube.transform.up);

            // 오른쪽 벡터를 구해야함.
            Vector3 right_vector;
            right_vector = Vector3.Cross(Cube.transform.forward, Cube.transform.up);
            Debug.DrawRay(hit.point, right_vector);
            


            Ray tc_ray = new Ray(hit.point, -1 * test_cube);
            Ray te_ray = new Ray(hit.point, test_earth);
            Debug.DrawRay(tc_ray.origin, tc_ray.direction * 50, Color.green);
            Debug.DrawRay(te_ray.origin, te_ray.direction * 50, Color.blue);

            Vector3 zto0 = new Vector3(-1* Cube.transform.forward.x, -1 *  Cube.transform.forward.y, earth_ray.direction.z);
            Ray tz_ray = new Ray(hit.point, zto0);
            Debug.DrawRay(tz_ray.origin, tz_ray.direction * 50, Color.white);
            
            Debug.Log("angle2 = " + Vector3.Angle(-1 * earth_ray.direction, Cube.transform.forward)); // 이 각도만큼 Z를 빼주면 됨
            Debug.Log("angle3 = " + Vector3.Angle(-1 * tz_ray.direction, Cube.transform.forward)); // 이 각도만큼 Z를 빼주면 됨


            float is_right = Vector3.Angle(right_vector, te_ray.direction);
            bool isright;
            if (is_right < 90.0f)
            {
                isright = true;
                Debug.Log("angle4 = " + is_right + " it is left");
            }
            else
            {
                isright = false;
                Debug.Log("angle4 = " + is_right + " it is right");
            }




        }
    }
    
    
}
