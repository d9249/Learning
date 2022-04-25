using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Object들의 Material을 흰색으로 바꾸고 원래대로 해주는 Class
public class KG_ShadowControl : MonoBehaviour {
   
    public Text KG_ShadowControlText; // Shadow Off 버튼을 누를 시, 버튼 속 Text를 변경하기 위해 참조
    public bool isShadowOn = true; // 현재 Shadow Off 버튼을 눌렀는 지 안눌렀는 지 알기 위해 선언

    // 건물의 Material을 바꾸려면 모든 Object의 MeshRenderer를 받아와야 한다
    private MeshRenderer[] Tile_Meshs; // 현재 존재하는 타일 매쉬를 받아오기 위한 변수
    private MeshRenderer[] Build_Meshs; // 현재 존재하는 건물 매쉬를 받아오기 위한 변수

    public Material KG_White; // 흰색 색상으로 만들어주기 위한 Material Prefab (KG_Prefab 폴더안에 있다)
    public Dictionary<string, Material> MatDic = new Dictionary<string, Material>(); // 원래의 색상을 저장하기 위한 Dictionary

    public void Shadow_Change()
    {
        Tile_Meshs = GameObject.Find("TileObject").GetComponentsInChildren<MeshRenderer>(); // Object를 찾아 그 자식 Objcet들의 MeshRenderer를 담음
        Build_Meshs = GameObject.Find("BuildObject").GetComponentsInChildren<MeshRenderer>();

        if (isShadowOn) // 현재 흰색 물체가 아니라면 즉 그림자가 켜져있는 원래의 상태라면
        {
            foreach(MeshRenderer mesh in Tile_Meshs) // 각 타일의 meshrenderer를 순회
            {
                if (!MatDic.ContainsKey(mesh.gameObject.name)) // 만일 Dictionary에 해당하는 이름이 없다면
                    MatDic.Add(mesh.gameObject.name, mesh.material); // 이름과 함께 material을 추가
                mesh.material = KG_White; // 흰색 물체로 만든다
            }
            foreach(MeshRenderer mesh in Build_Meshs) // 물체를 흰색으로 만들기 위한 코드 ( 위와 동일)
            {

                if (!MatDic.ContainsKey(mesh.gameObject.name))
                    MatDic.Add(mesh.gameObject.name, mesh.material);
                mesh.material = KG_White;
            }
            ShadowText_Change(); // 내부 Text 변경
        }
        else  // 현재 흰색 물체라면 ( 위 코드와 동일 )
        {
            foreach(MeshRenderer mesh in Tile_Meshs)
            {
                if (!MatDic.ContainsKey(mesh.gameObject.name))
                    MatDic.Add(mesh.gameObject.name, mesh.material);
                mesh.material = MatDic[mesh.gameObject.name]; // 원래 물체를 Dictionary에서 검색을 하여 다시 집어 넣는다.
            }
            foreach(MeshRenderer mesh in Build_Meshs)
            {
                if (!MatDic.ContainsKey(mesh.gameObject.name))
                    MatDic.Add(mesh.gameObject.name, mesh.material);
                mesh.material = MatDic[mesh.gameObject.name];
            }
            ShadowText_Change();
        }
    }
    
    
    private void ShadowText_Change() // Shadow Off 버튼 클릭 시, 내부 Text 변경함수
    {
            if (isShadowOn)
            {
                KG_ShadowControlText.text = "원본 이미지";
                isShadowOn = false;
            }
            else
            {
                KG_ShadowControlText.text = "마스크 씌우기";
                isShadowOn = true;
            }
    }


    public void Shadow_Texture_Change(string Texture_Type)
    {
        Tile_Meshs = GameObject.Find("TileObject").GetComponentsInChildren<MeshRenderer>(); // Object를 찾아 그 자식 Objcet들의 MeshRenderer를 담음
        Build_Meshs = GameObject.Find("BuildObject").GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in Tile_Meshs)
        {
            mesh.material.shader = Shader.Find(Texture_Type);
        }
        foreach (MeshRenderer mesh in Build_Meshs) // 물체를 흰색으로 만들기 위한 코드 ( 위와 동일)
        {
            mesh.material.shader = Shader.Find(Texture_Type);
        }
    }
        /*
        public GameObject Building_Text; // 짓는 동안 클릭 못하게 만드는 TEXT
        IEnumerator Building_Message()
        {
            Building_Text.SetActive(true);
            yield return new WaitForSeconds(1f);
            yield return null;
        }
        */



    }
