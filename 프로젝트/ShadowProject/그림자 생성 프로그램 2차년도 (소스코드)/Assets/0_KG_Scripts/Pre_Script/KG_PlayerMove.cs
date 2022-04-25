using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마우스와 키보드 조작 클래스
public class KG_PlayerMove : MonoBehaviour {

    // 회전 감도 변수.
    public float Sensitivity = 1.0f;
    // 카메라 이동 속도
    public float CameraSpeed = 10.0f;
    public float BoosterSpeed = 0.5f;
    // 줌인 아웃 속도
    public float ZoomSpeed = 1.0f;
    // 자동스크린샷 수행시에 키보드 조작을 막기 위한 변수.
    public bool isSCActivated = false;
    
    void Update()
    {
        if (!isSCActivated) // 자동스크린샷이 수행하지 않을 경우에만 활성화
        {
            // 마우스 우클릭시 작동
            //회전하고 싶은 축과 입력축이 반대인 것에 유의
            float yRot = Input.GetAxis("Mouse X") * Sensitivity; 
            float xRot = Input.GetAxis("Mouse Y") * Sensitivity;

            if (Input.GetMouseButton(1)) // 우클릭시 카메라 회전
                CameraRotate(yRot, xRot);
            
            // 카메라 이동 (wasd 이동)
            if (Input.GetKey(KeyCode.W))
                this.transform.Translate(Vector3.forward * CameraSpeed * Time.deltaTime); // 지정한 속도만큼 앞으로 이동
            if (Input.GetKey(KeyCode.S))
                this.transform.Translate(Vector3.back * CameraSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.A))
                this.transform.Translate(Vector3.left * CameraSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
                this.transform.Translate(Vector3.right * CameraSpeed * Time.deltaTime);


            if (Input.GetKey(KeyCode.LeftShift)) //왼쪽 shift할 시 더 빠르게 이동가능하게 속도에 점차적으로 설정한 BoosterSpeed를 더해준다
                CameraSpeed += BoosterSpeed;
            else if (Input.GetKeyUp(KeyCode.LeftShift)) // shift 땔 시 원래 값으로 되돌아감.
                CameraSpeed = 10.0f;

            // 줌인 아웃.
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
                this.transform.Translate(Vector3.back * ZoomSpeed);
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                this.transform.Translate(Vector3.forward * ZoomSpeed);
        }
    }


    // 카메라 마우스 회전
    void CameraRotate(float yRot,float xRot)
    {
            //오브젝트(기준이 되는 축을 유지해야 됨)와 카메라 회전을 분리해야 됨
            //쿼터니안은 곱해야 누적됨
            this.transform.localRotation *= Quaternion.Euler(0, yRot, 0);
            this.transform.localRotation *= Quaternion.Euler(-xRot, 0, 0);//부호 주의
    }
}
