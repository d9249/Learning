using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private float moveSpeed = 6.0f;
    private float gravity = 9.80665f;
    
    // Use this for initialization
    void Start () {
		
	}
    
    // Update is called once per frame
    void Update() {
        
    }

    Quaternion GetRotFromVectors(Vector2 posStart, Vector2 posend)
    {
        return Quaternion.Euler(0.0f, 0.0f, -Mathf.Atan2(posend.x - posStart.x, posend.y - posStart.y) * Mathf.Rad2Deg);
    }

    //2018하계연수생남대현
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Terrian")
            Earth.gravity = 0f;
        Debug.Log("trigger stay");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Terrian")
            Earth.gravity = -9.8f;

        Debug.Log("trigger exit");
    }
}
