using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray_test2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public GameObject Cube;
    void Update()
    {
        Ray ray = new Ray(Cube.transform.position, -1 * Cube.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100000000, Color.white);
        Ray earth_ray = new Ray(GameObject.Find("TileObject").GetComponent<Earth>().origin / 1000, - 1 * (GameObject.Find("TileObject").GetComponent<Earth>().origin / 1000 -  Cube.transform.position));
        Debug.DrawRay(earth_ray.origin, earth_ray.direction * 10000000000000000000, Color.yellow);
        Debug.Log(Cube.transform.forward);
        Debug.Log(earth_ray.direction);

        Debug.Log("angle = " + Vector3.Angle(-1 * earth_ray.direction, -1 * Cube.transform.forward)); // 이 각도만큼 Z를 빼주면 됨.
    }
}
