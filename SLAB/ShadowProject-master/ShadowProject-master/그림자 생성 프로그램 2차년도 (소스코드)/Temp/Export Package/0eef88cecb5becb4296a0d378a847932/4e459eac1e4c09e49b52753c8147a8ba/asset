using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LatLong : MonoBehaviour {

    InputField inputLat;
    InputField inputLon;

    float tempLat, tempLon;
	// Use this for initialization
	void Start () {
        inputLat = GameObject.Find("LatGroup").GetComponentInChildren<InputField>();
        inputLon = GameObject.Find("LonGroup").GetComponentInChildren<InputField>();

        tempLat = PlayerPrefs.GetFloat("LATITUDE");
        tempLon = PlayerPrefs.GetFloat("LONGITUDE");
        inputLat.text = tempLat.ToString();
        inputLon.text = tempLon.ToString();
    }
    public void GoViewer()
    {
        if (tempLat.ToString() != inputLat.text)
        {
            tempLat = System.Single.Parse(inputLat.text);
            PlayerPrefs.SetFloat("LATITUDE", tempLat);
        }
        if (tempLon.ToString() != inputLon.text)
        {
            tempLon = System.Single.Parse(inputLon.text);
            PlayerPrefs.SetFloat("LONGITUDE", tempLon);
        }

        SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
    }
}
