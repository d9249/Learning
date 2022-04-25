using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facade : MonoBehaviour
{
    public GameObject Cube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Cube.transform.position = new Vector3(0f, 0f, 0f);
            Cube.transform.position = Camera.main.transform.position;
            Cube.transform.parent = Camera.main.transform;
            Cube.transform.localPosition = new Vector3(0, 0, 5f);
            Cube.transform.LookAt(Camera.main.transform);


        }

    }
}
