using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 회전궤도 옵션으로 KG_RotShot 토글 UI를 클릭시에 수행을 조작하는 클래스
public class KG_RotShot : MonoBehaviour {

    public bool RotShotOn = false; // 현재 KG_RotShot Toggle이 켜져 있는 지 아닌 지를 알 수 있는 변수 ( 이것도 이상하게 바로 접근이 안되서 따로 토글.ison을 받아오는 간접적인 방법 선택)
    public GameObject RotShotToggle; // RotShot 토글 UI를 참조하는 변수 ( KG_RotShot 을 참조 )

    public void ChangeValue() // KG_RotShot 토글을 클릭할 때마다 수행되는 함수
    {
        if (RotShotToggle.GetComponent<Toggle>().isOn == true) // 클릭을 했을 때 만일 켜져 있었다면
        {
            RotShotOn = true; // 토글 현 상황 확인하는 변수를 그대로 넣어주고
            StartCoroutine(TurnOnText()); // TurnOnText 수행 ( 자세한 설명은 밑에서 )
        }
        else
            RotShotOn = false;
    }


    public Text RotText; // KG_RotShot 토글 UI가 켜졌을 경우에, "Q와E를 사용하여 조작하세요" 텍스트를 띄우기 위한 KG_RotShotText 참조 변수
    public GameObject Cube; // 회전 궤도를 시각적으로 나타내기 위해 "Player"의 자식으로 KG_VerticalRotateCube로써, "Player"의 상대적인 위치로 카메라 앞에 있는 큐브를 참조한 변수 ( 상대적인 위치는 직접 조절함 )
    IEnumerator TurnOnText() // 토글 UI 켜질 시, 텍스트를 깜박이게 하고, 큐브를 활성화하여 보이게 하는 코루틴 함수
    {
        float sec = 1.0f; // 깜박이는 시간을 조절하는 변수
        if (RotShotOn) // 만일 토글이 켜져있다면
        {
            Cube.gameObject.SetActive(true); // 회전 궤도를 보여주는 참조한 Cube를 활성화
            RotText.gameObject.SetActive(true); // "Q와E를 사용하여..." Text를 활성화

            while (true) // Text를 깜박이게 하는 부분 ( 색을 사용하여 효과를 표현, 감마 값을 켰다가 끄는 방식 )
            {
                // 만일 토글을 도중에 끄면 이 코루틴을 빠져나가서 기능을 멈춰야 하는 것을 검사하는 조건 ( 이것 또한 혹시 몰라 두개 다 한 것일 뿐, 사실 1개만 써도 상관없음)
                if (RotShotOn == false || RotShotToggle.GetComponent<Toggle>().isOn == false) break;
                
                RotText.color = new Color(255, 255, 255, 255); // Text의 색은 흰색으로 표현할 것이기에, rgb는 모두 255로 올리고, 투명도를 나타내는 감마값도 255로 설정
                yield return new WaitForSeconds(sec); // sec만큼의 초를 기다린다
                RotText.color = new Color(0, 0, 0, 0); // Text의 감마를 0으로 설정하므로 아예 투명하게 하여 안보이게 한다. rgb를 000으로 한 이유는, 그냥 혹시 몰라 한 것일 뿐 다른 의도는 없음.
                yield return new WaitForSeconds(sec); // 다시 sec 초를 기다린다
            }

            // KG_RayShot에서 자동스크린샷이 시작될 때에도 KG_RayShot 내부에서 토글을 끄기도 한다
            RotText.gameObject.SetActive(false); // Text를 원상태로 활성화를 끈다
            Cube.SetActive(false); // Cube도 활성화를 끄게 한다
        }
        yield return null; // 수행을 다 마치면 코루틴 종료
    }


    float cubeRspeed = 1.0f; // TurnOnText에서 Prefab으로 생성된, 회전 궤도를 시각적으로 보여주기 위한 직육면체 큐브, 객체의 회전 속도를 담당하는 변수
    public float RotAngle = 0; // 회전 궤도를 나타내는 Cube가 회전되는 만큼의 값을 그대로 누적하여, 나중에 자동스크린샷을 시작하려고 할때 참조당하게 하기 위해 선언한 변수 ( 참조당하게 해야하기때문에 public )
    void Update() // 만약 토글이 켜져있다면 매 프레임마다 Q와 E를 통해 회전을 시키게 하는 기능이 들어가 있다
    {
        if(RotShotOn) // 만일 KG_RotShot 토글이 켜져있는 상태라면
        {
            if (Input.GetKey(KeyCode.Q)) // 'Q'를 입력받는 대로
            {
                Cube.transform.Rotate(0, 0, cubeRspeed, Space.Self); // Cube를 본인 기준으로 회전한다. Space.World 기준으로 회전 시에는 다르게 동작... z축으로 회전시키는 이유는 별 다른 건 없다. z축으로 해야 고개를 기준으로 기우뚱하게 하는 축 회전이 가능하기에
                RotAngle += cubeRspeed; // cubeRspeed로 Cube를 회전시킨 만큼RotAngle에 누적시킨다.
            }
            else if (Input.GetKey(KeyCode.E)) // 'E'를 입력을 받을 대로
            {
                Cube.transform.Rotate(0, 0, -cubeRspeed, Space.Self);
                RotAngle -= cubeRspeed;
            } // 이렇게 할 경우에 Q와 E를 동시에 입력받지 못한다는 것을 의미. 동시에 받게 하려면 else if가 아닌 if로 써야한다
        }
    }    
}
