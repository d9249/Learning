using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// 각 건물의 정보를 화면에 표출시킴
public class Get_Object_attributes : MonoBehaviour
{
    bool ison = false; // 기능 활성화 체크
    public GameObject BD_Info_Panel;

    public void toggle_click() 
    {
        /* 토글이 클릭할 때마다 수행되는 함수 
         * 기능 활성화를 키고 끄는 역할
         */ 
        if (ison == false)
        {
            ison = true;
            
            // 다른 기능들의 비활성화
            GameObject.Find("Make_Facade").GetComponent<Toggle>().isOn = false;
            GameObject.Find("Sun_Location_Predict_toggle").GetComponent<Toggle>().isOn = false;
            if(GameObject.Find("KG_Manager").GetComponent<KG_RayShot>().isRayOn == true)
                GameObject.Find("KG_Manager").GetComponent<KG_RayShot>().RayOn();
        }
        else
        {
            ison = false;
        }
    }

    public void cancel_button() 
    {
        // 취소버튼을 누르면, 기능은 활성화를 유지하며, 건물 정보 판넬이 사라짐
        BD_Info_Panel.SetActive(false);
    }

    void Update()
    {
        if (ison) // 기능 활성화 시에만 작동
        {
            // 좌클릭 시, 건물 정보 표출
            if (Input.GetMouseButtonDown(0)) 
                StartCoroutine(get_info());
        }
    }

    IEnumerator get_info() // 건물의 정보를 가져옴
    {
        /* 좌클릭시, 건물의 정보를 가져오는 IEnumerator
         * 웹에서부터 정보를 파싱하여 판넬에 작성
         */ 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
    
        if (Physics.Raycast(ray, out hit, 10000) == true && EventSystem.current.IsPointerOverGameObject() == false && hit.transform.parent.name.Contains("TileObject") == false) // 거리 10000정도에 맞았는 데, 그게 UI 쪽이면 클릭 불가, 타일도 선택 불가
        {
            // Raycast한 건물의 이름에서 위경도를 가져옴
            string[] split_obj_name = hit.transform.name.Split('_'); 
            string obj_lon = split_obj_name[4];
            string obj_lat = split_obj_name[5];
            string get_selected_building_info_URL = "http://map.vworld.kr/v3map_po_buildMetaInfoGIS.do?BLDGPOS=POINT(" + obj_lon + "%20" + obj_lat + ")&SRSNAME=EPSG:4326"; // 건물 정보 파싱할 주소
            WWW www = new WWW(get_selected_building_info_URL);
            yield return www;
            byte[] b = www.bytes;
            if (b == null) // 없으면 끝냄
                // 없다고 정보 알려줘야함!!!
                yield return null;
            else
            {

                BinaryReader demReader = null;
                demReader = new BinaryReader(new MemoryStream(b));
                demReader.BaseStream.Seek(0, SeekOrigin.Begin);
                System.Text.Encoding.UTF8.GetString(demReader.ReadBytes(12)); // 쓸모없는 BOM 제거
                var full_text = System.Text.Encoding.UTF8.GetString(demReader.ReadBytes(b.Length - 12)); // 나머지 스트링을 다 구함
                string body_text = get_string(full_text, "<body>", "</body>");
                Building_INFO bd = new Building_INFO(); // 정보 담을 클래스 생성
                fill_BD_info(bd, body_text); // 클래스에 정보 채우기
                bd.print(); // 디버깅 확인 용
                if (demReader != null) demReader.Close(); // 사용을 다했으면 닫아준다.

                BD_Info_Panel.SetActive(true); // 판넬 활성화
                fill_Panel_text(bd);
            }
        }
        yield return null;            
    }


    // 판넬 속 Text 채워넣을 변수를 모두 채워넣음
    public Text name, detail_name, road, road_english, purpose,structure, upstage,downstage,area,height,floor_area_ratio,coverage_ratio,total_floor_area,land_area,accept_date;
    void fill_Panel_text(Building_INFO bd)
    {
        // 판넬 정보 입력
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
        /* INFO 정보 채워주는 함수
         * 
         */

        //지번
        string tmp;
        tmp = get_string(body, "<div class=\"ko\">", "</div>");
        bd.BD_road = get_string(tmp, "<span>(", ")</span>");

        // 지번 영어
        tmp = get_string(body, "<div class=\"en\">", "</div>");
        bd.BD_road_english= get_string(tmp, "<span>(", ")</span>");

        // 표 테이블 구하기
        string table;
        table = get_string(body, "<table class=\"inn-ly-bd01\">", "</table>");

        //건물 명칭
        tmp = get_string(table, "건물명칭</th>", "</tr>");
        bd.BD_name = get_string(tmp, "<td colspan=\"3\">", "</td>");

        //건물 동명칭
        tmp = get_string(table, "건물동명칭</th>", "</tr>");
        bd.BD_detail_name = get_string(tmp, "<td colspan=\"3\">", "</td>");

        //건물 용도
        tmp = get_string(table, "건물용도", "구조");
        bd.BD_purpose = get_string(tmp, "<td>", "</td>");

        // 구조
        tmp = get_string(table, "구조", "</tr>");
        bd.BD_structure = get_string(tmp, "<td>", "</td>");

        // 지상 층수
        tmp = get_string(table, "지상층수", "지하층수");
        bd.BD_upstage = get_string(tmp, "<td>", "</td>");

        // 지하 층수
        tmp = get_string(table, "지하층수", "</tr>");
        bd.BD_downstage = get_string(tmp, "<td>", "</td>");

        // 건물 면적
        tmp = get_string(table, "건물면적", "건물높이");
        bd.BD_area = (get_string(tmp, "<td>", "</td>")).Trim();

        // 건물 높이
        tmp = get_string(table, "건물높이", "</tr>");
        bd.BD_height = get_string(tmp, "<td>", "</td>").Trim();

        // 용적률
        tmp = get_string(table, "용적률", "건폐율");
        bd.BD_floor_area_ratio = get_string(tmp, "<td>", "</td>").Trim();

        // 건폐율
        tmp = get_string(table, "건폐율", "</tr>");
        bd.BD_coverage_ratio = get_string(tmp, "<td>", "</td>").Trim();

        // 연면적
        tmp = get_string(table, "연면적", "대지면적");
        bd.BD_total_floor_area = get_string(tmp, "<td>", "</td>").Trim();

        // 대지면적
        tmp = get_string(table, "대지면적", "</tr>");
        bd.BD_land_area = get_string(tmp, "<td>", "</td>").Trim();

        // 사용승인일자
        tmp = get_string(table, "사용승인일자", "</tr>");
        bd.BD_accecpt_date = get_string(tmp, "<td colspan=\"3\">", "</td>").Trim();
        
}

    String get_string(string origin, string start, string end) 
    {
        /* 문자열 사이에 있는 문자열을 구하는 함수
         * 파싱할 때의 편리성을 위한 함수
         */
        int start_index = origin.IndexOf(start); // start 문자열이 시작되는 index를 구함
        string start_adjusted_origin = origin.Substring(start_index); // start_index부터의 문자열
        int end_index = start_adjusted_origin.IndexOf(end); // 잘라진 문자열에서 end가 시작되는 index 구함
        string output = start_adjusted_origin.Substring(0, end_index); // end가 시작되는 곳까지 자름
        output = output.Substring(start.Length); // 처음에 검색한 결과는 제거
        return output;
    }
    
    
}
