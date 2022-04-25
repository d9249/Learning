using UnityEngine;
using System.Collections;

public class CameraAction : MonoBehaviour {
    // 지구반지름
    const float earthRadius = 6378137f / 1000f;

    private Vector3 m_lastMousePosition = Vector3.zero;

    // Use this for initialization
    void Start () {
        
        //this.transform.LookAt(Vector3.zero);

    }
	
	// Update is called once per frame
	void Update () {
        
        var wheelDelta = Input.GetAxis("Mouse ScrollWheel");
        // 줌아웃 가속도 구하기
        var v = Mathf.Log10(this.transform.position.magnitude);
        var delta = Mathf.Pow(5, v);
        if (wheelDelta < 0)
        {
            // 줌아웃
            // print("줌아웃" + wheelDelta+ this.transform.position.magnitude);    
            this.transform.Translate(this.transform.forward.normalized * -delta, Space.World);
            print(this.transform.position.normalized + " " + this.transform.forward);
            // UpdateTiles();
        }
        else if (wheelDelta > 0)
        {
            // 줌인
            // print("줌인" + wheelDelta);
            var vec = this.transform.position;
            if ((vec.magnitude - delta) < (earthRadius + 0.3f))
            {
                delta = vec.magnitude - earthRadius - 0.3f;
                print(delta);
            }
            this.transform.Translate(this.transform.forward.normalized * delta, Space.World);
            print(this.transform.position.normalized + " " + this.transform.forward);
            //UpdateTiles();
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 버튼이 눌러졌을 때 위치 저장
            m_lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            var mouseMoveDelta = Input.mousePosition - m_lastMousePosition;

            print(string.Format("{0} {1}", mouseMoveDelta.x, mouseMoveDelta.y));

            // 카메라 회전
            this.transform.RotateAround(Vector3.zero, Vector3.up, mouseMoveDelta.x * -1.0f);
            this.transform.RotateAround(Vector3.zero, this.transform.right, mouseMoveDelta.y);
            print(this.transform.position.normalized + " " + this.transform.forward);

            m_lastMousePosition = Input.mousePosition;
        }
        
    }

    void UpdateTiles()
    {
        // 원점
        Vector3 O = new Vector3(0f, 0f, 0f);
        // 절두체 검사 실시
        var c = Camera.main;
        // [0] = Left, [1] = Right, [2] = Down, [3] = Up, [4] = Near, [5] = Far
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        // 좌우 범위 추출
        // 좌측 면과 원점과의 거리 구하기
        var leftPlaneDistToO = planes[0].GetDistanceToPoint(O);
        print("절두체 좌측면과의 거리 : " + leftPlaneDistToO);
        // 카메라에서 원점까지의 거리
        var camDistToO = c.transform.position.magnitude;

        float hRangeInDeg = 120;
        if (leftPlaneDistToO < earthRadius)
        {
            // 교차하므로 내각을 구해야 함    
            var alpha = Mathf.Asin(leftPlaneDistToO / camDistToO);
            var beta = Mathf.Acos(leftPlaneDistToO / earthRadius);
            var theta = Mathf.PI / 2f - alpha - beta;
            hRangeInDeg = theta * 2 * Mathf.Rad2Deg;
        }
        print("좌측면 교차점과의 내각: " + hRangeInDeg);
        var hN = GetTileLevel(hRangeInDeg, Screen.width);
        print("가로 판단 타일 레벨: " + hN);
        print("가로 타일 개수 :" + (hRangeInDeg / (36 / Mathf.Pow(2, hN))));

        // 상하 범위 추출
        var topPlaneDistToO = planes[3].GetDistanceToPoint(O);
        float vRangeInDeg = 120;
        if (topPlaneDistToO < earthRadius)
        {
            // 교차하므로 내각을 구해야 함
            var alpha = Mathf.Asin(topPlaneDistToO / camDistToO);
            var beta = Mathf.Acos(topPlaneDistToO / earthRadius);
            var theta = Mathf.PI / 2f - alpha - beta;    
            vRangeInDeg = theta * 2 * Mathf.Rad2Deg;
        }
        print("윗면 교차점과의 내각: " + vRangeInDeg);
        var vN = GetTileLevel(vRangeInDeg, Screen.height);
        print("세로 판단 타일 레벨: " + vN);
        print("세로 타일 개수 :" + (vRangeInDeg / (36 / Mathf.Pow(2, vN))));
    }

    // 적정 타일 레벨을 구함
    int GetTileLevel(float tileRange, int screenSize)
    {
        var tileCnt = Mathf.Round(screenSize / 65f);
        if (tileCnt <= 0) tileCnt = 1;
        var n = Mathf.Log(36 * tileCnt / tileRange) / Mathf.Log(2);

        return (int)Mathf.Round(n);
    }

    

}
