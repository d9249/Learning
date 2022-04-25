using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class Building_Information : MonoBehaviour
{
    // 없는 건물 표시 => null 값으로 넣는 것도 괜챃ㄴ

    public bool isOn = false;
    public GameObject BD_Info_Panel;

    public void cancel_button()
    {
        BD_Info_Panel.SetActive(false);
    }

    void Update()
    {
        if(isOn)
        {
            if (Input.GetMouseButton(0))
                StartCoroutine(get_info());
        }
    }


    IEnumerator get_info() // 건물의 정보를 가져옴
    {
        // get information from web
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000) == true && EventSystem.current.IsPointerOverGameObject() == false && hit.transform.parent.name.Contains("TileObject")== false) // distance 100000, no tile and no ui
        {
            // get lat,lon from racasted building
            string[] split_obj_name = hit.transform.name.Split('_');
            string obj_lon = split_obj_name[4];
            string obj_lat = split_obj_name[5];
            string get_selected_building_info_URL = "http://map.vworld.kr/v3map_po_buildMetaInfoGIS.do?BLDGPOS=POINT(" + obj_lon + "%20" + obj_lat + ")&SRSNAME=EPSG:4326"; // address to get information from web
            WWW www = new WWW(get_selected_building_info_URL);
            yield return www;
            byte[] b = www.bytes;
            if (b == null) // if it has no information

                // 없다고 정보 알려줘야함!!!
                yield return null;
            else
            {
                BinaryReader demReader = null;
                demReader = new BinaryReader(new MemoryStream(b));
                demReader.BaseStream.Seek(0, SeekOrigin.Begin);
                System.Text.Encoding.UTF8.GetString(demReader.ReadBytes(12)); // remove unnessasary BCM
                var full_text = System.Text.Encoding.UTF8.GetString(demReader.ReadBytes(b.Length - 12)); // get remained string
                string body_text = get_string(full_text, "<body>", "</body>");
                Building_INFO bd = new Building_INFO(); // create a class
                fill_BD_info(bd, body_text); // fill information of class
                bd.print(); // just for debugging
                if (demReader != null) demReader.Close(); // close reader
                BD_Info_Panel.SetActive(true); // activate panel
                fill_Panel_text(bd);
            }
        }
        yield return null;
    }


    
    public Text name, detail_name, road, road_english, purpose, structure, upstage, downstage, area, height, floor_area_ratio, coverage_ratio, total_floor_area, land_area, accept_date;
    void fill_Panel_text(Building_INFO bd)
    {
        // put information into panel
        name.text = "건물 명칭 : " + bd.BD_name;
        detail_name.text = "건물동 명칭 : " + bd.BD_detail_name;
        road.text = "지번 : " + bd.BD_road;
        road_english.text = "지번(영어) : " + bd.BD_road_english;
        purpose.text = "건물 용도 : " + bd.BD_purpose;
        structure.text = "구조 : " + bd.BD_structure;
        upstage.text = "지상 층수 : " + bd.BD_upstage;
        downstage.text = "지하 층수 : " + bd.BD_downstage;
        area.text = "건물 면적 : " + bd.BD_area;
        height.text = "건물 높이 : " + bd.BD_height;
        floor_area_ratio.text = "용적율 : " + bd.BD_floor_area_ratio;
        coverage_ratio.text = "건폐율 : " + bd.BD_coverage_ratio;
        total_floor_area.text = "연면적 : " + bd.BD_total_floor_area;
        land_area.text = "대지면적 : " + bd.BD_land_area;
        accept_date.text = "사용승인일자 : " + bd.BD_accecpt_date;
    }

    void fill_BD_info(Building_INFO bd, string body)
    {
        // put information into Building_INFO class

        //adress korea
        string tmp;
        tmp = get_string(body, "<div class=\"ko\">", "</div>");
        bd.BD_road = get_string(tmp, "<span>(", ")</span>");

        // adress english
        tmp = get_string(body, "<div class=\"en\">", "</div>");
        bd.BD_road_english = get_string(tmp, "<span>(", ")</span>");

        // table
        string table;
        table = get_string(body, "<table class=\"inn-ly-bd01\">", "</table>");

        //building name
        tmp = get_string(table, "건물명칭</th>", "</tr>");
        bd.BD_name = get_string(tmp, "<td colspan=\"3\">", "</td>");

        //detailed name
        tmp = get_string(table, "건물동명칭</th>", "</tr>");
        bd.BD_detail_name = get_string(tmp, "<td colspan=\"3\">", "</td>");

        //purpose
        tmp = get_string(table, "건물용도", "구조");
        bd.BD_purpose = get_string(tmp, "<td>", "</td>");

        // structure
        tmp = get_string(table, "구조", "</tr>");
        bd.BD_structure = get_string(tmp, "<td>", "</td>");

        // upstage
        tmp = get_string(table, "지상층수", "지하층수");
        bd.BD_upstage = get_string(tmp, "<td>", "</td>");

        // downstage
        tmp = get_string(table, "지하층수", "</tr>");
        bd.BD_downstage = get_string(tmp, "<td>", "</td>");

        // area
        tmp = get_string(table, "건물면적", "건물높이");
        bd.BD_area = (get_string(tmp, "<td>", "</td>")).Trim();

        // height
        tmp = get_string(table, "건물높이", "</tr>");
        bd.BD_height = get_string(tmp, "<td>", "</td>").Trim();

        // floor area ratio
        tmp = get_string(table, "용적률", "건폐율");
        bd.BD_floor_area_ratio = get_string(tmp, "<td>", "</td>").Trim();

        // coveratge ratio
        tmp = get_string(table, "건폐율", "</tr>");
        bd.BD_coverage_ratio = get_string(tmp, "<td>", "</td>").Trim();

        // total floor area
        tmp = get_string(table, "연면적", "대지면적");
        bd.BD_total_floor_area = get_string(tmp, "<td>", "</td>").Trim();

        // land area
        tmp = get_string(table, "대지면적", "</tr>");
        bd.BD_land_area = get_string(tmp, "<td>", "</td>").Trim();

        // accepted date
        tmp = get_string(table, "사용승인일자", "</tr>");
        bd.BD_accecpt_date = get_string(tmp, "<td colspan=\"3\">", "</td>").Trim();

    }

    string get_string(string origin, string start, string end)
    {
        // parsing the string between one string and another string
        int start_index = origin.IndexOf(start);
        string start_adjusted_origin = origin.Substring(start_index);
        int end_index = start_adjusted_origin.IndexOf(end); 
        string output = start_adjusted_origin.Substring(0, end_index);
        output = output.Substring(start.Length);
        return output;
    }
}
