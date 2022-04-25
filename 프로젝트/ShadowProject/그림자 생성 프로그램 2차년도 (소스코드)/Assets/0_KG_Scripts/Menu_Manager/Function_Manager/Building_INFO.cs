using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_INFO
{
    // 지번
    public string BD_road { get; set; }
    // 지번 영어
    public string BD_road_english { get; set; }
    // 건물 명칭
    public string BD_name { get; set; }
    // 건물동명칭
    public string BD_detail_name { get; set; }
    // 건물용도
    public string BD_purpose { get; set; }
    // 구조
    public string BD_structure { get; set; }
    // 지상 층수
    public string BD_upstage { get; set; }
    // 지하 층수
    public string BD_downstage { get; set; }
    // 건물 면적
    public string BD_area { get; set; }
    // 건물 높이
    public string BD_height { get; set; }
    // 용적률
    public string BD_floor_area_ratio { get; set; }
    // 건폐율
    public string BD_coverage_ratio { get; set; }
    // 연면적
    public string BD_total_floor_area { get; set; }
    // 대지면적
    public string BD_land_area { get; set; }
    // 사용승인일자
    public string BD_accecpt_date { get; set; }

    public void print() // 모든 변수 출력해주는 함수
    {
        Debug.Log("BD_road : " + this.BD_road);
        Debug.Log("BD_road_english : " + this.BD_road_english);
        Debug.Log("BD_name : " + this.BD_name);
        Debug.Log("BD_detail_name : " + this.BD_detail_name);
        Debug.Log("BD_purpose : " + this.BD_purpose);
        Debug.Log("BD_structure : " + this.BD_structure);
        Debug.Log("BD_upstage : " + this.BD_upstage);
        Debug.Log("BD_downstage : " + this.BD_downstage);
        Debug.Log("BD_area : " + this.BD_area);
        Debug.Log("BD_height : " + this.BD_height);
        Debug.Log("BD_floor_area_ratio : " + this.BD_floor_area_ratio);
        Debug.Log("BD_coverage_ratio : " + this.BD_coverage_ratio);
        Debug.Log("BD_total_floor_area : " + this.BD_total_floor_area);
        Debug.Log("BD_land_area : " + this.BD_land_area);
        Debug.Log("BD_accecpt_date : " + this.BD_accecpt_date);

    }
}
