using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // UI 클릭시 터치 이벤트 발생 방지 ( EventSystem을 따로 추가해야함)
using System; // Math 함수 쓰기 위함

// 자동스크린샷 UI 버튼에 관련한 모든 기능을 조작
public class KG_RayShot : MonoBehaviour {

    public GameObject Origin; // 자동스크린샷 버튼을 누른 후, 회전 중심 물체를 설정할 때, 선택된 물체를 저장시키기 위한 변수 ( 다른것을 선택하면 변경시켰던 것들을 돌려놔야하기 때문에 )
    public Material OriginMat; // 선택된 물체의 Material을 저장시키는 변수 ( 사실 Obejct 자체로 저장을 했었지만 코딩 편의상 선언 )
    public Material Selected; // 선택된 회전 중심 물체의 색상을 변경시키기 위해 참조되는 함수 ( KG_Materials 폴더속 KG_SelectedObject Material을 사용 )
    public int Select = 0; // 선택에 대한 순서를 위한 변수 ( 0 : 선택된 것 없음 1: 물체 선택, 2: 다른 물체 선택 ) , 1과 2를 반복하고 기능 후에는 0으로 재초기화
    public Scrollbar barYear, barMonth, barDay, barHour, barMinute;    // 시간 입력을 넣기 위해 "Image"의 각 "Group"속 Scrollbar를 참조하기 위한 변수
    private float lat, lon; // Player의 위경도를 실시간으로 저장하기 위한 변수
    float first_lat, first_lon; // SetRegion에서 입력한 위경도를 저장하기 위한 변수
    void Start()
    {
        // ScreenShot을 할 때 저장할 파일명에 필요한 , SetRegion에서 설정한 위경도를 입력 받는 부분. 차후 Screenshot할 때 쓰임.
        first_lat = PlayerPrefs.GetFloat("LATITUDE");
        first_lon = PlayerPrefs.GetFloat("LONGITUDE");
    }
    
