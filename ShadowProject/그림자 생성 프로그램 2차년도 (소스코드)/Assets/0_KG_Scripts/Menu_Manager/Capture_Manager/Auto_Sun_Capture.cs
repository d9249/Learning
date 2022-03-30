using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Auto_Sun_Capture : MonoBehaviour
{
    public GameObject Real_Sun;
    public GameObject Virtual_Sun;
    public GameObject Player;
    public GameObject TouchDisable_Panel;

    Vector3 Real_Sun_Origin_Rotation, Virtual_Sun_Origin_Rotation;
    public void Sun_Shot(string option , Capture_Class capture_options)
    {
        Player.GetComponent<KG_PlayerMove>().isSCActivated = true;
        TouchDisable_Panel.SetActive(true);

        Real_Sun_Origin_Rotation = Real_Sun.transform.eulerAngles;
        Virtual_Sun_Origin_Rotation = Virtual_Sun.transform.eulerAngles;

        if (option.Contains("Virtual"))
            StartCoroutine(Virtual_Shot(capture_options));
        if (option.Contains("Real"))
            StartCoroutine(Real_Shot(capture_options));
    }

    public Dropdown Virtual_Angle;
    public GameObject Capture_Object;
    IEnumerator Virtual_Shot(Capture_Class capture_options)
    {
        float angle = (float)get_virtual_sun_angle(Virtual_Angle.value);
        //Virtual_Sun.transform.rotation = Quaternion.Euler(60, 90, 0);
        for (int i = 0; i< 5;i++)
        {
            Debug.Log(Virtual_Sun.transform.position);
            gameObject.GetComponent<Capture>().common_capture(capture_options, false);
            Virtual_Sun.transform.Rotate(0f, angle, 0f);
            yield return new WaitForSeconds(0.01f);
        }
        Shot_end();
        yield return null;
    }

    public InputField Real_Sun_Time_Interval;
    public Dropdown Real_Sun_Time_Type;
    public Dropdown Real_Sun_Time_Count;

    public Scrollbar year, month, day, hour, minute;
    IEnumerator Real_Shot(Capture_Class capture_options)
    {
        int result = 0;
        if (int.TryParse(Real_Sun_Time_Interval.text,out result) == false) // if it is just string
            yield return null;

        float interval = int.Parse(Real_Sun_Time_Interval.text);
        string type = get_time_type(Real_Sun_Time_Type.value);
        int count = get_time_count(Real_Sun_Time_Count.value);
        gameObject.GetComponent<Capture>().common_capture(capture_options, false); // get image at origin position

        for (int i = 0;i<count;i++)
        {
            bool over = false; // for year over

            switch (type) // count up value of Scrollbar
            {
                case "MINUTE":
                    if (minute.value + (interval / 59.0f) > 1.0f) // if minute is over 60
                    {
                        float remain = ((minute.value + (interval / 59.0f)) - 1.0f ); // get remain. ex) 55 minute + 10 minute => 5minute remain
                        hour.value += 1f / 23f;
                        minute.value = 0.0f + remain;
                    }
                    else
                        minute.value += (interval / 59.0f); // if minute doesnt over 60
                    break;
                case "HOUR":
                    if (hour.value + (interval / 23.0f) > 1.0f)
                    {
                        float remain = ((hour.value + (interval / 23.0f)) - 1.0f);
                        day.value += 1f / 30f;
                        hour.value = 0.0f + remain;
                    }
                    else
                        hour.value += (interval / 23.0f); // if minute doesnt over 60
                    break;
                case "DAY":
                    if (day.value + (interval / 30.0f) > 1.0f) // if minute is over 60
                    {
                        float remain = ((day.value + (interval / 30.0f)) - 1.0f); // get remain. ex) 55 minute + 10 minute => 5minute remain
                        month.value += 1f / 11f;
                        day.value = 0.0f + remain;
                    }
                    else
                        day.value = day.value + (interval / 30.0f); // if minute doesnt over 60
                    break;
                case "MONTH":
                    if (month.value + (interval / 11.0f) > 1.0f) // if minute is over 60
                    {
                        float remain = ((month.value + (interval / 11.0f)) - 1.0f); // get remain. ex) 55 minute + 10 minute => 5minute remain
                        year.value += 1f / 200f;
                        month.value = 0.0f + remain;
                    }
                    else
                        month.value += (interval / 11.0f); // if minute doesnt over 60
                    break;
                case "YEAR":
                    if (year.value + (interval / 200.0f) > 1.0f) // if year overs ==> just stop
                        over = true;
                    else
                        year.value += (interval / 200.0f); // if minute doesnt over 60
                    break;
            }

            if (over == true)
                break;
            gameObject.GetComponent<Capture>().common_capture(capture_options, false);
            yield return new WaitForSeconds(0.01f);
        }
        Shot_end();
        yield return null;
    }

    public GameObject function_option;
    void Shot_end()
    {
        Player.GetComponent<KG_PlayerMove>().isSCActivated = false;
        Real_Sun.transform.eulerAngles = Real_Sun_Origin_Rotation;
        Virtual_Sun.transform.eulerAngles = Virtual_Sun_Origin_Rotation;
        TouchDisable_Panel.SetActive(false);
        // 스크린샷할때 function option의 dropdown value가 0으로 세팅되므로 다시 세팅해줌
        function_option.GetComponent<Dropdown>().value = 5;
    }

    int get_virtual_sun_angle(int value)
    {
        return 10 + (value * 10);
    }

    string get_time_type(int value)
    {
        // real sun time type value
        // returns type string
        string type = null;
        switch(value)
        {
            case 0:
                type = "MINUTE";
                break;
            case 1:
                type = "HOUR";
                break;
            case 2:
                type = "DAY";
                break;
            case 3:
                type = "MONTH";
                break;
            case 4:
                type = "YEAR";
                break;
        }
        return type;
    }

    int get_time_count(int value)
    {
        return (value *2) + 3;
    }
}