    void Update()
    {
        lat = GetLat(Player.transform.position); // 실시간으로 Player의 위경도를 lat,lon 변수에 저장
        lon = GetLon(Player.transform.position);

        if (Origin == null && Select > 0) // 만일 선택된 물체가 존재하지 않은 데, 선택은 했었던 경우 : 즉, 물체를 선택하고 다른 곳으로 이동해서 물체가 없어진 상 
        if (!isRayOn) return; // 확인 차 넣은 검사코드
        if (isRayOn == true) // 만일 '자동 스크린샷' 을 눌렀다면 => 즉, 눌러서 자동스크린샷 수행하려고 한다면
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 화면의 마우스지점에서 빔을 쏜다
            RaycastHit hit; // ray로 쏜 빔에 무언가 맞았다면 이곳에 저장시키려는 변수
            if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭을 했을 때
            {
                if (Physics.Raycast(ray, out hit, 10000) == true && EventSystem.current.IsPointerOverGameObject() == false) // 빔에 어떤 객체가 맞고 && 어떠한 UI 위에 있지 않을 때
                {
                    if (Select == 0) // 처음 선택한 것을 의미
                    { 
                        TurnText.text = "자동 스크린샷 찍기"; //  회전을 하기 위한 물체를 선택했을 때, 회전 가능하다고 Text 변경
                        Origin = hit.transform.gameObject; // 선택한 물체를 표시하기 위해 색을 바꾸기 위해서는 Origin객체에 선택 객체를 담아야한다. 그래야 나중에 되돌릴 수 있다
                        OriginMat = hit.transform.GetComponent<MeshRenderer>().material; // hit 객체의 Material도 담음.
                        hit.transform.GetComponent<MeshRenderer>().material = Selected; // 선택됬다는 의미의 색상을 넣어줌
                        Select = 1; // 1로 바꿔주어 앞으로 1과 2로 계속적인 선택을 해도 같은 코드가 돌도록 한다
                    }
                    else if (Select == 1) // 그 후 선택시
                    {
                        if (Origin == null && Select > 0) // 마찬가지로 지도 범위 밖으로 이동하여 물체가 사라질 시 기능을 끈다
                        {
                            RayOff();
                        }
                        Origin.GetComponent<MeshRenderer>().material = OriginMat; // 다른 물체를 선택시, 다른 물체를 담기전에 select =0에서 선택한 물체의 Material을 돌려놔야한다
                        Origin = hit.transform.gameObject; // 새로운 객체를 넣음.
                        OriginMat = hit.transform.GetComponent<MeshRenderer>().material; // hit 객체의 Material도 담음.
                        hit.transform.GetComponent<MeshRenderer>().material = Selected; // 선택됬다는 의미의 색상을 넣어줌.
                        Select = 2; // 1과 2를 반복하기 위함. 
                    }
                    else if (Select == 2)
                    {
                        if (Origin == null && Select > 0) // 오류 검사 코드
                        {
                            RayOff();
                        }
                        Origin.GetComponent<MeshRenderer>().material = OriginMat; 
                        Origin = hit.transform.gameObject;
                        OriginMat = hit.transform.GetComponent<MeshRenderer>().material; 
                        hit.transform.GetComponent<MeshRenderer>().material = Selected; 
                        Select = 1;
                    }
                }
            }
        }
        else if (isRayOn == false) // Ray기능 끌시
        {
            if (OriginMat == null) // 선택되지 않았더라도 다 비우게 한다
            {
                Origin = null;
                OriginMat = null;
                Select = 0; // 첫 선택을 할 수 있도록 원래의 값 0으로 돌려놓는다
            }
            else if (OriginMat != null) // 무언가 선택되어 Origin에 담겨있다면 원래 Material로 돌려주고 다 비워버린다
            {
                Origin.GetComponent<MeshRenderer>().material = OriginMat;
                Origin = null;
                OriginMat = null;
                Select = 0;
            }
        }
    }

    void RayOff() // 기능을 끄거나 초기화 시킬때 주로 사용하는 함수
    {
        Select = 0; // 초기화
        isRayOn = false; // 기능 자체를 끄기 위해 isRayOn 변수를 false로 만든다. 그렇게 될 시, 밑 스크린샷 기능 자체를 사용불가
        RayOn(); // 한번 RayOn 함수로 껐다가 킴으로써 모든 것을 초기화 시킨다
        RayOn();
    }

    // 버튼 누를 시 RayCast On
    public bool isRayOn = false; // KG_RayOn버튼(== '자동스크린샷' 버튼 )이 눌린 상태인지 아닌 지를 확인하는 변수
    public Text RayOnText; // KG_RayOn의 Text를 변경시키기 위한 참조변수
    public Text TurnText; // RayOn 버튼으로 RayCast가 수행된 상태에서 켜지는 TurnButton에서의 Text를 수정하기 위한 변수
    public Button TurnButton; // KG_RayOn 버튼을 클릭시 함께 켜져야하기 위해 참조하는 KG_TurnButton 참조 변수
    public InputField RspeedInputField; // 위와 동일 이유로 참조하는 KG_RspeedInputField 참조 변수
    public GameObject ShotPanel; // 별 다른 기능이 없는 일반 판넬을 키기 위한 KG_Panel 참조 변수
    public Toggle TimeShot; // 자동스크린샷 수행 시, 옵션 설정을 할 수 있는 시간 옵션 토글 KG_DayTimeShot 토글 참조 변수
    public Toggle RotShot; // 궤도 옵션을 설정할 수 있는 KG_RotShot 토글 참조 변수
    public void RayOn() // '자동스크린샷' 버튼 UI를 클릭했을 때 수행하는 함수
    {
        if (isRayOn == false) // 만약 '자동스크린샷' 버튼을 클릭했다면
        {
            // 다른 토글 클릭 해제 시킴
            GameObject.Find("Make_Facade").GetComponent<Toggle>().isOn = false;
            GameObject.Find("Sun_Location_Predict_toggle").GetComponent<Toggle>().isOn = false;
            GameObject.Find("BD_Info").GetComponent<Toggle>().isOn = false;

            RayOnText.text = "기능 끄기"; // KG_RayOn 버튼의 Text를 바꿔준다
            TurnButton.gameObject.SetActive(true);// 자동스크린샷을 최종적으로 수행하게 하는 버튼 활성화
            RspeedInputField.gameObject.SetActive(true); // 각도를 입력할 수 있는 KG_RspeedInputField 버튼 활성화
            ShotPanel.gameObject.SetActive(true); // KG_Panel을 활성화
            if (!GameObject.Find("KG_ControlAvailableLight")) // 임의 조작 태양을 수행 시, 시간 옵션을 사용할 수 없으므로 임의 조작 태양이 있는 지 확인하고 킨다 
                TimeShot.gameObject.SetActive(true); // 시간 옵션 토글 활성화
            RotShot.gameObject.SetActive(true); // 궤도 옵션 토글 활성화
            TimeShot.isOn = false; // 일종의 초기화
            RotShot.isOn = false; 
            isRayOn = true; // 현재 RayOn 버튼을 누른 상태라고 표시
        }
        else if (isRayOn == true) // '자동스크린샷'에서 '기능끄기'버튼으로 바뀐 '기능끄기'버튼을 클릭 시
        {
            RayOnText.text = "자동 스크린샷"; // 원상태로 돌려놓기 위해 다시 Text를 돌려놓는다
            TurnText.text = "회전 중심물체를 선택하세요"; // 원래의 text로 돌려놓는다
            TurnButton.gameObject.SetActive(false); // 자동스크린샷 기능을 꺼야하기 때문에 위 if와 반대로 적용
            RspeedInputField.gameObject.SetActive(false);
            ShotPanel.gameObject.SetActive(false);
            TimeShot.gameObject.SetActive(false);
            RotShot.gameObject.SetActive(false);
            TimeShot.isOn = false;
            RotShot.isOn = false;
            isRayOn = false;
        }
    }

    //// TurnButton에 수행되는 기능 ////
    public GameObject Player; // 자동스크린샷 기능 수행 시, Player의 위치가 변하게 하면서 찍어야하므로 참조하는 변수
    public Text RspeedText; // 입력값에 의한 각도 회전 하기 위한 참조
    private bool TimeShotOn = false; // 타임 옵션 설정 되었는 지 확인하기 위한 변수
    private bool RotShotOn = false; // 궤도 옵션 확인 변수
    public Slider ProgressBar; // 자동스크린샷 기능 시작 후, 진행상황을 알리기 위한 프로그래스바를 조작하기 위한 참조변수
    public void ScreenShotAvailable() //KG_TurnButton에서 수행되는 함수
    {
        if (TurnText.text.Equals("자동 스크린샷 찍기")) // 물체가 선택되어 Text가 변경된 이후에만 버튼 기능 활성화
        {
            if (Origin != null) // 선택된 물체가 있을 때를 한번 더 확인
            {
                if(int.Parse(RspeedText.text) < 0.0f) // 오류 검사 코드, 입력한 Rspeed가 0보다 작으면 안된다
                    StartCoroutine(error("각도 입력을 안했거나 0보다 작습니다")); //error 코루틴 수행
                else if(ScreenShotSpeed <= 0.0f) // 오류 검사 코드, 입력한 ScreenShotSpeed가 0보다 작으면 안된다
                    StartCoroutine(error("회전 시간이 잘못 되었습니다. 다시 입력하세요."));
                else if (RspeedText.text != null && ScreenShotSpeed > 0.0f) // 만약 둘다 정상적으로 기입이 되었다면
                {
                    GameObject Target; // 선택한 물체를 담을 변수
                    Target = Origin; // 선택을 하였을 때 Origin에 담은 것을 Target 변수에 옮김
                    Origin.GetComponent<MeshRenderer>().material = OriginMat; // Origin에 담긴 객체의 원래 Material로 바꿔줌
                    TimeShotOn = TimeShot.isOn; // 현재 시간 옵션 상황을 담는다
                    RotShotOn = RotShot.isOn; // 궤도 옵션 상황을 담는다
                    RayOn(); // 버튼 UI를 다 안보이게 한다
                    TriggerOn(); // 모든 빌딩의 Trigger를 킨다. ( 그래야 다른 건물에 혹시라도 부딪혔을 때 감지가능) 
                    StartCoroutine(CrossTurn(Target, int.Parse(RspeedText.text))); // 회전중심 물체와 함께 Rspeed 값을 int로 형변환하여 CrossTurn 코루틴 수행
                }
            }
        }
    }


    public Toggle ThreeShot; // 희진이형 세개 찍어주기
    

    public Camera Maincam; // Player속 메인카메라를 참조하는 변수
    Vector3 POriginPosition; // Player의 원래 위치를 담아야하기 때문. 그래야 스크린샷 찍을때 파일명을 관리하고 원래 위치로 돌아가게 하기 위해 참조하는 변수
    Quaternion POriginRotation; // 원래 회전각도로도 돌려놓기 위한 Player 회전 값 참조 변수
    public GameObject TouchDisablePanel; // 아무것도 못만지게 하는 판넬을 참조하는 변수
    public bool isCancleOn = false; // 취소버튼 누를 시 수행하는 자동스크린샷을 멈추기 위해 참조하는 변수
    public Text TimeInputText; // 시간 입력을 받기 위한 InputText
    public float ScreenShotSpeed = 1.0f; // 스크린샷 속도
    IEnumerator CrossTurn(GameObject Target, float Rspeed) // 자동스크린샷 수행 함수
    {
        TouchDisablePanel.SetActive(true); // 자동스크린샷이 시작되면 우선 다른 버튼을 누르면 안되기 때문에 판넬을 킨다
        GameObject.Find("Player").GetComponent<KG_PlayerMove>().isSCActivated = true; // PlayerMove 스크립트의 isSCActivated를 true로 만들어 키보드 조작으로 인한 Player의 이동을 못하게 막는다
        int scNumber = 0; // 파일을 저장하기 위해 쓰이는 순서 인덱스

        POriginPosition = Player.transform.position; // 현재 찍는 Player의 원래 위치를 담음
        POriginRotation = Player.transform.rotation;

        float lat = GetLat(POriginPosition); // Player의 현 위경도를 얻는다
        float lon = GetLon(POriginPosition);
        string filePath = string.Format("{0}/../ScreenShot/{1}_{2}", Application.dataPath, lat, lon); // 위경도에 따른 파일디렉토리명
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath); // 디렉토리 생성
        if (!di.Exists) di.Create(); // 만약 디렉토리가 없다면 만들고
        else if(di.Exists) // 같은 디렉토리 명이 존재한다면
        {
            int i = 0; // 디렉토리 인덱스
            while (true)
            {
                filePath = string.Format("{0}/../ScreenShot/{1}_{2}({3})", Application.dataPath, lat, lon,i);
                System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(filePath); // index 숫자를 붙이고 다시 검사한다
                if (d.Exists) // 만약 있다면
                {
                    i++; // i를 올려서 한번 더 검사
                    continue;
                }
                else // 없다면
                {
                    d.Create(); //그 이름으로 만든다. ( 중복 방지용. 사실 오버로딩 방지용 )
                    break;
                }
            }
        }

        if (ThreeShot.isOn == true) // 희진이형을 위한 코드
        {
            ProgressBar.maxValue = (int)(360f / 30) * 3f; // 총 찍는 개수는 360도를 Rspeed로 나눈 것뿐이지만, GT사진까지 같이 찍기 때문에 2배
            Vector3 OriginPosition = Target.transform.position; // Target의 원래포지션 
            float StopRot = 0.0f; // StopRot에 Rspeed를 더하는 것을 통해 정해준 Rspeed를 더한 것이 360도를 넘었는 지 확인하기 위한 변수

            GameObject.Find("Three_Shot_Manager").GetComponent<Three_Shot>().getshot(); // 스크린 샷 기능

            while (!isCancleOn) // cancle 버튼 누를 시 isCanleOn이 바뀌는 데, 그럴 시 하던 반복까지만 하고 그만 돌게 된다
            {
                Player.transform.RotateAround(Target.transform.position, GetSurfacePosDeg(lat, lon), Rspeed);
                if (Player.transform.GetComponent<KG_CameraTrigger>().OnTrigger == false)
                {
                    if (GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().isShadowOn == true)
                   
                        GameObject.Find("Three_Shot_Manager").GetComponent<Three_Shot>().getshot(); // 스크린 샷 기능


                    else
                    {
                        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                        GameObject.Find("Three_Shot_Manager").GetComponent<Three_Shot>().getshot(); // 스크린 샷 기능
                    }

                }
                else if (Player.transform.GetComponent<KG_CameraTrigger>().OnTrigger == true) // 건물에 접촉 됬다면 아무 일도 없어야하고 프로그래스 바의 최대값에서 안찍는 사진의 개수를 빼야한다
                    ProgressBar.maxValue -= 2f; // 건물 접촉시 2개 낮춤 / 프로그래스 바는 건물 접촉 등으로 인해 100퍼가 넘거나 낮아도 진행되거나 멈출 가능성 높음.

                StopRot += Rspeed; // StopRot을 Rspeed만큼 증가시킨다
                if (StopRot + Rspeed >= 360.0f) break; // StopRot에 다음 Rspeed까지 더한 것이 360도를 넘어서면 그만 멈춤. 왜냐하면 예로 180으로 했을 경우, 이미 처음 시작할때 찍었던 사진인데 360되었을 때 또 찍는다면 중복이기 때문
                yield return new WaitForSeconds(ScreenShotSpeed); // 한번 기다린다

            }
        }


        else if (TimeShotOn == false && RotShotOn == false) // 시간 옵션과 궤도 옵션을 선택안한 일반 자동스크린샷
        {
            ProgressBar.maxValue = (int)(360f / Rspeed) * 2f; // 총 찍는 개수는 360도를 Rspeed로 나눈 것뿐이지만, GT사진까지 같이 찍기 때문에 2배

            Vector3 OriginPosition = Target.transform.position; // Target의 원래포지션 
            float StopRot = 0.0f; // StopRot에 Rspeed를 더하는 것을 통해 정해준 Rspeed를 더한 것이 360도를 넘었는 지 확인하기 위한 변수
            if(GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().isShadowOn == true) // 그림자 버튼을 누른 상태에서도 찍을 수 있게 하기 위함
            {
                ScreenShot(Maincam, scNumber, filePath, ""); // 우선 찍는다
                yield return new WaitForSeconds(ScreenShotSpeed); // 한번 기다린다
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change(); // 그림자를 끈다
                ScreenShot(Maincam, scNumber++, filePath, "GT"); // 그림자가 제거된 사진을 찍는다. GT라는 string을 따로 추가
                yield return new WaitForSeconds(ScreenShotSpeed); // 한번 더 기다린다
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change(); // 다시 원상태로 복귀
            }
            else // 그림자 버튼이 눌려져 있는 상태에서는 반대로만 해주면 된다
            {
                ScreenShot(Maincam, scNumber, filePath, "GT");
                yield return new WaitForSeconds(ScreenShotSpeed);
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                ScreenShot(Maincam, scNumber++, filePath, "");
                yield return new WaitForSeconds(ScreenShotSpeed);
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
            }

            while (!isCancleOn) // cancle 버튼 누를 시 isCanleOn이 바뀌는 데, 그럴 시 하던 반복까지만 하고 그만 돌게 된다
            {
                Player.transform.RotateAround(Target.transform.position, GetSurfacePosDeg(lat, lon), Rspeed); // RotateAround함수를 통해 회전
                /* 회전의 중심은 선택했던 물체의 중심
                 * lat,lon을 GetSurfacePosDeg를 통해 회전각을 알아내어 축을 회전시킨다
                 * Rspeed는 그 물체를 중심으로 Rspeed 각도만큼 회전하라는 의미
                 */

                yield return new WaitForSeconds(ScreenShotSpeed); // 잠시 기다린다
                if (TimeShotOn == false && Player.transform.GetComponent<KG_CameraTrigger>().OnTrigger == false) // 만일 회전하는 Player가 건물이나 타일의 Trigger에 들어가있지 않다면
                {
                    if (GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().isShadowOn == true) // 자동스크린샷 수행
                    {
                        ScreenShot(Maincam, scNumber, filePath, "");
                        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                        ScreenShot(Maincam, scNumber++, filePath, "GT");
                        yield return new WaitForSeconds(ScreenShotSpeed);
                        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                    }else
                    {
                        ScreenShot(Maincam, scNumber, filePath, "GT");
                        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                        ScreenShot(Maincam, scNumber++, filePath, "");
                        yield return new WaitForSeconds(ScreenShotSpeed);
                        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                    }
                    
                }
                else if (Player.transform.GetComponent<KG_CameraTrigger>().OnTrigger == true) // 건물에 접촉 됬다면 아무 일도 없어야하고 프로그래스 바의 최대값에서 안찍는 사진의 개수를 빼야한다
                    ProgressBar.maxValue -= 2f; // 건물 접촉시 2개 낮춤 / 프로그래스 바는 건물 접촉 등으로 인해 100퍼가 넘거나 낮아도 진행되거나 멈출 가능성 높음.

                StopRot += Rspeed; // StopRot을 Rspeed만큼 증가시킨다
                if (StopRot + Rspeed >= 360.0f) break; // StopRot에 다음 Rspeed까지 더한 것이 360도를 넘어서면 그만 멈춤. 왜냐하면 예로 180으로 했을 경우, 이미 처음 시작할때 찍었던 사진인데 360되었을 때 또 찍는다면 중복이기 때문
            }
         }

        else if(TimeShotOn == true && TimeInputText != null) // 시간 옵션 설정을 하였고 시간입력란에 무언가 써져 있다면
        {
            Vector3 OriginPosition = Target.transform.position;
            string timetext = TimeInputText.text; // 시간 입력란의 text를 받아온다
            if(timetext.Length < 27 || !timetext.Contains("~") || !timetext.Contains("/")) // 제대로 타임 텍스트가 되어있는 지 확인하기위해 일정 개수 이상이고 , 형식에 맞는 ~와 /를 썼는 지 확인
            {
                isCancleOn = true; // 만약 아니라면 반복문을 그만하기 위해 바로 isCancleOn을 true로 바꾼다
                StartCoroutine(error("시간을 제대로 다시 입력하시오")); // 에러 메세지 출력
            }

            // 제대로 된 시간 입력  = 2013.1.1.1.1~20131.1.1.1.2/1m
            string[] Split_TimeInterval = timetext.Split('/'); // '/'로 분리한다
            if(Split_TimeInterval[0].Length <25) // 만일 '/' 앞 string의 길이가 25를 안넘는다면 임의로 설정해 에러가 나는 것을 우선 막는다
            {
                timetext = "2013.1.1.1.1~2013.1.1.1.2/1m";
                Split_TimeInterval = timetext.Split('/');
            }

            string Times = Split_TimeInterval[0]; // '/' 앞 string
            string[] T = Times.Split('~'); // 앞 string을 ~로 쪼갠다
            string[] startTime = T[0].Split('.'); // '~' 앞 시작 시간을 '.'를 단위로 쪼갠다
            string[] endTime = T[1].Split('.');// '~' 뒷 시작 시간을 '.' 단위로 쪼갠다
            string interval = Split_TimeInterval[1]; // '/' 뒤 시간간격을 담는다

            if (timetext == null || Split_TimeInterval == null || Times == null || T == null || interval == null) // 무엇이라도 담기지 않았다면 취소를 해야한다
            {
                isCancleOn = true;
                StartCoroutine(error("무언가 잘못 입력되었습니다"));
            }

            // 년월일시분 모두 변수에 넣어준다
            string startTimeY = startTime[0];
            string startTimeM = startTime[1];
            string startTimeD = startTime[2];
            string startTimeH = startTime[3];
            string startTimem = startTime[4];
            string endTimeY = endTime[0];
            string endTimeM = endTime[1];
            string endTimeD = endTime[2];
            string endTimeH = endTime[3];
            string endTimem = endTime[4];

            // Datetime 클라스로 string이였던 값들을 모두 int로 형변환하여 시작시간과 끝 시간을 Datetime 변수로 만들어놓는다
            DateTime start = new DateTime(int.Parse(startTimeY), int.Parse(startTimeM), int.Parse(startTimeD), int.Parse(startTimeH), int.Parse(startTimeM), 0);
            DateTime end = new DateTime(int.Parse(endTimeY), int.Parse(endTimeM), int.Parse(endTimeD), int.Parse(endTimeH), int.Parse(endTimeM), 0);
            TimeSpan timeSpan = end - start; // Timespan 클래스로 두 시간의 차이를 계산한다

            //시간 간격을 확인하고 그에 따른 회전횟수를 계산하기 위한 로직
            double TimeTotal=0.0f; // 총 시간을 의미한다
            string intervalTime = "";
            if(interval.Contains("Y")) // 만일 예시로 1Y로 되어있다면, 2013~2015까지 1년단위로 시간을 올리고 싶다는 것이다
            {
                intervalTime += "Y"; // intervalTime에 Y를 설정
                TimeTotal = timeSpan.TotalDays / 365; // 끝 - 시작의 시간을 365로 나누어 몇년인지 구한다
            }
            else if(interval.Contains("M"))
            {
                intervalTime += "M";
                TimeTotal = timeSpan.TotalDays / 30;
            }
            else if (interval.Contains("D"))
            {
                intervalTime += "D";
                TimeTotal = timeSpan.TotalDays;
            }
            else if (interval.Contains("H"))
            {
                intervalTime += "H";
                TimeTotal = timeSpan.TotalHours;
            }
            else if (interval.Contains("m"))
            {
                intervalTime += "m";
                TimeTotal = timeSpan.TotalMinutes;
            }
            else // Y M D M m을 잘못 입력했다면 우선 설정한다
            {
                intervalTime += "Y";
                TimeTotal = timeSpan.TotalDays / 365;
            }
            interval = interval.Remove(interval.Length - 1); // 아까 시간 간격을 담은 interval의 뒤 1개를 빼면 30m에서 30만 남게된다
            
            //시작 시간을 실제 value에 집어 넣는다
            barYear.value = (float.Parse(startTimeY) - 1900.0f+1.0f) / 200.0f;
            barMonth.value = (float.Parse(startTimeM) - 1.0f+1.0f) / 12.0f;
            barDay.value = (float.Parse(startTimeD) - 1.0f+1.0f) / 31.0f;
            barHour.value = ((float.Parse(startTimeH))/24.0f);
            barMinute.value = (float.Parse(startTimem)) / 60.0f;
            ProgressBar.maxValue = (int)(360f / Rspeed) * 2f * (int)((int)TimeTotal / int.Parse(interval));// 기본 횟수를 년수만큼 돌리는 것이므로 TimeTotal / interval 만큼 돌린다. 즉 한바퀴 돌고 1년 올리고 한바퀴 돌는 형식

            DateTime currentTime = start; // 시작시간을 현재 시간에 넣는다

            // 우선 지금 그 위치에서 한장을 찍는다
            if (GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().isShadowOn == true)
            {
                ScreenShot(Maincam, scNumber, filePath, "");
                yield return new WaitForSeconds(ScreenShotSpeed);
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                ScreenShot(Maincam, scNumber++, filePath, "GT");
                yield return new WaitForSeconds(ScreenShotSpeed);
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
            }else
            {
                ScreenShot(Maincam, scNumber, filePath, "GT");
                yield return new WaitForSeconds(ScreenShotSpeed);
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                ScreenShot(Maincam, scNumber++, filePath, "");
                yield return new WaitForSeconds(ScreenShotSpeed);
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
            }


            // 회전을 하기 전에, 궤도가 설정되어있다면 궤도에 따른 각도를 고려해야한다
            Vector3 tempsur = GetSurfacePosDeg(lat, lon); // 현재 회전 각도
            Vector3 tempfor = Player.transform.forward; // 현재 카메라가 바라보는 각도
            Vector3 tempcross = Vector3.Cross(tempsur, tempfor); // 둘 사이의 정중앙 벡터를 구함 . 그렇게 될 시 케메라가 지금 바라보는 벡터와 Player의 위치의 lat lon을 이용한 회전 축를 알 시에, 현재 선택된 물체와 카메라 사이에 축을 구할 수 있음.
            Quaternion mtemp = Quaternion.Euler(GameObject.Find("KG_Manager").GetComponent<KG_RotShot>().RotAngle , 0f, 0f); // 이걸 곱할수록 축의 x좌표 즉 좌우가 회전된다. RotAngle은 궤도 설정시 회전각도이다
            Vector3 AddRot = mtemp * tempcross;  // mtemp를 tempcross에 곱한 값
            Vector3 VerticalRot = tempcross; // 그냥 세로 회전 값

            while (true)
            {
                TimeSpan isover = end - currentTime; // 점점 올라가는 currentTime이 end보다 커져서 isover가 0보다 작아지면 멈춘다
                if (isover.TotalMinutes < 0 || isCancleOn == true)
                    break;

                float StopRot = 0.0f;
                
                while (!isCancleOn)
                {
                    if(RotShotOn == false) // 궤도 옵션이 설정 안되었다면 평상시 대로 회전
                        Player.transform.RotateAround(Target.transform.position, GetSurfacePosDeg(lat, lon), Rspeed);
                    else if(RotShotOn == true) // 궤도 옵션이 설정되었다면
                    {
                        Player.transform.RotateAround(Target.transform.position, mtemp * VerticalRot, Rspeed); // RotateAround의 축을 위에 계산한 축으로 넣는다
                        if (StopRot > 90) break; // 대신 이렇듯 세로로 회전할 경우에는 바닥을 뚫고 나갈 우려가 있기도 하고 땅바닥에서 찍는 경우도 별로 없으므로 90도 정도만 찍어도 충분하다고 판단
                    }
                    yield return new WaitForSeconds(ScreenShotSpeed);

                    if (TimeShotOn == true && Player.transform.GetComponent<KG_CameraTrigger>().OnTrigger == false) // 건물에 부딪히지 않았다면
                    {
                        if (GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().isShadowOn == true)
                        {
                            ScreenShot(Maincam, scNumber, filePath, "");
                            GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                            ScreenShot(Maincam, scNumber++, filePath, "GT");
                            yield return new WaitForSeconds(ScreenShotSpeed);
                            GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                        }else
                        {
                            ScreenShot(Maincam, scNumber, filePath, "GT");
                            GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                            ScreenShot(Maincam, scNumber++, filePath, "");
                            yield return new WaitForSeconds(ScreenShotSpeed);
                            GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                        }
                            
                    }
                    else if (Player.transform.GetComponent<KG_CameraTrigger>().OnTrigger == true) // 건물에 접촉 됬다면 아무 일도 없어야하고 프로그래스 바의 최대값을 1 낮춰야함
                        ProgressBar.maxValue -= 2f;

                    StopRot += Rspeed;
                    if (StopRot + Rspeed >= 360.0f)
                        break;
                }
                Player.transform.position = POriginPosition; //한번 회전을 다했다면 원래 위치로 이동시켜준다
                Player.transform.rotation = POriginRotation;

                // 입력한 시간단위에 따라 한번 회전했다면 그만큼 bar를 이동시키고 (태양을 회전시켜야하므로), currentTime도 그만큼 증가시킨다
                if (intervalTime.Equals("m"))
                {
                    barMinute.value = barMinute.value + float.Parse(interval) / 60.0f;
                    currentTime = currentTime.AddMinutes(double.Parse(interval));
                }
                else if (intervalTime.Equals("H"))
                {
                    barHour.value = barHour.value + float.Parse(interval) / 24f;
                    currentTime = currentTime.AddHours(double.Parse(interval));
                }
                else if (intervalTime.Equals("D"))
                {
                    barDay.value = barDay.value + float.Parse(interval) / 31f;
                    currentTime = currentTime.AddDays(double.Parse(interval));
                }
                else if (intervalTime.Equals("M"))
                {
                    barMonth.value = barMonth.value + float.Parse(interval) / 12f;
                    currentTime = currentTime.AddMonths(int.Parse(interval));
                }
                else if (intervalTime.Equals("Y"))
                {
                    barYear.value = barYear.value + float.Parse(interval) / 200f;
                    currentTime = currentTime.AddYears(int.Parse(interval));
                }


                // bar의 최대 value까지 갔을 경우, 즉 59초까지 갔을 경우 1분을 추가시키는 동시에 0초로 만드는 작업이다
                if (barMinute.value >= 1.00f)
                {
                    barMinute.value = 0.00f+1/60f;
                    barHour.value = barHour.value + 1f/24f;
                }
                if(barHour.value >= 1.00f)
                {

                    barHour.value = 0.00f + 1f/24f;
                    barDay.value = barDay.value + 1f/31f;
                }
                if (barDay.value >= 1.00f)
                {

                    barDay.value = 0.00f;
                    barMonth.value = barMonth.value + 1f / 12f;
                }
                if (barMonth.value >= 1.00f)
                {

                    barMonth.value = 0.00f;
                    barYear.value = barYear.value + 1f / 200f;
                }
                if (barYear.value >= 1.00f)
                {
                    barMinute.value = 0.0f;
                    barHour.value = 0.0f;
                    barDay.value = 0.0f;
                    barMonth.value = 0.0f;
                    barYear.value = 0.0f;
                    break; // 넘으면 그 후는 할 수 없으므로 그냥 끝낸다.
                }
            }

        }else if (RotShotOn == true && TimeShotOn == false) // 궤도 옵션만 설정했을 경우
        {

            ProgressBar.maxValue = (int)(360f / Rspeed) * 2f; 
            Vector3 OriginPosition = Target.transform.position; 
            float StopRot = 0.0f;

            if (GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().isShadowOn == true)
            {
                ScreenShot(Maincam, scNumber, filePath, "");
                yield return new WaitForSeconds(ScreenShotSpeed);
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                ScreenShot(Maincam, scNumber++, filePath, "GT");
                yield return new WaitForSeconds(ScreenShotSpeed);
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
            }
            else
            {
                ScreenShot(Maincam, scNumber, filePath, "GT");
                yield return new WaitForSeconds(ScreenShotSpeed);
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                ScreenShot(Maincam, scNumber++, filePath, "");
                yield return new WaitForSeconds(ScreenShotSpeed);
                GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
            }

            Vector3 tempsur = GetSurfacePosDeg(lat, lon); // 현재 회전 각도
            Vector3 tempfor = Player.transform.forward; // 현재 카메라가 바라보는 각도
            Vector3 tempcross = Vector3.Cross(tempsur, tempfor); // 둘 사이의 정중앙 벡터를 구함 . 그렇게 될 시 케메라가 지금 바라보는 벡터와 Player의 위치의 lat lon을 이용한 회전 축를 알 시에, 현재 선택된 물체와 카메라 사이에 축을 구할 수 있음.
            Quaternion mtemp = Quaternion.Euler( GameObject.Find("KG_Manager").GetComponent<KG_RotShot>().RotAngle, 0f, 0f); // 이걸 곱할수록 틀어짐. x로 하면 됨.
            Vector3 AddRot = mtemp * tempcross;
            Vector3 VerticalRot = tempcross;

            while (!isCancleOn)
            {
                Player.transform.RotateAround(Target.transform.position, mtemp * VerticalRot, Rspeed); // 회전시킨다
                yield return new WaitForSeconds(ScreenShotSpeed);

                if (TimeShotOn == false && Player.transform.GetComponent<KG_CameraTrigger>().OnTrigger == false)
                {
                    if (GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().isShadowOn == true)
                    {
                        ScreenShot(Maincam, scNumber, filePath, "");
                        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                        ScreenShot(Maincam, scNumber++, filePath, "GT");
                        yield return new WaitForSeconds(ScreenShotSpeed);
                        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                    }
                    else
                    {
                        ScreenShot(Maincam, scNumber, filePath, "GT");
                        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                        ScreenShot(Maincam, scNumber++, filePath, "");
                        yield return new WaitForSeconds(ScreenShotSpeed);
                        GameObject.Find("KG_Manager").GetComponent<KG_ShadowControl>().Shadow_Change();
                    }

                }
                else if (Player.transform.GetComponent<KG_CameraTrigger>().OnTrigger == true) // 건물에 접촉 됬다면 아무 일도 없어야하고 프로그래스 바의 최대값을 1 낮춰야함
                    ProgressBar.maxValue -= 2f; // 건물 접촉시 2개 낮춤. 
                
                StopRot += Rspeed; // StopRot을 각도만큼 올림.
                if (StopRot + Rspeed >= 360.0f || StopRot > 90) break;
            }
        }

        TriggerOff(); // 켰던 건물의 Tigger를 끈다
        Player.transform.position = POriginPosition; // Player를 원위치로 원상복귀
        Player.transform.rotation = POriginRotation; // 회전도 원상복귀

        TouchDisablePanel.SetActive(false); // 만지지 못하게 하는 Panel을 비활성화
        GameObject.Find("Player").GetComponent<KG_PlayerMove>().isSCActivated = false; // 키보드 조작도 활성화
        ProgressBar.value = 0; // ProgressBar의 value를 0으로 초기화 해준다
        isCancleOn = false; // 확실하게 끝내도록 false해준다
        yield return null;
    }
    
    //// 회전 축 설정하기 위해 Earth.cs에서 가져온 함수 ////
    Vector3 GetSurfacePosDeg(float lat, float lon) // 위경도에 따른 회전각을 반환 ( Earth.cs 에서 그대로 가져온 것 )
    {
        var height = 50.0f;
        var rotate = Quaternion.Euler(0f, -lon, lat);
        var v = rotate * new Vector3((6378137f + height), 0f, 0f);
        return v;
    }
    float GetLat(Vector3 currPos) // 현재위치의 위도를 반환 ( Earth.cs 에서 그대로 가져온 것 )
    {
        float lat = 0.0f;
        Vector3 pos = currPos + GameObject.Find("TileObject").GetComponent<Earth>().origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);
        lat = Vector3.Angle(projVec, pos);
        return lat;
    }
    float GetLon(Vector3 currPos) // 현재위치의 경도를 반환하는 함수 ( Earth.cs 에서 그대로 가져 온 것 )
    {
        float lon = 0.0f;
        Vector3 pos = currPos + GameObject.Find("TileObject").GetComponent<Earth>().origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);
        lat = Vector3.Angle(projVec, pos);
        lon = Vector3.Angle(new Vector3(1f, 0f, 0f), projVec);
        return lon;
    }


    //// 스크린샷을 수행하는 기능 ////
    public Text UIFilePath; // 저장할 파일 경로를 얻어올 변수
    public Camera skycamera; // 이미지 저장시 Skycamera에서 배경을 렌더링하기 위해 참조하는 변수
    public Text YearValue, MonthValue, DayValue, HourValue, MinuteValue; // 파일명을 정하기 위해서는 설정한 날짜를 다 기록해야기 때문에 참조받는 변수 (Image->Group -> Text -> Scrollbar -> Value )
    void ScreenShot(Camera camera, int num , string filePath, string isGT) // 현재 Player 내부 Camera에서 보는 화면의 사진 한장을 저장하는 함수
    {
        int resWidth = Screen.width, resHeight = Screen.height; // 저장할 이미지 해상도의 가로 세로 픽셀 개수 설정. / 화면 크기에 따라 저장하고 싶다면 Screen.width / height로 하면 된다
        UIFilePath.text = Application.dataPath + "/../ScreenShot"; // Asset에서 디렉토리 명
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        skycamera.targetTexture = rt;
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        skycamera.Render(); // 배경을 먼저 렌더링한 후에 camera 렌더링
        camera.Render(); // 순서가 바뀌면 하늘만 보인다
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        skycamera.targetTexture = null;
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToJPG(); // JPG 파일 형식으로 byte배열에 저장
        Destroy(screenShot); // 랜더링한 텍스쳐를 지워야한다. 안지우면 메모리를 계속적으로 차지하기에 memory leak문제로 프로그램 멈춤    
       string filename = null;
        if (GameObject.Find("KG_Manager").GetComponent<KG_LightPositionControl>().isOn == false) // 임의 태양이 아닐 때 실제 태양의 파일명 모드
        {
            filename = string.Format("{0}/first({10},{11})_current({12},{13})_DL({3}y{4}m{5}d{6}h{7}m)_P{8}_R{9}_num_{2}_{1}.jpg", filePath, num, isGT, YearValue.text, MonthValue.text, DayValue.text, HourValue.text, MinuteValue.text
                , Player.transform.position, Player.transform.eulerAngles, first_lat, first_lon, lat, lon);
        }
        else
        {
            Light CL_Rotation = GameObject.Find("KG_Manager").GetComponent<KG_LightPositionControl>().CL;
            filename = string.Format("{0}/first({6},{7})_current({8},{9})_CL({3})_P{4}_R{5}_num_{2}_{1}.jpg", filePath, num, isGT, CL_Rotation.transform.eulerAngles, Player.transform.position, Player.transform.eulerAngles, first_lat, first_lon, lat, lon);
        }
        System.IO.File.WriteAllBytes(filename, bytes); //읽은 byte를 filename으로 저장
        GameObject.Find("KG_Manager").GetComponent<KG_PixelOption>().PixelOption(filename); // KG_PixelOption Script의 PixelOption으로 INFO 파일 생성
        ProgressBar.value += 1.0f; // ProgressBar의 value를 올려 1장을 찍었다는 것을 표시
    }


    //// 자동스크린샷을 하다보면 건물 안으로 들어가게 되는 경우들이 있다. 그럴 경우를 고려하여 건물 내부로 들어가게 될 경우를 알기 위해 Trigger를 키는 기능 ////
    private MeshCollider[] Build_Meshs; // 건물들의 Tigger를 조작하기 위해 MeshRender를 받아올 배열
    private void TriggerOn() // 현존하는 건물들의 Trigger를 키는 함수 ( 타일까지는 고려안해도 된다고 생각하여 안했다 )
    {
        Build_Meshs = GameObject.Find("BuildObject").GetComponentsInChildren<MeshCollider>(); // 현재 BuildObject에 존재하는 건물들의 MeshCollider를 받아온다
        foreach (MeshCollider col in Build_Meshs) // 각 건물의 MeshCollider의 Trigger를 킨다
            col.isTrigger = true;
    }
    private void TriggerOff() // 켰던 건물들의 Trigger를 끄는 함수 (  Trigger On 의 반대 )
    { 
        Build_Meshs = GameObject.Find("BuildObject").GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider col in Build_Meshs)
            col.isTrigger = false;
    }
    
   //// 이동을 하다보면 본인의 위경도가 도대체 어디쯤인지 알기 위한 기능 ////
   public Text LatText, LonText; // 현재 'Player'의 위치를 시각적으로 보여주기 위해 LatText와 LonText를 참조 받을 변수
   void LateUpdate() // Update보단 느린 프레임으로 도는 함수로써 위경도 갱신 함수
   {
        LatText.text = "Lat : " + GetLat(Player.transform.position); // 현재 플레이어의 Unity상의 위치를 GetLat 함수로 계산하여 경도로 만들고, LatText에 덧붙인다
        LonText.text = "Lon : " + GetLon(Player.transform.position);
   }

    //// 잘못된 입력등을 하였을 경우에 시각적으로 보여주기 위한 기능 ////
    public GameObject ErrorMessage; // 에러 메세지 출력을 하기 위해 KG_ErrorMessage를 참조받는 변수
    IEnumerator error(string err) // 에러 메세지 출력 함수
    {
        ErrorMessage.SetActive(true); // 꺼져있던 에러메세지를 활성화하여 보이게 한다
        ErrorMessage.GetComponent<Text>().text = err; // 에러메세지 객체의 Text의 text에 받은 err string을 넣는다

        float fading = 0.0f; // 서서히 사라지게 하기 위한 fade 변수
        float fadingTime = 0.001f; // 서서히 사라지게 하기 위해 fade 변수를 빼는 시간 변수
        while (true)
        {
            ErrorMessage.GetComponent<Text>().color = new Color(255, 255, 255, 1 - fading); // 에러메세지 객체의 Color의 투명도 값에서 fading 값을 뺀다
            fading += 0.01f; // fading 변수 값을 올려서 다음 반복때 좀 더 투명하게 하게끔 빼줌
            yield return new WaitForSeconds(fadingTime); // fadingTime만큼 기다렸다가 반복
            if (ErrorMessage.GetComponent<Text>().color.a < 0.0f) break; //만일 투명도가 0.0f보다 떨어졌을 경우에는 그만 반복
        }
        ErrorMessage.GetComponent<Text>().color = new Color(255, 255, 255, 1); // 다음에 호출할 에러메세지를 위해 감마값을 원상태로 복구
        ErrorMessage.SetActive(false); // 수행을 끝냈기에 처음처럼 비활성화시킨다
        yield return null;
    }
    

}
