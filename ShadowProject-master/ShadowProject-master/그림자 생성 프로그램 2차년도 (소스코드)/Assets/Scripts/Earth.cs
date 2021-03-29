using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Earth : MonoBehaviour
{

    /// <summary>
    /// 요청지역의 지도레벨
    /// </summary>
    private int level = -1;

    /// <summary>
    /// 요청지역의 경도
    /// </summary>
    private float lon;

    /// <summary>
    /// 요청지역의 위도
    /// </summary>
    private float lat;

    /// <summary>
    /// 0레벨의 타일크기(각도)
    /// </summary>
    private const float TILE_SIZE_ZERO_LEVEL_DEG = 36;

    /// <summary>
    /// 타일 당 셀 개수(가로, 세로 동일)
    /// </summary>
    private const int CELL_COUNT = 64;

    /// <summary>
    /// 구글좌표계 지구반경
    /// </summary>
    private const float R = 6378137f;

    /// <summary>
    /// 지구의 중력
    /// </summary>
    public static float gravity = -9.8f;

    /// <summary>
    /// 플레이어의 지구중력 방향
    /// </summary>
    Vector3 playerGravityDir;

    /// <summary>
    /// 플레이어의 이동속도
    /// </summary>
    float moveSpeed = 10.0f;

    /// <summary>
    /// 최근 위치 좌표X
    /// </summary>
    private int lastPosX = -1;
    /// <summary>
    /// 최근 위치 좌표Y
    /// </summary>
    private int lastPosY = -1;
    /// <summary>
    /// 현재 위치 좌표x
    /// </summary>
    private int currPosX = -1;
    /// <summary>
    /// 현재 위치 좌표Y
    /// </summary>
    private int currPosY = -1;

    private Boolean isChangedTile = false;

    /// <summary>
    /// DEM 자료 요청 URL 구성
    /// </summary>
    /// <param name="level">레벨</param>
    /// <param name="col">타일 컬럼</param>
    /// <param name="row">타일 로우</param>
    /// <returns>요청 URL</returns>
    string GetDemReqUrl(int level, int col, int row)
    {
        // 행과 열의 인덱스 문자열을 구성
        var xIdx8Str = col > 9999 ?
            col.ToString().PadLeft(8, '0') : col.ToString().PadLeft(4, '0');
        var yIdx8Str = row > 9999 ?
            row.ToString().PadLeft(8, '0') : row.ToString().PadLeft(4, '0');

        // DEM 요청 URL 구성
        var urlParams = string.Format("{0}/{1}/{1}_{2}.bil", level, yIdx8Str, xIdx8Str);

        return @"http://xdworld.vworld.kr:8080/dem/" + urlParams;
    }

    /// <summary>
    /// Texture 다운로드 URL 구성
    /// </summary>
    /// <param name="level">레벨</param>
    /// <param name="col">타일 컬럼</param>
    /// <param name="row">타일 로우</param>
    /// <returns>요청 URL</returns>
    string GetTextureReqUrl(int level, int col, int row)
    {
        var xIdx8Str = col > 9999 ? col.ToString() : col.ToString().PadLeft(4, '0');
        var yIdx8Str = row > 9999 ? row.ToString() : row.ToString().PadLeft(4, '0');

        //(key 발급이 필요하며… 임시 테스트로 사용가능 81EC01D7 - 0327 - 3868 - B85D - 67E737396E44)
        var reqParams = string.Format("Layer=tile_mo_HD&Level={0}&IDX={1}&IDY={2}&APIKey=81EC01D7-0327-3868-B85D-67E737396E44",
            level, xIdx8Str, yIdx8Str);
        return @"http://xdworld.vworld.kr:8080/XDServer/requestLayerNode?" + reqParams;
    }

    /// <summary>
    /// 특정지역의 경위도 위치의 DEM과 Texture를 VWorld 서버로부터 수신하여 Tile을 생성
    /// </summary>
    /// <param name="level">tile 레벨</param>
    /// <param name="latitude">위도</param>
    /// <param name="longitude">경도</param>
    /// <param name="rangeCellCnt">경위도 반경의 셀범위 개수</param>
    /// <returns></returns>
    IEnumerator BuildArea(int level, float lat, float lon, int rangeCellCnt)
    {
        int tileCol = GetColIdx(level, lon);
        int tileRow = GetRowIdx(level, lat);

        int colMax = GetColCount(level);
        int rowMax = GetRowCount(level);
        if (tileCol + rangeCellCnt < colMax) colMax = tileCol + rangeCellCnt;
        if (tileRow + rangeCellCnt < rowMax) rowMax = tileRow + rangeCellCnt;

        int colMin = 0;
        int rowMin = 0;
        if (tileCol - rangeCellCnt > 0) colMin = tileCol - rangeCellCnt;
        if (tileRow - rangeCellCnt > 0) rowMin = tileRow - rangeCellCnt;

        for (int r = rowMin; r <= rowMax; r++)
        {
            for (int c = colMin; c <= colMax; c++)
            {
                yield return BuildTile(level, c, r);
            }
        }

        yield break;
    }

    /// <summary>
    /// DEM과 Texture를 VWorld 서버로부터 수신하여 Tile을 생성
    /// </summary>
    /// <param name="level">tile 레벨</param>
    /// <param name="tileCol">타일컬럼 인덱스</param>
    /// <param name="tileRow">타일로우 인덱스</param>
    /// <returns></returns>
    IEnumerator BuildTile(int level, int tileCol, int tileRow)
    {
        //Debug.Log("타일크기(m):" + GetTileSizeMeter(level));

        // 타일 ID 생성
        var tileId = string.Format("{0}_{1}_{2}", level, tileCol, tileRow);

        // DEM 요청
        var url = GetDemReqUrl(level, tileCol, tileRow);
        //Debug.Log("DEM 요청 시작 - " + url);
        var wwwDem = new WWW(url);
        yield return wwwDem;
        //Debug.Log("DEM 요청 완료 - " + url);

        byte[] demData = null;
        if (wwwDem.error == null && wwwDem.bytes != null)
            demData = wwwDem.bytes;
        //else Debug.Log("DEM 요청 오류 - " + url + wwwDem.error);

        // 게임오브젝트 생성
        GameObject gameObj = new GameObject(tileId);
        gameObj.transform.parent = this.gameObject.transform;

        gameObj.AddComponent<MeshFilter>();
        gameObj.AddComponent<MeshRenderer>();

        MeshCollider collider = gameObj.AddComponent<MeshCollider>();

        // 렌러더 설정
        MeshRenderer renderer = gameObj.GetComponent<MeshRenderer>();
        // 재질
        Material[] mats = this.GetComponent<Renderer>().materials;
        renderer.material = mats[0];
        // 메쉬 구성
        MeshFilter mf = gameObj.GetComponent<MeshFilter>();
        mf.mesh = BuildMesh(demData, level, tileCol, tileRow);

        // mesh collider 설정
        collider.sharedMesh = mf.mesh;
        collider.convex = true;

        // Texture 요청
        var textureUrl = GetTextureReqUrl(level, tileCol, tileRow);
        //Debug.Log("Texture 요청 시작 - " + textureUrl);
        UnityWebRequest wwwTexture = UnityWebRequestTexture.GetTexture(textureUrl);
        yield return wwwTexture.Send();
        //Debug.Log("Texture 요청 완료 - " + textureUrl);
        Texture texture = DownloadHandlerTexture.GetContent(wwwTexture);

        // Texture 설정
        if (wwwTexture.error == null)
        {
            renderer.material.mainTexture = texture;
            
            if (texture != null)
            {
                renderer.material.mainTexture.wrapMode = TextureWrapMode.Clamp;
            }
        }

        yield break;
    }

    /// <summary>
    /// DEM과 Texture를 VWorld 서버로부터 수신하여 Tile을 생성
    /// </summary>
    /// <param name="level">tile 레벨</param>
    /// <param name="tileCol">타일컬럼 인덱스</param>
    /// <param name="tileRow">타일로우 인덱스</param>
    /// <returns></returns>
    public GameObject Image_Option;
    public Material Mask_Material;
    IEnumerator BuildTile2(int level, int tileCol, int tileRow)
    {
        //Debug.Log("타일크기(m):" + GetTileSizeMeter(level));

        // 타일 ID 생성
        var tileId = string.Format("{0}_{1}_{2}", level, tileCol, tileRow);

        // DEM 요청
        var url = GetDemReqUrl(level, tileCol, tileRow);
        //Debug.Log("DEM 요청 시작 - " + url);
        var wwwDem = new WWW(url);
        yield return wwwDem;
        //Debug.Log("DEM 요청 완료 - " + url);

        byte[] demData = null;
        if (wwwDem.error == null && wwwDem.bytes != null)
            demData = wwwDem.bytes;
        //else Debug.Log("DEM 요청 오류 - " + url + wwwDem.error);

        // 게임오브젝트 생성
        GameObject gameObj = new GameObject(tileId);
        gameObj.transform.parent = this.gameObject.transform;
        gameObj.AddComponent<MeshFilter>();
        gameObj.AddComponent<MeshRenderer>();

        var meshCollider = gameObj.AddComponent<MeshCollider>();

        //var collider = gameObj.AddComponent<BoxCollider>();

        // 렌러더 설정
        MeshRenderer renderer = gameObj.GetComponent<MeshRenderer>();
        // 재질
        Material[] mats = this.GetComponent<Renderer>().materials;
        renderer.material = mats[0];
        


        // 메쉬 구성
        MeshFilter mf = gameObj.GetComponent<MeshFilter>();
        mf.mesh = BuildMesh2(level, tileCol, tileRow, demData, 64);


        meshCollider.convex = true;
        meshCollider.sharedMesh = mf.mesh;

        //meshCollider.constantForce.

        // Texture 요청
        var textureUrl = GetTextureReqUrl(level, tileCol, tileRow);
        //Debug.Log("Texture 요청 시작 - " + textureUrl);
        UnityWebRequest wwwTexture = UnityWebRequestTexture.GetTexture(textureUrl);
        yield return wwwTexture.Send();
        //Debug.Log("Texture 요청 완료 - " + textureUrl);
        Texture texture = DownloadHandlerTexture.GetContent(wwwTexture);

        // Texture 설정
        if (wwwTexture.error == null)
        {
            renderer.material.mainTexture = texture;
            
            if (texture != null)
            {
                renderer.material.mainTexture.wrapMode = TextureWrapMode.Clamp;
                image_option_script.MatDic.Add(gameObj.name, renderer.material);
            }
        }
        //KG_수정사항
        if(Image_Option.GetComponent<Dropdown>().value == 2)
            renderer.material =  Mask_Material;
        if (Image_Option.GetComponent<Dropdown>().value == 3)
            renderer.material.shader = Shader.Find("Unlit/Texture");

        yield break;
    }

    /// <summary>
    /// 레벨 별 타일의 크기
    /// 각도 단위
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns>타일크기(각도)</returns>
    float GetTileSizeDeg(int level)
    {
        return TILE_SIZE_ZERO_LEVEL_DEG / Mathf.Pow(2, level);
    }

    /// <summary>
    /// 레벨 별 타일의 크기
    /// 각도 단위
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns>타일크기(각도)</returns>
    float GetTileSizeRad(int level)
    {
        return GetTileSizeDeg(level) * Mathf.Deg2Rad;
    }

    /// <summary>
    /// 레벨 별 셀의 크기 (라디안 단위)
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns>셀크기(라디안)</returns>
    float GetCellSizeRad(int level)
    {
        return GetCellSizeDeg(level) * Mathf.Deg2Rad;
    }

    /// <summary>
    /// 레벨 별 셀의 크기 (도 단위)
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns>셀크기(각도)</returns>
    float GetCellSizeDeg(int level)
    {
        return GetTileSizeDeg(level) / CELL_COUNT;
    }

    /// <summary>
    /// 수신된 DEM과 Texture를 이용해 Mesh 생성
    /// </summary>
    /// <param name="demData">DEM 데이터</param>
    /// <param name="level">Tile 레벨</param>
    /// <param name="tileCol">Tile 컬럼 인덱스</param>
    /// <param name="tileRow">Tile 로우 인덱스</param>
    /// <returns>생성된 타일 매쉬</returns>
    Mesh BuildMesh(byte[] demData, int level, int tileCol, int tileRow)
    {
        var mesh = new Mesh();

        // 타일의 한 변 크기(도)
        float tileSizeDeg = this.GetTileSizeDeg(level);
        // 타일의 시작위도(라디안)
        float tileLatDeg = tileRow * tileSizeDeg - 90;
        // 타일의 시작경도(라디안)
        float tileLongDeg = tileCol * tileSizeDeg - 180;

        // 셀의 한 변 크기(라디안)
        float cellSizeDeg = GetCellSizeDeg(level);

        BinaryReader demReader = null;
        if (demData != null)
            demReader = new BinaryReader(new MemoryStream(demData));

        // vertex 목록
        List<Vector3> vertices = new List<Vector3>();
        // UV 배열
        List<Vector2> uvs = new List<Vector2>();
        // 삼각형 인덱스 목록
        List<int> triangles = new List<int>();
        // dem 값
        float dem = 0.0f;

        // 버텍스 생성
        for (int rowIdx = 0; rowIdx <= CELL_COUNT; rowIdx++)
        {
            // 셀 위도
            float lat = tileLatDeg + rowIdx * cellSizeDeg;
            float v = (float)rowIdx / CELL_COUNT;
            int VERTEXT_COUNT = CELL_COUNT + 1;

            if (demReader != null)
                demReader.BaseStream.Seek((VERTEXT_COUNT * (VERTEXT_COUNT - (rowIdx + 1))) * 4, SeekOrigin.Begin);
            for (int colIdx = 0; colIdx <= CELL_COUNT; colIdx++)
            {
                // 셀 경도
                float lon = tileLongDeg + colIdx * cellSizeDeg;
                // 고도 
                // Data에서 dem 값을 읽어옴
                if (demReader != null)
                    dem = demReader.ReadSingle();
                else
                    dem = 0.0f;
                // 버텍스 추가
                var vertex = GetSurfacePosDeg(lon, lat, dem);
                vertex = TranslateOrigin(origin, vertex);


                vertices.Add(vertex);
                float u = (float)colIdx / CELL_COUNT;
                // UV 인덱스 추가
                uvs.Add(new Vector2(u, v));

                //if (colIdx == 0 || colIdx == CELL_COUNT)
                //    Debug.Log(string.Format("{0}:({1}, {2}, {3}) \n", colIdx, vertex.x, vertex.y, vertex.z));
            }
        }

        if (demReader != null) demReader.Close();

        // 셀 삼각형 인덱스 생성
        for (int rowIdx = 0; rowIdx < CELL_COUNT; rowIdx++)
        {
            // 좌측 하단
            int idxLb = rowIdx * (CELL_COUNT + 1);
            // 좌측 상단
            int idxLt = idxLb + CELL_COUNT + 1;
            for (int colIdx = 0; colIdx < CELL_COUNT; colIdx++)
            {
                // 우측 하단
                int idxRb = idxLb + 1;
                // 우측 상단
                int idxRt = idxLt + 1;

                // 위쪽 삼각형
                triangles.Add(idxLb);
                triangles.Add(idxLt);
                triangles.Add(idxRt);
                // 아래쪽 삼각형
                triangles.Add(idxLb);
                triangles.Add(idxRt);
                triangles.Add(idxRb);

                idxLb++;
                idxLt++;
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    public Vector3 TranslateOrigin(Vector3 standardOrigin, Vector3 v)
    {
        v = v - standardOrigin;

        return v;
    }

    /// <summary>
    /// 수신된 DEM과 Texture를 이용해 Mesh 생성
    /// </summary>
    /// <param name="level">Tile 레벨</param>
    /// <param name="tileCol">Tile 컬럼 인덱스</param>
    /// <param name="tileRow">Tile 로우 인덱스</param>
    /// <param name="demData">타일의 DEM</param>
    /// <param name="cellCount">타일의 셀 개수</param>
    /// <returns>생성된 타일 매쉬</returns>
    Mesh BuildMesh2(int level, int tileCol, int tileRow, byte[] demData, int cellCount = 1)
    {
        var mesh = new Mesh();

        // 타일의 한 변 크기(도)
        float tileSizeDeg = this.GetTileSizeDeg(level);
        // 타일의 시작위도(라디안)
        float tileLatDeg = tileRow * tileSizeDeg - 90;
        // 타일의 시작경도(라디안)
        float tileLongDeg = tileCol * tileSizeDeg - 180;

        // 타일의 네 모서리 점 구하기
        // 좌측 하단
        var tileLeftBottom = GetSurfacePosDeg(tileLongDeg, tileLatDeg, 0f);
        // 우측 하단
        var tileRightBottom = GetSurfacePosDeg(tileLongDeg + tileSizeDeg, tileLatDeg, 0f);
        // 좌측 상단
        var tileLeftUp = GetSurfacePosDeg(tileLongDeg, tileLatDeg + tileSizeDeg, 0f);
        // 우측 상단
        var tileRightUp = GetSurfacePosDeg(tileLongDeg + tileSizeDeg, tileLatDeg + tileSizeDeg, 0f);

        // 첫타일의 좌측 하단 기준으로 원점을 지정
        if (origin == Vector3.zero)
        {
            float first_tileSizeDeg = this.GetTileSizeDeg(level);
            // 타일의 시작위도(라디안)
            float first_tileLatDeg = First_tileRow * tileSizeDeg - 90;
            // 타일의 시작경도(라디안)
            float first_tileLongDeg = First_tileCol * tileSizeDeg - 180;

            // 타일의 네 모서리 점 구하기
            // 좌측 하단
            var first_tileLeftBottom = GetSurfacePosDeg(first_tileLongDeg, first_tileLatDeg, 0f);

            origin = first_tileLeftBottom;

            Debug.Log("/////////////////////////////");
            Debug.Log("tileRow" + tileRow);
            Debug.Log("tileCol" + tileCol);
            Debug.Log("origin =" + origin);
            Debug.Log("/////////////////////////////");
        }
        // 타일의 원점이동이 적용된 네 모서리 점 구하기
        // 좌측 하단
        var orgTileLeftBottom = TranslateOrigin(origin, tileLeftBottom);
        // 우측 하단
        var orgTileRightBottom = TranslateOrigin(origin, tileRightBottom);
        // 좌측 상단
        var orgTileLeftUp = TranslateOrigin(origin, tileLeftUp);
        // 우측 상단
        var orgTileRightUp = TranslateOrigin(origin, tileRightUp);

        // 셀의 한 변 크기(라디안)
        float cellSizeDeg = GetCellSizeDeg(level) * CELL_COUNT / cellCount;
        //Debug.Log("셀 크기: " + cellSizeDeg + "도");

        // vertex 목록
        List<Vector3> vertices = new List<Vector3>();
        // UV 배열
        List<Vector2> uvs = new List<Vector2>();
        // 삼각형 인덱스 목록
        List<int> triangles = new List<int>();
        // dem 값
        float dem = 0.0f;

        BinaryReader demReader = null;
        if (demData != null)
            demReader = new BinaryReader(new MemoryStream(demData));

        // 버텍스 생성
        for (int rowIdx = 0; rowIdx <= cellCount; rowIdx++)
        {
            // 셀 위도
            float lat = tileLatDeg + rowIdx * cellSizeDeg;
            float v = (float)rowIdx / cellCount;
            int VERTEXT_COUNT = cellCount + 1;

            if (demReader != null)
                demReader.BaseStream.Seek((VERTEXT_COUNT * (VERTEXT_COUNT - (rowIdx + 1))) * 4, SeekOrigin.Begin);

            for (int colIdx = 0; colIdx <= cellCount; colIdx++)
            {
                // 셀 경도
                float lon = tileLongDeg + colIdx * cellSizeDeg;

                // 고도 
                // Data에서 dem 값을 읽어옴
                if (demReader != null)
                    dem = demReader.ReadSingle();
                else
                    dem = 0.0f;

                // 버텍스 추가
                var vertex = GetVertexPosOfTile(orgTileLeftBottom, orgTileRightBottom, orgTileLeftUp, orgTileRightUp,
                    colIdx, rowIdx, cellCount);

                // 높이 [높이방향 * dem]
                var upVector = GetVertexPosOfTile(tileLeftBottom, tileRightBottom, tileLeftUp, tileRightUp,
                    colIdx, rowIdx, cellCount).normalized * dem;

                // 기존 버텍스에서 높이를 적용
                vertex = vertex + upVector;

                vertices.Add(vertex);
                float u = (float)colIdx / cellCount;
                // UV 인덱스 추가
                uvs.Add(new Vector2(u, v));
            }
        }

        // 셀 삼각형 인덱스 생성
        for (int rowIdx = 0; rowIdx < cellCount; rowIdx++)
        {
            // 좌측 하단
            int idxLb = rowIdx * (cellCount + 1);
            // 좌측 상단
            int idxLt = idxLb + cellCount + 1;
            for (int colIdx = 0; colIdx < cellCount; colIdx++)
            {
                // 우측 하단
                int idxRb = idxLb + 1;
                // 우측 상단
                int idxRt = idxLt + 1;

                // 위쪽 삼각형
                triangles.Add(idxLb);
                triangles.Add(idxLt);
                triangles.Add(idxRt);
                // 아래쪽 삼각형
                triangles.Add(idxLb);
                triangles.Add(idxRt);
                triangles.Add(idxRb);

                idxLb++;
                idxLt++;
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        if (demReader != null) demReader.Close();

        return mesh;
    }

    /// <summary>
    /// 타일 내부의 버텍스 위치 구하기
    /// 타일을 직사각형으로 가정하여 구함
    /// </summary>
    /// <param name="leftBottom">타일의 좌측하단 모서리</param>
    /// <param name="rightBottom">타일의 우측하단 모서리</param>
    /// <param name="leftUp">타일의 좌측상단 모서리</param>
    /// <param name="rightUp">타일의 우측상단 모서리</param>
    /// <param name="col">타일의 버텍스 열 좌표</param>
    /// <param name="row">타일의 버텍스 행 좌표</param>
    /// <param name="cellCount">타일의 한변을 구성하는 셀 개수</param>
    /// <returns>타일 내부 점의 좌표</returns>
    Vector3 GetVertexPosOfTile(Vector3 leftBottom, Vector3 rightBottom, Vector3 leftUp, Vector3 rightUp,
        int col, int row, int cellCount)
    {
        var leftUnit = (leftUp - leftBottom).magnitude / cellCount;
        var rightUnit = (rightUp - rightBottom).magnitude / cellCount;

        // 좌측 출발점
        var left = leftBottom + (leftUp - leftBottom).normalized * row * leftUnit;
        // 우측 도착점
        var right = rightBottom + (rightUp - rightBottom).normalized * row * rightUnit;

        var colUnit = (right - left).magnitude / cellCount;

        var v = left + (right - left).normalized * col * colUnit;

        return v;
    }

    /// <summary>
    /// 각 레벨의 전체 타일컬럼 개수
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns>타일 컬럼 개수</returns>
    int GetColCount(int level)
    {
        return (int)Math.Pow(2, level) * 10;
    }

    /// <summary>
    /// 각 레벨의 전체 타일로우 개수
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns>타일 로우 개수</returns>
    int GetRowCount(int level)
    {
        return (int)Math.Pow(2, level) * 5;
    }

    /// <summary>
    /// 특정레벨의 Latitude에 해당하는 타일맵의 Row인덱스 반환
    /// </summary>
    /// <param name="level">요청레벨</param>
    /// <param name="lat">위도값</param>
    /// <returns>Row인덱스</returns>
    int GetRowIdx(int level, float lat)
    {
        float oneDegree = TILE_SIZE_ZERO_LEVEL_DEG;
        if (level > 0)
        {
            float lv2Pow = Mathf.Pow(2, level);
            oneDegree = TILE_SIZE_ZERO_LEVEL_DEG / lv2Pow;
        }
        int rIdx = (int)((lat + 90f) / oneDegree);

        return rIdx;
    }

    /// <summary>
    /// 특정레벨의 Longitude에 해당하는 타일맵의 Col인덱스 반환
    /// </summary>
    /// <param name="level">요청레벨</param>
    /// <param name="lon">위도값</param>
    /// <returns>Row인덱스</returns>
    int GetColIdx(int level, float lon)
    {
        float oneDegree = TILE_SIZE_ZERO_LEVEL_DEG;
        if (level > 0)
        {
            float lv2Pow = (float)Mathf.Pow(2, level);
            oneDegree = TILE_SIZE_ZERO_LEVEL_DEG / lv2Pow;
        }
        int cIdx = (int)((180f + lon) / oneDegree);

        return cIdx;
    }

    /* KG 수정사항
     * origin을 public 으로 변경하여 사용.
     */
    public Vector3 origin = Vector3.zero;
    

    /// <summary>
    /// 경위도 좌표를 직각좌표로 변환
    /// </summary>
    /// <param name="lonDeg">지구구체 경도</param>
    /// <param name="latDeg">지구구체 위도</param>
    /// <param name="height">지구구체 상 고도(해수면고)</param>
    /// <returns>변환된 직각좌표</returns>
    Vector3 GetSurfacePosDeg(float lonDeg, float latDeg, float height)
    {
        // Z축(위도) 및 Y축(경도)를 중심으로 회전(위도)
        var rotate = Quaternion.Euler(0f, -lonDeg, latDeg);
        // 자오선 및 적도 원점 벡터
        var v = rotate * new Vector3((R + height), 0f, 0f);

        return v;
    }

    /// <summary>
    /// 경위도 좌표를 직각좌표로 변환
    /// </summary>
    /// <param name="lon">지구구체 경도</param>
    /// <param name="lat">지구구체 위도</param>
    /// <param name="height">지구구체 상 고도(해수면고)</param>
    /// <returns>변환된 직각좌표</returns>
    Vector3 GetSurfacePosRad(float lon, float lat, float height)
    {
        return GetSurfacePosDeg(lon * Mathf.Rad2Deg, lat * Mathf.Rad2Deg, height);
    }

    /// <summary>
    /// 타일의 인덱스에 해당하는 DAT파일을 요청하여 
    /// </summary>
    /// <param name="wwwDat">Dat파일요청정보</param>
    /// <returns></returns>
    List<XDOInfo> getXDOInfo(WWW wwwDat)
    {   
        List<XDOInfo> fileInfos = null;


        BinaryReader datReader = new BinaryReader(new MemoryStream(wwwDat.bytes));
        datReader.BaseStream.Seek(0, SeekOrigin.Begin);

        // { 레벨, 경도ID, 위도ID, 객체의 총개수 }
        uint depth = datReader.ReadUInt32();
        if (depth == 15)
        {
            uint idx = datReader.ReadUInt32();
            uint idy = datReader.ReadUInt32();
            uint objCnt = datReader.ReadUInt32();

            fileInfos = new List<XDOInfo>();
            XDOInfo fileInfo = null;
            // 객체의 총 개수만큼 반복
            for (int i = 0; i < objCnt; i++)
            {
                fileInfo = new XDOInfo();

                // 버전
                fileInfo.version = datReader.ReadUInt32();

                // 타입
                fileInfo.type = datReader.ReadSByte();
                if (fileInfo.type == 8)
                {
                    // 객체의 키값 크기
                    fileInfo.keyLength = datReader.ReadSByte();
                    // 객체의 키값
                    fileInfo.key = System.Text.Encoding.UTF8.GetString(
                        datReader.ReadBytes(fileInfo.keyLength));
                    // 객체의 중심좌표 (경위도 radian)
                    fileInfo.centerPosX = datReader.ReadDouble();
                    // 객체의 중심좌표 (경위도 radian)
                    fileInfo.centerPosY = datReader.ReadDouble();
                    // 객체의 고도값
                    fileInfo.altitude = datReader.ReadSingle();
                    // 객체의 바운더리 Min X (구면 vectro)
                    fileInfo.boundMinX = datReader.ReadDouble();
                    // 객체의 바운더리 Min Y (구면 vectro)
                    fileInfo.boundMinY = datReader.ReadDouble();
                    // 객체의 바운더리 Min Z (구면 vectro)
                    fileInfo.boundMinZ = datReader.ReadDouble();
                    // 객체의 바운더리 Max X (구면 vectro)
                    fileInfo.boundMaxX = datReader.ReadDouble();
                    // 객체의 바운더리 Max Y (구면 vectro)
                    fileInfo.boundMaxY = datReader.ReadDouble();
                    // 객체의 바운더리 Max Z (구면 vectro)
                    fileInfo.boundMaxZ = datReader.ReadDouble();
                    // 이미지 레벨
                    fileInfo.imgLevel = datReader.ReadSByte();
                    // 객체의 파일이름 길이
                    fileInfo.dataFileLen = datReader.ReadSByte();
                    // 객체의 파일이름 (확장자포함)
                    fileInfo.dataFileName = System.Text.Encoding.UTF8.GetString(
                        datReader.ReadBytes(fileInfo.dataFileLen));
                    // 이미지 파일이름 길이
                    fileInfo.imgFileNameLen = datReader.ReadSByte();
                    // 이미지 파일이름
                    fileInfo.imgFileName = System.Text.Encoding.UTF8.GetString(
                        datReader.ReadBytes(fileInfo.imgFileNameLen));

                    fileInfos.Add(fileInfo);

                }

            }

        }

        datReader.Close();



        return fileInfos;
    }

    /// <summary>
    /// 특정지역의 경위도 위치의 건물(XDO)을 구성
    /// </summary>
    /// <param name="level">지도레벨</param>
    /// <param name="tileCol">타일컬럼</param>
    /// <param name="tileRow">타일로우</param>
    /// <returns></returns>
    IEnumerator BuildXDO(int level, int tileCol, int tileRow)
    {
        // 건물 불러오기
        // DAT 요청 URL 구성
        String xIdxDatStr = String.Concat(new String('0', 8 - tileCol.ToString().Length), tileCol.ToString());
        String yIdxDatStr = String.Concat(new String('0', 8 - tileRow.ToString().Length), tileRow.ToString());
        // http://dev1.mvsoftech.com
        // A40FA24F-DC95-3D1C-9944-287B4AEBDBC5

        // 81EC01D7-0327-3868-B85D-67E737396E44

        /*
        // 정상적으로 DAT를 가져오며 아래의 호출과 동일한 결과를 반환
        String reqDatUrlParam =
            String.Concat("Layer=facility_build&Level=15&IDX=", xIdxDatStr, "&IDY=", yIdxDatStr, "&APIKey=81EC01D7-0327-3868-B85D-67E737396E44");
        String datUrl = String.Concat(@"http://xdworld.vworld.kr:8080/XDServer/requestLayerNode?", reqDatUrlParam);
        */
        String reqDatUrlParam = String.Concat("15/", yIdxDatStr, "/", yIdxDatStr, "_", xIdxDatStr, ".dat");
        String datUrl = String.Concat(@"http://xdworld.vworld.kr:8080/real3d/facility_build/", reqDatUrlParam);

        var wwwDat = new WWW(datUrl);
        yield return wwwDat;

        // DAT를 요청하여 XDO의 정보목록을 가져옴.
        if (wwwDat.error == null)
        {
            List<XDOInfo> xdoInfos = DatReader.readDat(wwwDat);
            // XDO정보목록으로 XDO를 요청하여 생성함.
            yield return CreateXDO(xdoInfos, tileCol, tileRow);
        }
        yield break;

    }

    /// <summary>
    /// DAT에서 취득한 XDO정보 목록으로 XDO 구성
    /// </summary>
    /// <param name="xdoInfos">XDO정보목록</param>
    /// <param name="tileCol">타일컬럼</param>
    /// <param name="tileRow">타일로우</param>
    /// <returns></returns>
    IEnumerator CreateXDO(List<XDOInfo> xdoInfos, int tileCol, int tileRow)
    {
        
        if (xdoInfos != null)
        {
            XDOInfo xdoInfo = null;
            for (int i = 0; i < xdoInfos.Count; i++)
            {
                xdoInfo = xdoInfos[i];

                String xIdxDatStr = String.Concat(new String('0', 8 - tileCol.ToString().Length), tileCol.ToString());
                String yIdxDatStr = String.Concat(new String('0', 8 - tileRow.ToString().Length), tileRow.ToString());
                
                // XDO 파일을 요청함.
                String reqXdoUrlParam = String.Concat("15/", yIdxDatStr, "/", yIdxDatStr, "_", xIdxDatStr, "/", xdoInfo.dataFileName);
                String xdoUrl = String.Concat(@"http://xdworld.vworld.kr:8080/real3d/facility_build/", reqXdoUrlParam);
                //Debug.Log(xdoUrl);

                var wwwXdo = new WWW(xdoUrl);
                yield return wwwXdo;
                if (wwwXdo.error == null && wwwXdo.bytes != null)
                {
                    var reader = new System.IO.BinaryReader(new MemoryStream(wwwXdo.bytes));

                    var xdo = new XDO(reader, xdoInfo.version);
                    foreach (var xdoMesh in xdo.Meshes)
                    {
                        //Debug.Log(String.Format("Bound Min X:{0} Y:{1} Z:{2}", xdoInfo.boundMinX, xdoInfo.boundMinY, xdoInfo.boundMinZ));
                        //Debug.Log(String.Format("Bound Max X:{0} Y:{1} Z:{2}", xdoInfo.boundMaxX, xdoInfo.boundMaxY, xdoInfo.boundMaxZ));
                        Debug.Log(xdo.Key);
                        
                        if (xdo.Key == "111100000000000000490205") //15_279416_116122_
                        {
                            Debug.Log("True 490205");
                            continue;
                        }

                        var key = string.Format("{0}_{1}_{2}_{3}", level, tileCol, tileRow, xdo.Key);

                        GameObject bd = new GameObject(key);
                        bd.transform.parent = Build_Object.transform;

                        MeshCollider collider = bd.AddComponent<MeshCollider>();

                        MeshFilter xdoMf = bd.AddComponent<MeshFilter>();
                        // MeshFilter mf = GetComponent<MeshFilter>();
                        Mesh mesh = new Mesh();
                        xdoMf.mesh = mesh;

                        mesh.vertices = xdoMesh.Vertex.ToArray();
                        mesh.normals = xdoMesh.Normals.ToArray();

                        mesh.triangles = xdoMesh.Index.ToArray();
                        mesh.uv = xdoMesh.UV.ToArray();

                        mesh.RecalculateNormals();
                        mesh.RecalculateBounds();
                        // mesh collider 설정
                        collider.sharedMesh = mesh;
                        collider.convex = true;

                        MeshRenderer renderer = bd.AddComponent<MeshRenderer>();

                        // 0레벨 고해상도 이미지명으로 변환
                        //string hqImgNm = String.Concat(xdoMesh.ImageName.Split('.')[0], ".jpg");
                        http://xdworld.vworld.kr:8080/real3d/facility_build/15/00116046/00116046_00279452/b2015.jpg

                        // 고해상도 이미지 파일을 요청함.
                        String reqImgUrlParam = String.Concat("15/", yIdxDatStr, "/", yIdxDatStr, "_", xIdxDatStr, "/", xdoMesh.ImageName);
                        String imgUrl = String.Concat(@"http://xdworld.vworld.kr:8080/real3d/facility_build/", reqImgUrlParam);

                        UnityWebRequest wwwTexture = UnityWebRequestTexture.GetTexture(imgUrl);
                        yield return wwwTexture.Send();
                        //Debug.Log("Texture 요청 완료 - " + imgUrl);
                        //*****************************************
                        try
                        {
                            Texture texture = DownloadHandlerTexture.GetContent(wwwTexture);
                            // Texture 설정
                            if (wwwTexture.error == null)
                            {
                                renderer.material.mainTexture = texture;

                                if (texture != null)
                                {

                                    //**********************************************
                                    renderer.material.mainTexture.wrapMode = TextureWrapMode.Clamp;
                                    if (!image_option_script.MatDic.ContainsKey(bd.gameObject.name) )
                                        image_option_script.MatDic.Add(bd.gameObject.name, renderer.material);
                                }
                                else
                                {
                                    Destroy(bd); // 다운은 받았지만 텍스처가 없는 것들은 바로 제거
                                }
                            }

                            // 빌딩의 바운딩박스의 중심점높이만큼 빌딩객체의 높이를 적용. 
                            Vector3 vecMin = new Vector3((float)xdoInfo.boundMinX, (float)xdoInfo.boundMinY, (float)xdoInfo.boundMinZ);
                            Vector3 vecMax = new Vector3((float)xdoInfo.boundMaxX, (float)xdoInfo.boundMaxY, (float)xdoInfo.boundMaxZ);
                            Vector3 upVec = (vecMax / 2 - vecMin) - (new Vector3((float)xdoInfo.boundMaxX, (float)xdoInfo.boundMaxY, (float)xdoInfo.boundMinZ) / 2 - vecMin);

                            var bdPos = GetSurfacePosDeg((float)xdoInfo.centerPosX, (float)xdoInfo.centerPosY, (xdoInfo.altitude + upVec.magnitude));

                            // 속성정보를 얻기 위해 이름을 바꿈
                            bd.name = bd.name +"_"+ ((double)xdoInfo.centerPosX).ToString() +"_"+ ((double)xdoInfo.centerPosY).ToString();
                            bdPos = TranslateOrigin(origin, bdPos);

                            bd.transform.Translate(bdPos);

                            //Debug.Log("건물 이동 : " + bdPos);

                            bd.transform.Rotate(new Vector3(-90, 0, 0), Space.Self);
                            //KG_수정사항
                            if (Image_Option.GetComponent<Dropdown>().value == 2)
                                renderer.material = Mask_Material;
                            if (Image_Option.GetComponent<Dropdown>().value == 3)
                                renderer.material.shader = Shader.Find("Unlit/Texture");
                        }
                        catch
                        {
                            Debug.Log("Destroyed object with no texture");
                            Destroy(bd); // 텍스처 다운받는 과정에서 에러생긴 물체들은 제거
                        }


                       
                        
                    }

                }

            }
        }
        yield break;
    }

    /// <summary>
    /// 플레이어의 위치 지정
    /// </summary>
    /// <param name="lon">경도</param>
    /// <param name="lat">위도</param>
    /// <param name="height">플레이어의 위치 높이값</param>
    /// <returns></returns>
    IEnumerator setPlayer(float lon, float lat, float height)
    {
        while (origin == Vector3.zero) yield return null;

        var player = Player;
        var playerPos2 = GetSurfacePosDeg(lon, lat, 0f);
        var playerPos = GetSurfacePosDeg(lon, lat, height);
        var orgPlayerPos = TranslateOrigin(origin, playerPos);
        //Debug.Log("orgPlayerPos : " + orgPlayerPos);
        Vector3 standard_Playerpos = new Vector3((origin.x / 1000) * 1000, (origin.y / 1000) * 1000, (origin.z / 1000) * 1000); // 매번 달라지는 위치를 변경하기 위해 설정
        //orgPlayerPos = TranslateOrigin(origin, standard_Playerpos);
        orgPlayerPos = TranslateOrigin(origin, playerPos);
        // 기존은 playerPos
        //Debug.Log("orgPlayerPos : " + orgPlayerPos);
        //Debug.Log("Origin : " + origin);
        //Debug.Log("player position : " + player.transform.position);
        //Debug.Log("palyerPos2 : " + playerPos2);
        //Debug.Log("palyerPos : " + playerPos);
        //Debug.Log("standard_Playerpos : " + standard_Playerpos);
        player.transform.Translate(orgPlayerPos);
        //Debug.Log("player position : " + player.transform.position);

        player.transform.Rotate(new Vector3(-lon, -lat, 180f), Space.Self);

        int level = 15;
        int tileCol = GetColIdx(level, lon);
        int tileRow = GetRowIdx(level, lat);

        // 타일의 한 변 크기(도)
        float tileSizeDeg = this.GetTileSizeDeg(level);

        // 타일의 시작위도(라디안)
        float tileLatDeg = tileRow * tileSizeDeg - 90;
        // 타일의 시작경도(라디안)
        float tileLongDeg = tileCol * tileSizeDeg - 180;

        // 타일의 네 모서리 점 구하기
        // 좌측 하단
        playerGravityDir = playerPos.normalized;

        yield break;
    }

    /// <summary>
    /// 위도 단위 타일의 크기(미터 단위)
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns></returns>
    float GetTileSizeMeter(int level)
    {
        return R * GetTileSizeRad(level);
    }

    /// <summary>
    /// 플레이어의 키보드 컨트롤 설정 및 중력 적용
    /// </summary>
    public GameObject Main_Camera;
    void PlayerKeyboardControl()
    {
        var player = Player;
        var camera = Main_Camera;
        
        Transform cameraTransform = Camera.main.transform;
        Vector3 moveDirection;

        // 키보드 이동
        if (Input.GetAxis("USB Joystick y-axis") == -1 || Input.GetKey(KeyCode.W))
        {
            moveDirection = cameraTransform.forward;
            moveDirection.Normalize();

            moveDirection *= moveSpeed;

            player.GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);

        }

        if (Input.GetAxis("USB Joystick y-axis") == 1 || Input.GetKey(KeyCode.S))
        {
            moveDirection = cameraTransform.forward * -1.0f;
            moveDirection.Normalize();

            moveDirection *= moveSpeed;

            player.GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);

        }

        if (Input.GetAxis("USB Joystick x-axis") == -1 || Input.GetKey(KeyCode.A))
        {
            moveDirection = cameraTransform.right * -1.0f;
            moveDirection.Normalize();

            moveDirection *= moveSpeed;

            player.GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);

        }

        if (Input.GetAxis("USB Joystick x-axis") == 1 || Input.GetKey(KeyCode.D))
        {
            moveDirection = cameraTransform.right;
            moveDirection.Normalize();

            moveDirection *= moveSpeed;

            player.GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
        }

        /*
        // 플레이어를 중력을 적용함.
        if (!player.GetComponent<CharacterController>().isGrounded)
        {
            moveDirection = player.transform.up;
            moveDirection += playerGravityDir * gravity;
            moveDirection += player.transform.up * gravity;
            player.GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
        }
        */

        Boolean isNeckMove = false;
        if (Input.GetAxis("USB Joystick right y-axis") == -1)
        {
            isNeckMove = true;
        }
        if (Input.GetAxis("USB Joystick right y-axis") == 1)
        {
            isNeckMove = true;
        }

        if (Input.GetAxis("USB Joystick right x-axis") == -1)
        {
            isNeckMove = true;
        }
        if (Input.GetAxis("USB Joystick right x-axis") == 1)
        {
            isNeckMove = true;
        }
        if (isNeckMove)
        {
            isNeckMove = true;
            player.transform.Rotate(0f, Camera.main.transform.rotation.y + (5f * Input.GetAxis("USB Joystick right x-axis")), 0f, Space.Self);
        }

        // 시점 카메라 회전 기능
        /* KG_수정사항 
        if(Input.GetKey(KeyCode.Q)) //Left Rotate
        {
            camera.transform.Rotate(new Vector3(0, -moveSpeed * Time.deltaTime, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.E)) //Right Rotate
        {
            camera.transform.Rotate(new Vector3(0, moveSpeed * Time.deltaTime, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.R)) //UP Rotate
        {
            camera.transform.Rotate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.F)) //Down Rotate
        {
            camera.transform.Rotate(new Vector3(moveSpeed * Time.deltaTime, 0, 0), Space.Self);
        }

        */

    }

    /// <summary>
    /// 현재위치의 위도를 반환
    /// </summary>
    /// <param name="currPos"></param>
    /// <returns></returns>
    float GetLat(Vector3 currPos)
    {
        float lat = 0.0f;

        Vector3 pos = currPos + origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);

        lat = Vector3.Angle(projVec, pos);

        return lat;
    }

    /// <summary>
    /// 현재위치의 경도를 반환
    /// </summary>
    /// <param name="currPos"></param>
    /// <returns></returns>
    float GetLon(Vector3 currPos)
    {
        float lon = 0.0f;

        Vector3 pos = currPos + origin;
        Vector3 projVec = new Vector3(pos.x, 0f, pos.z);

        lat = Vector3.Angle(projVec, pos);

        lon = Vector3.Angle(new Vector3(1f, 0f, 0f), projVec);

        return lon;
    }

    /// <summary>
    /// 플레이어의 위치에 따른 맵 요청 체크
    /// </summary>
    public GameObject Player;
    void CheckMapRequest()
    {
        var player = Player;

        // xz평면에 투영되는 벡터를 구함.
        lat = GetLat(player.transform.position);
        lon = GetLon(player.transform.position);

        int c = GetColIdx(level, lon);
        int r = GetRowIdx(level, lat);

        currPosX = c;
        currPosY = r;

        Boolean changed = false;
        if (!lastPosX.Equals(currPosX))
        {
            lastPosX = currPosX;
            changed = true;
        }

        if (!lastPosY.Equals(currPosY))
        {
            lastPosY = currPosY;
            changed = true;
        }

        // 플레이어가 다른 타일로 넘어가는 경우
        if (changed)
        {
            // 인근 새 타일을 요청함
            //Debug.Log("변경됨");
            isChangedTile = true;

            StartCoroutine(RefleshData());
        }

    }


    /// <summary>
    /// 플레이어의 위치에 따라 지형 및 건물 정보를 갱신함.
    /// </summary>
    /// <returns></returns>
    public Image_Option image_option_script;
    public GameObject Tile_Object, Build_Object;
    IEnumerator RefleshData()
    {
        //while (true)
        //{
            yield return new WaitForSeconds(1f);

            int r = currPosY;
            int c = currPosX;

            if (isChangedTile)
            {
                List<string> readTileData = new List<string>();

            // 30으로 늘린 후에 더이상 생성하지 못하게 막아놓자. 왜냐면 그러면 타일을 너무 많이 생성함.
                for (int tileRow = r - 1; tileRow <= r + 1; tileRow++)
                {
                    for (int tileCol = c - 1; tileCol <= c + 1; tileCol++)
                    {
                        var key = string.Format("{0}_{1}_{2}", level, tileCol, tileRow);
                        readTileData.Add(key);
                    }
                }

                // 이동한 타일의 중심으로 주변 9개의 타일을 제외하고 모두 제거
                
                GameObject tilesParent = Tile_Object;
                GameObject tile = null;
                
            // 사전에 있는 데이터도 제거해야함
                for (int i = 0; i < tilesParent.transform.childCount; i++)
                {
                    tile = tilesParent.transform.GetChild(i).gameObject;
                    if (!readTileData.Contains(tile.name))
                    {
                    image_option_script.MatDic.Remove(tile.name);
                    Destroy(tile);
                    }
                }
                
                // 주변 9개의 타일중 새로운 타일을 생성
                foreach (string key in readTileData)
                {
                
                    var transform = tilesParent.transform.Find(key);
                    if (transform == null)
                    {
                        int pLevel = int.Parse(key.Split('_')[0]);
                        int pTileCol = int.Parse(key.Split('_')[1]);
                        int pTileRow = int.Parse(key.Split('_')[2]);
                        yield return StartCoroutine(BuildTile2(pLevel, pTileCol, pTileRow));

                    }
                }
                // 이동한 타일의 중심으로 주변 9개의 타일의 건물을 제외하고 모두 제거
                GameObject buildParent = Build_Object;
                GameObject build = null;
                
                for (int i = 0; i < buildParent.transform.childCount; i++)
                {
                    build = buildParent.transform.GetChild(i).gameObject;
                    string bdKey =
                        string.Format("{0}_{1}_{2}", build.name.Split('_')[0], build.name.Split('_')[1], build.name.Split('_')[2]);

                    if (!readTileData.Contains(bdKey))
                    {
                    image_option_script.MatDic.Remove(build.name);
                    Destroy(build);
                    }
                }
                

                // 주변 9개의 타일중 새로운 타일의 건물을 생성
                foreach (string key in readTileData)
                {
                    int pLevel = int.Parse(key.Split('_')[0]);
                    int pTileCol = int.Parse(key.Split('_')[1]);
                    int pTileRow = int.Parse(key.Split('_')[2]);
                    Boolean isExists = false;
                    for (int i = 0; i < buildParent.transform.childCount; i++)
                    {
                        GameObject buildObj = buildParent.transform.GetChild(i).gameObject;
                        string buildObjName =
                            string.Format("{0}_{1}_{2}", buildObj.name.Split('_')[0], buildObj.name.Split('_')[1], buildObj.name.Split('_')[2]);
                        if (key == buildObjName) {
                            isExists = true;
                            break;
                        }
                    }
                    if (!isExists)
                    {
                        yield return StartCoroutine(BuildXDO(pLevel, pTileCol, pTileRow));

                    }
                }

            isChangedTile = false;
            }
        //}
    }

    public GameObject Real_Sun; // same as Directional Light 
    public GameObject Sun_Option;
    public GameObject SkyCamera;
    // Update is called once per frame
    void Update()
    {
        /* KG 수정사항 
         * 스크린샷 활동중에는 맵요청을 안하게끔 함.
         * 왜냐면 카메라 이동중에 계속 이동을 하므로.
         */
        if (!Player.GetComponent<KG_PlayerMove>().isSCActivated)
        {

            // 플레이어의 키보드 컨트롤 설정 및 중력 적용
            //PlayerKeyboardControl();

            // 플레이어의 위치에 따른 맵 요청 체크
            CheckMapRequest();
        }

        // Skybox 카메라를 메인카메라의 회전으로 맞춤.
        var main = Camera.main;
        var sky = SkyCamera;
        //sky.transform.rotation = main.transform.localRotation;

        // 0 - no sun / 1 - real sun / 2 - virtual sun
        // setBasedata function have to be activated when it is real_sun
        int current_state = Sun_Option.GetComponent<Dropdown>().value;
        if (current_state == 1)
            // 남대현 _ 실시간 태양 위치 갱신
            setBaseData(Real_Sun); // 근사값 기반 그림자테스팅

        //DebugLog(); //표준 사이트 기반 결과값 비교 - 미완 (2018.08.13 기준)
    }

    // Use this for initialization
    void Start()
    {
        //GameObject.Find("SkyboxCamera").transform.Rotate(new Vector3(0f, lat, 0f), Space.World);
        if (!PlayerPrefs.HasKey("LATITUDE"))
        {
            // 광화문 37.576190, 126.976894
            lat = 37.576790f;
            lon = 126.976894f;

            // 서울 바클레이즈은행
            //lat = 37.566995f;
            //lon = 126.983562f;

            // 유성 온천1동 주민센터
            // 36.353018, 
            // 127.338378

            //lat = 37.57348f;
            //lon = 126.967792f;

            PlayerPrefs.SetFloat("LATITUDE", lat);
            PlayerPrefs.SetFloat("LONGITUDE", lon);
        }
        else
        {
            lat = PlayerPrefs.GetFloat("LATITUDE");
            lon = PlayerPrefs.GetFloat("LONGITUDE");
        }

        BuildFunc();
    }
    // Origin이 코루틴의 비동기적인 성질때문에 계속 변화하는 것을 막아주기 위함.
    public int First_tileRow = 0;
    public int First_tileCol = 0;
    public void BuildFunc()
    {
        level = 15;

        int c = GetColIdx(level, lon);
        int r = GetRowIdx(level, lat);
        lastPosX = c;
        lastPosY = r;
        currPosX = c;
        currPosY = r;
        First_tileRow = r;
        First_tileCol = c;

        for (int tileRow = r - 1; tileRow <= r + 1; tileRow++)
        {
            Debug.Log("tileRow : " + tileRow);
            for (int tileCol = c - 1; tileCol <= c + 1; tileCol++)
            {
                // 맵생성
                Debug.Log("tileCol : " + tileCol);
                StartCoroutine(BuildTile2(level, tileCol, tileRow));
            }
        }
        for (int tileRow = r - 1; tileRow <= r + 1; tileRow++)
        {
            for (int tileCol = c - 1; tileCol <= c + 1; tileCol++)
            {
                // 건물생성
                StartCoroutine(BuildXDO(level, tileCol, tileRow));
            }
        }
        
        // 플레이어 위치지정 ( 위경도, 높이)
        StartCoroutine(setPlayer(lon, lat, 50f));

        // 플레이어의 위치에 따른 지형 및 건물 데이터를 갱신함.
        StartCoroutine(RefleshData());

        // 남대현 - 초기 태양(Directional Light) 위치 지정 
        GameObject light = Real_Sun;
        light.transform.rotation = Quaternion.Euler(new Vector3(-(lon + 90), -lat, 180f)); //조명은 직각으로 내리비추도록 해야한다.
        // 임의 태양에도 이걸 적용하자.

    }

    //==============================================================================================================

    //적위/고도/방위각 수치 정의 --> 근사 오차가 발생함.
    public void setBaseData(GameObject light)
    {
        uint YEAR = UIManager.YEAR;
        uint MONTH = UIManager.MONTH;
        uint DAY = UIManager.DAY;
        uint HOUR = UIManager.HOUR;
        uint MINUTE = UIManager.MINUTE;
        
        float DECLINATION = calcDeclination((int)YEAR, (int)MONTH, (int)DAY);
        float ALTITUDE = calcAltitude(lat, DECLINATION, (int)HOUR, (int)MINUTE);
        float AZIMUTH = calcAzimuth(DECLINATION, ALTITUDE, (int)HOUR, (int)MINUTE);

        //Debug.Log(HOUR + "시 : 적위 " + DECLINATION + " 고도 " + ALTITUDE + " 방위각 " + AZIMUTH);
        if (Sun_Option.GetComponent<Dropdown>().value != 2)
        {
            //태양 위치 초기화
            light.transform.rotation = Quaternion.Euler(-lon, -lat, 180f);
            //초기화된 태양에 시간데이터를 입력받고 로컬값으로 회전
            light.transform.Rotate(new Vector3(-ALTITUDE + 180, AZIMUTH, 0), Space.Self);
        }
    }

    //적위 계산식
    float calcDeclination(int year, int month, int day)
    {
        //문서
        float temp = day + getDayCount(year, month) + 284;
        temp = temp * 360 / 365;
        temp = Mathf.Sin(temp * Mathf.Deg2Rad) * (23.45f * Mathf.Deg2Rad);
        return (temp * Mathf.Rad2Deg);
    }

    //고도 계산식
    float calcAltitude(float lat, float dec, int h, int m) //매개변수 : 위도, 적위, 시(Hour), 분(minute)
    {
        float temp = Mathf.Asin(Mathf.Sin(lat * Mathf.Deg2Rad)) * Mathf.Sin(dec * Mathf.Deg2Rad) + Mathf.Cos(lat * Mathf.Deg2Rad) * Mathf.Cos(dec * Mathf.Deg2Rad) * Mathf.Cos(getTime(h, m) * Mathf.Deg2Rad);

        return temp * Mathf.Rad2Deg;
    }

    //방위각 계산식
    float calcAzimuth(float dec, float alt, int h, int m) //매개변수 : 적위, 고도, 시, 분
    {
        /*
        //문서 
        //방위각이 점차 커지는 게 정상적인데 해당 알고리즘을 사용했을 때, 정오부근에서 방위각 결과값이 감소하는 현상이 발생합니다.
        float temp1 = Mathf.Cos(dec * Mathf.Deg2Rad) * Mathf.Sin(getTime(h, m) * Mathf.Deg2Rad);
        float temp2 = Mathf.Cos(alt * Mathf.Deg2Rad);
        float temp = Mathf.Asin(temp1 / temp2);
        return (temp * Mathf.Rad2Deg) % 360;
        */

        //한국천문연구원KASI 생활천문관
        float temp1 = Mathf.Sin(getTime(h, m) * Mathf.Deg2Rad);
        float temp21 = Mathf.Cos(getTime(h, m) * Mathf.Deg2Rad) * Mathf.Sin(lat * Mathf.Deg2Rad);
        float temp22 = Mathf.Tan(dec * Mathf.Deg2Rad) * Mathf.Cos(lat * Mathf.Deg2Rad);
        float temp2 = temp21 - temp22;
        //x2 = Mathf.Cos(deg2rad(topocen_local_hour_angle_deg)) * Mathf.Sin(deg2rad(obslat)) - Math.tan(deg2rad(topocen_sun_dec_deg)) * Mathf.Cos(deg2rad(obslat))

        float temp = Mathf.Atan2(temp1, temp2);
        return (temp * Mathf.Rad2Deg + 180) % 360;

    }

    //0시일 때 -180, 12시일 때 0, 24시일 때 180
    float getTime(int hour, int minute)
    {
        //minute * 15 / 60
        float temp = minute / 4;
        return -180 + (hour * 15) + temp;
    }

    int getDayCount(int year, int month)
    {
        int totalDay = 0;

        for (int i = 1; i < month; i++)
        {
            totalDay += System.DateTime.DaysInMonth(year, i);
        }
        
        return totalDay;
    }
    

    //================================================================================================
    //하단의 스크립트는 '한국천문연구원 천문우주지식정보(KASI)'에서 제공되는 알고리즘을 이용한 내용입니다.
    //각 시간대별로 상이한 결과값이 출력되야하는 것과 다르게
    //현재 이 코드를 통해서 진행하였을 때, 일부 시간구간별로 값이 동일하게 출력되는 현상이 발생하고 있습니다.

    /*
    public void DebugLog()
    {
        initData((int)YEAR, (int)MONTH, (int)DAY, (int)HOUR, (int)MINUTE, 0);
    }
    
    //var obslong = 126.9766844;
    //var obslat = 37.5773616;
    //var lochigh = 39;
    
    void initData(int y, int m, int d, int h, int i, int s)
    {
        var jd = date2jd(y, m + 1, d, h, i, s);
        var jde = get_jde(jd, 69);
        var jce = get_jce(jde);
        var jme = get_jme(jce);

        var sun_ra = get_geocentric_sun_ra(jd, 69);
        var sun_dec = get_geocentric_sun_dec(jd, 69);
        var sidtime_G = get_apparent_siderial_time(jd, 69); // degree
        var H = sidtime_G + lon - sun_ra;
        H = limit_deg(H, 360.0f);

        //Debug.Log(h+"시 jd " + jd + " ===> sun_ra " + sun_ra + " sun_dec " + sun_dec + " sidetime_G " + sidtime_G + " H " + H);

        var solar_info = get_topocentric_sun_ra_dec(jme, lat, 0, sun_ra, sun_dec, H);

        Debug.Log("앵글 - " + solar_info[2]  + " " + (h + 9) + "시 : 적위 " + solar_info[1] + " 고도 " + solar_info[3] + " 방위각 " + solar_info[4]);
    }

    double[] get_topocentric_sun_ra_dec(double jme, double obslat, double height, double sun_ra, double sun_dec, double H)
    {
        
        var equ_hori_parallax_of_the_sun = 8.794 / (3600.0 * get_earth_radius_vector_R(jme)); // degree
        //Debug.Log("=============" + equ_hori_parallax_of_the_sun);
        var term_u = Mathf.Atan(0.99664719f * Mathf.Tan((float)(obslat * Mathf.Deg2Rad))); // radian
        var term_x = Mathf.Cos(term_u) + height * Mathf.Cos((float)(obslat * Mathf.Deg2Rad)) / 6378140.0f;
        var term_y = 0.99664719 * Mathf.Sin(term_u) + height * Mathf.Sin((float)(obslat * Mathf.Deg2Rad)) / 6378140.0f;

        //적경
        var x = Mathf.Cos((float)(sun_dec * Mathf.Deg2Rad)) - term_x * Mathf.Sin((float)(equ_hori_parallax_of_the_sun * Mathf.Deg2Rad)) * Mathf.Cos((float)H * Mathf.Deg2Rad);
        var y = -1.0f * term_x * Mathf.Sin((float)(equ_hori_parallax_of_the_sun * Mathf.Deg2Rad)) * Mathf.Sin((float)H * Mathf.Deg2Rad);
        var parallax_sun_ra_rad = Mathf.Atan2((float)y, (float)x); // radian
        var parallax_sun_ra_deg = parallax_sun_ra_rad * Mathf.Rad2Deg; // degree
        
        var topocen_sun_ra_deg = sun_ra + parallax_sun_ra_deg; // degree
        
        //적위
        var x1 = Mathf.Cos((float)(sun_dec * Mathf.Deg2Rad)) - term_y * Mathf.Sin((float)(equ_hori_parallax_of_the_sun * Mathf.Deg2Rad)) * Mathf.Cos((float)H * Mathf.Deg2Rad);
        var y1 = ( Mathf.Sin((float)(sun_dec * Mathf.Deg2Rad)) - term_y * Mathf.Sin((float)(equ_hori_parallax_of_the_sun * Mathf.Deg2Rad)) ) * Mathf.Cos((float)(parallax_sun_ra_deg * Mathf.Deg2Rad));
        var topocen_sun_dec_rad = Math.Atan2(y1, x1);
        var topocen_sun_dec_deg = topocen_sun_dec_rad * Mathf.Rad2Deg; // degree

        
        var topocen_local_hour_angle_deg = H - parallax_sun_ra_deg; // degree
        //고도
        var topocen_elev_angle_rad = Math.Asin(Mathf.Sin((float)obslat * Mathf.Deg2Rad) * Mathf.Sin((float)(topocen_sun_dec_deg * Mathf.Deg2Rad)) + Mathf.Cos((float)obslat * Mathf.Deg2Rad) * Mathf.Cos((float)(topocen_sun_dec_deg * Mathf.Deg2Rad)) * Mathf.Cos((float)topocen_local_hour_angle_deg * Mathf.Deg2Rad)); // radian
        var topocen_elev_angle_deg = topocen_elev_angle_rad * Mathf.Rad2Deg; // degree

        //방위각
        var x2 = Mathf.Cos((float)topocen_local_hour_angle_deg * Mathf.Deg2Rad) * Mathf.Sin((float)obslat * Mathf.Deg2Rad) - Mathf.Tan((float)(topocen_sun_dec_deg * Mathf.Deg2Rad)) * Mathf.Cos((float)obslat * Mathf.Deg2Rad);
        var y2 = Mathf.Sin((float)topocen_local_hour_angle_deg * Mathf.Deg2Rad);

        var topocen_azi_angle_rad = Mathf.Atan2(y2, x2); // radian
        var topocen_azi_angle_deg = topocen_azi_angle_rad * Mathf.Rad2Deg + 180.0; // degree

        double[] result = { topocen_sun_ra_deg, topocen_sun_dec_deg, topocen_local_hour_angle_deg, topocen_elev_angle_deg, topocen_azi_angle_deg };

        return result;
     }


    double date2jd(int yearj, int monthj, int dayj, int hj, int mj, int sj)
    {
        int year1;
        int month1;

        if (monthj == 1 || monthj == 2)
        {
            year1 = yearj - 1;
            month1 = monthj + 12;
        }
        else
        {
            year1 = yearj;
            month1 = monthj;
        }
        var misc1 = (int)(365.25 * (year1 + 4716));
        var misc2 = (int)(30.6001 * (month1 + 1));
        var d = dayj + (hj + mj / 60.0 + sj / 3600.0) / 24.0;
        var a = (int)(year1 / 100);
        int b;

        if (yearj <= 1582 && monthj <= 10 && dayj <= 15)
        { // Julian Calendar
            b = 0;
        }
        else
        { // Gregorian Calendar
            b = 2 - a + (int)(a / 4);
        }

        return misc1 + misc2 + d + b - 1524.5;
    }

    double get_jde(double jd, double delta_t)
    { //67(default) : 원래 공식대로라면 delta_t.js 라고 연도에 따른 별도의 테이블을 이용해야한다.
        return jd + delta_t / 86400.0;
    }

    double get_jc(double jd)
    {
        return (jd - 2451545.0) / 36525.0;
    }

    double get_jce(double jde)
    {
        return (jde - 2451545.0) / 36525.0;
    }

    double get_jme(double jce)
    {
        return jce / 10.0;
    }
    
    double get_apparent_siderial_time(double jd, double delta_t)
    {
        var jc = get_jc(jd);
        //var jce = get_jce(get_jde(jd, delta_t));
        var jce = get_jce(jd);
        
        var mean_sidtime = 280.46061837 + 360.98564736629 * (jd - 2451545.0) + 0.000387933 * jc * jc - jc * jc * jc / 38710000.0; // degree
        mean_sidtime = limit_deg(mean_sidtime, 360.0);

        var nut_long_and_obliq = get_nutation_in_longitude_and_obliquity(jce);

        //Debug.Log("jc " + jc + " jce " + jce + " mean_sidtime " + mean_sidtime + " nut_long_and_obliq[0] " + nut_long_and_obliq[0] + " get_true_obliquity_of_the_ecliptic(jce) " + get_true_obliquity_of_the_ecliptic(jce));

        return (float)(mean_sidtime + nut_long_and_obliq[0] * Mathf.Cos((float)(get_true_obliquity_of_the_ecliptic(jce) * Mathf.Deg2Rad))); // degree
    }

    double get_geocentric_sun_ra(double jd, double delta_t)
    {
        var jde = get_jde(jd, delta_t);
        var jce = get_jce(jde);
        var jme = get_jme(jce);

        var x = Mathf.Cos((float)get_apparent_sun_longitude(jce) * Mathf.Deg2Rad);
        var y = Mathf.Sin((float)get_apparent_sun_longitude(jce) * Mathf.Deg2Rad)
                * Mathf.Cos((float)get_true_obliquity_of_the_ecliptic(jce) * Mathf.Deg2Rad)
                - Mathf.Tan(-1.0f * (float)get_earth_heliocentric_latitude_B((float)jme) * Mathf.Deg2Rad)
                * Mathf.Sin((float)get_true_obliquity_of_the_ecliptic(jce) * Mathf.Deg2Rad);

        var sun_ra_rad = Mathf.Atan2(y, x);
        var sun_ra_deg = sun_ra_rad * Mathf.Rad2Deg;

        return limit_deg(sun_ra_deg, 360.0); // degree
    }

    double get_geocentric_sun_dec(double jd, double delta_t)
    {
        var jde = get_jde(jd, delta_t);
        var jce = get_jce(jde);
        var jme = get_jme(jce);

        var a = Mathf.Sin(-1.0f * (float)get_earth_heliocentric_latitude_B((float)jme) * Mathf.Deg2Rad) * Mathf.Cos((float)get_true_obliquity_of_the_ecliptic(jce) * Mathf.Deg2Rad);
        var b = Mathf.Cos(-1.0f * (float)get_earth_heliocentric_latitude_B((float)jme) * Mathf.Deg2Rad) * Mathf.Sin((float)get_true_obliquity_of_the_ecliptic(jce) * Mathf.Deg2Rad) * Mathf.Sin((float)get_apparent_sun_longitude(jce) * Mathf.Deg2Rad);

        return Mathf.Asin(a + b) * Mathf.Rad2Deg; // degree
    }

    double limit_deg(double deg, double deno)
    {
        double deg2 = deg / deno;
        var limited = deno * (deg - Math.Truncate(deg2));
        if (limited < 0.0f)
        {
            limited = limited + deno;
        }
        return limited;
    }

    double[] get_nutation_in_longitude_and_obliquity(double jce)
    {
        double[] X = new double[5];

        // degree
        X[0] = 297.85036 + 445267.111480 * jce - 0.0019142 * jce * jce + jce * jce * jce / 189474.0;
        X[1] = 357.52772 + 35999.050340 * jce - 0.0001603 * jce * jce - jce * jce * jce / 300000.0;
        X[2] = 134.96298 + 477198.867398 * jce + 0.0086972 * jce * jce + jce * jce * jce / 56250.0;
        X[3] = 93.27191 + 483202.017538 * jce - 0.0036825 * jce * jce + jce * jce * jce / 327270.0;
        X[4] = 125.04452 - 1934.13626 * jce + 0.0020708 * jce * jce + jce * jce * jce / 450000.0;

        int nY = 63;
        int nPE = 63;

        double sum_nut_in_long = 0.0;
        double sum_nut_in_obliq = 0.0;
        double sum_temp_deg = 0.0;

        for (int i = 0; i < nY; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                sum_temp_deg = sum_temp_deg + X[j] * Y_TERMS[i, j];
            }
            sum_nut_in_long = sum_nut_in_long + (PE_TERMS[i, 0] + PE_TERMS[i, 1] * jce) * Mathf.Sin((float)sum_temp_deg * Mathf.Deg2Rad);
            sum_nut_in_obliq = sum_nut_in_obliq + (PE_TERMS[i, 2] + PE_TERMS[i, 3] * jce) * Mathf.Cos((float)sum_temp_deg * Mathf.Deg2Rad);
        }

        double nut_in_longitude = sum_nut_in_long / 36000000.0;
        double nut_in_obliquity = sum_nut_in_obliq / 36000000.0;

        double[] result = new double[2];
        result[0] = nut_in_longitude; //degree
        result[1] = nut_in_obliquity; //degree

        return result;
    }

    double[,] Y_TERMS = {
    {0, 0, 0, 0, 1 },
    { -2, 0, 0, 2, 2},
    {0, 0, 0, 2, 2},
    {0, 0, 0, 0, 2},
    {0, 1, 0, 0, 0},
    {0, 0, 1, 0, 0},
    {-2, 1, 0, 2, 2},
    {0, 0, 0, 2, 1},
    {0, 0, 1, 2, 2},
    {-2, -1, 0, 2, 2},
    {-2, 0, 1, 0, 0},
    {-2, 0, 0, 2, 1},
    {0, 0, -1, 2, 2},
    {2, 0, 0, 0, 0},
    {0, 0, 1, 0, 1},
    {2, 0, -1, 2, 2},
    {0, 0, -1, 0, 1},
    {0, 0, 1, 2, 1},
    {-2, 0, 2, 0, 0},
    {0, 0, -2, 2, 1},
    {2, 0, 0, 2, 2},
    {0, 0, 2, 2, 2},
    {0, 0, 2, 0, 0},
    {-2, 0, 1, 2, 2},
    {0, 0, 0, 2, 0},
    {-2, 0, 0, 2, 0},
    {0, 0, -1, 2, 1},
    {0, 2, 0, 0, 0},
    {2, 0, -1, 0, 1},
    {-2, 2, 0, 2, 2},
    {0, 1, 0, 0, 1},
    {-2, 0, 1, 0, 1},
    {0, -1, 0, 0, 1},
    {0, 0, 2, -2, 0},
    {2, 0, -1, 2, 1},
    {2, 0, 1, 2, 2},
    {0, 1, 0, 2, 2},
    {-2, 1, 1, 0, 0},
    {0, -1, 0, 2, 2},
    {2, 0, 0, 2, 1},
    {2, 0, 1, 0, 0},
    {-2, 0, 2, 2, 2},
    {-2, 0, 1, 2, 1},
    {2, 0, -2, 0, 1},
    {2, 0, 0, 0, 1},
    {0, -1, 1, 0, 0},
    {-2, -1, 0, 2, 1},
    {-2, 0, 0, 0, 1},
    {0, 0, 2, 2, 1},
    {-2, 0, 2, 0, 1},
    {-2, 1, 0, 2, 1},
    {0, 0, 1, -2, 0},
    {-1, 0, 1, 0, 0},
    {-2, 1, 0, 0, 0},
    {1, 0, 0, 0, 0},
    {0, 0, 1, 2, 0},
    {0, 0, -2, 2, 2},
    {-1, -1, 1, 0, 0},
    {0, 1, 1, 0, 0},
    {0, -1, 1, 2, 2},
    {2, -1, -1, 2, 2},
    {0, 0, 3, 2, 2},
    {2, -1, 0, 2, 2}
    };
    double[,] PE_TERMS =
        {

        { -171996, -174.2, 92025, 8.9},
        { -13187, -1.6, 5736, -3.1},
        { -2274, -0.2, 977, -0.5},
        { 2062, 0.2, -895, 0.5},
        { 1426, -3.4, 54, -0.1},
        { 712, 0.1, -7, 0},
        { -517, 1.2, 224, -0.6},
        { -386, -0.4, 200, 0},
        { -301, 0, 129, -0.1},
        { 217, -0.5, -95, 0.3},
        { -158, 0, 0, 0},
        { 129, 0.1, -70, 0},
        { 123, 0, -53, 0},
        { 63, 0, 0, 0},
        { 63, 0.1, -33, 0},
        { -59, 0, 26, 0},
        { -58, -0.1, 32, 0},
        { -51, 0, 27, 0},
        { 48, 0, 0, 0},
        { 46, 0, -24, 0},
        { -38, 0, 16, 0},
        { -31, 0, 13, 0},
        { 29, 0, 0, 0},
        { 29, 0, -12, 0},
        { 26, 0, 0, 0},
        { -22, 0, 0, 0},
        { 21, 0, -10, 0},
        { 17, -0.1, 0, 0},
        { 16, 0, -8, 0},
        { -16, 0.1, 7, 0 },
        { -15, 0, 9, 0 },
        { -13, 0, 7, 0 },
        { -12, 0, 6, 0 },
        { 11, 0, 0, 0 },
        { -10, 0, 5, 0 },
        { -8, 0, 3, 0 },
        { 7, 0, -3, 0 },
        { -7, 0, 0, 0 },
        { -7, 0, 3, 0 },
        { -7, 0, 3, 0 },
        { 6, 0, 0, 0 },
        { 6, 0, -3, 0 },
        { 6, 0, -3, 0 },
        { -6, 0, 3, 0 },
        { -6, 0, 3, 0 },
        { 5, 0, 0, 0 },
        { -5, 0, 3, 0 },
        { -5, 0, 3, 0 },
        { -5, 0, 3, 0},
        { 4, 0, 0, 0 },
        { 4, 0, 0, 0 },
        { 4, 0, 0, 0 },
        { -4, 0, 0, 0 },
        { -4, 0, 0, 0 },
        { -4, 0, 0, 0 },
        { 3, 0, 0, 0 },
        { -3, 0, 0, 0 },
        { -3, 0, 0, 0 },
        { -3, 0, 0, 0 },
        { -3, 0, 0, 0 },
        { -3, 0, 0, 0 },
        { -3, 0, 0, 0 },
        { -3, 0, 0, 0 }
        };

    double get_true_obliquity_of_the_ecliptic(double jce)
    {
        var jme = get_jme(jce);
        var u = jme / 10.0;
        var nut_long_and_obliq = get_nutation_in_longitude_and_obliquity(jce);

        double mean_obliquity = 84381.448 - 4680.93 * u - 1.55 * Math.Pow(u, 2) + 1999.25 * Math.Pow(u, 3) - 51.38 * Math.Pow(u, 4) - 249.67 * Math.Pow(u, 5) - 39.05 * Math.Pow(u, 6) + 7.12 * Math.Pow(u, 7) + 27.87 * Math.Pow(u, 8) + 5.79 * Math.Pow(u, 9) + 2.45 * Math.Pow(u, 10);

        return mean_obliquity / 3600.0 + nut_long_and_obliq[1]; // degree
    }

    double get_apparent_sun_longitude(double jce)
    {
        var jme = get_jme(jce);

        var a = get_earth_heliocentric_longitude_L(jme) + 180.0;
        var b = get_nutation_in_longitude_and_obliquity(jce);
        var c = get_abrration_correction_delta_tau(jme);

        return a + b[0] + c; // degree
    }

    double get_earth_heliocentric_longitude_L(double jme)
    {
        var nL_0 = 64;
        var nL_1 = 34;
        var nL_2 = 20;
        var nL_3 = 7;
        var nL_4 = 3;
        var nL_5 = 1;

        var L_sum_0 = 0.0;
        var L_sum_1 = 0.0;
        var L_sum_2 = 0.0;
        var L_sum_3 = 0.0;
        var L_sum_4 = 0.0;
        var L_sum_5 = 0.0;

        int i = 0;
        for (i = 0; i < nL_0; i++)
        {
            L_sum_0 = L_sum_0 + L_TERMS_0[i, 0] * Mathf.Cos((float)(L_TERMS_0[i, 1] + L_TERMS_0[i, 2] * jme));
        }
        for (i = 0; i < nL_1; i++)
        {
            L_sum_1 = L_sum_1 + L_TERMS_1[i, 0] * Mathf.Cos((float)(L_TERMS_1[i, 1] + L_TERMS_1[i, 2] * jme));
        }
        for (i = 0; i < nL_2; i++)
        {
            L_sum_2 = L_sum_2 + L_TERMS_2[i, 0] * Mathf.Cos((float)(L_TERMS_2[i, 1] + L_TERMS_2[i, 2] * jme));
        }
        for (i = 0; i < nL_3; i++)
        {
            L_sum_3 = L_sum_3 + L_TERMS_3[i, 0] * Mathf.Cos((float)(L_TERMS_3[i, 1] + L_TERMS_3[i, 2] * jme));
        }
        for (i = 0; i < nL_4; i++)
        {
            L_sum_4 = L_sum_4 + L_TERMS_4[i, 0] * Mathf.Cos((float)(L_TERMS_4[i, 1] + L_TERMS_4[i, 2] * jme));
        }
        for (i = 0; i < nL_5; i++)
        {
            L_sum_5 = L_sum_5 + L_TERMS_5[i, 0] * Mathf.Cos((float)(L_TERMS_5[i, 1] + L_TERMS_5[i, 2] * jme));
        }
        
        L_sum_0 = Math.Truncate(L_sum_0 * 1000000) / 1000000;
        L_sum_1 = Math.Truncate(L_sum_1 * 1000000) / 1000000;
        L_sum_2 = Math.Truncate(L_sum_2 * 1000000) / 1000000;
        L_sum_3 = Math.Truncate(L_sum_3 * 1000000) / 1000000;
        L_sum_4 = Math.Truncate(L_sum_4 * 1000000) / 1000000;
        L_sum_5 = Math.Truncate(L_sum_5 * 1000000) / 1000000;

        double L_rad = (L_sum_0 + L_sum_1 * jme + L_sum_2 * jme * jme + L_sum_3 * jme * jme * jme + L_sum_4 * jme * jme * jme * jme + L_sum_5 * jme * jme * jme * jme * jme) / 100000000.0;
        double L_deg = L_rad * Mathf.Rad2Deg;
        L_deg = limit_deg((float)L_deg, 360.0f);

        return L_deg; // degree
    }


    double get_earth_heliocentric_latitude_B(float jme)
    {
        var nB_0 = 5;
        var nB_1 = 2;

        var B_sum_0 = 0.0;
        var B_sum_1 = 0.0;

        int i;
        for (i = 0; i < nB_0; i++)
        {
            B_sum_0 = B_sum_0 + B_TERMS_0[i,0] * Mathf.Cos((float)(B_TERMS_0[i,1] + B_TERMS_0[i,2] * jme));
        }
        for (i = 0; i < nB_1; i++)
        {
            B_sum_1 = B_sum_1 + B_TERMS_1[i,0] * Mathf.Cos((float)(B_TERMS_1[i,1] + B_TERMS_1[i,2] * jme));
        }

        //B_sum_0 = Math.Round(B_sum_0, 6) / 1;
        //B_sum_1 = Math.Round(B_sum_1, 6) / 1;
        B_sum_0 = Math.Truncate(B_sum_0 * 1000000) / 1000000;
        B_sum_1 = Math.Truncate(B_sum_1 * 1000000) / 1000000;

        var B_deg = (B_sum_0 + B_sum_1 * jme) / 100000000.0;

        return limit_deg(B_deg, 360.0); // degree
    }

    double get_earth_radius_vector_R(double jme)
    {
        var nR_0 = 40;
        var nR_1 = 10;
        var nR_2 = 6;
        var nR_3 = 2;
        var nR_4 = 1;

        var R_sum_0 = 0.0;
        var R_sum_1 = 0.0;
        var R_sum_2 = 0.0;
        var R_sum_3 = 0.0;
        var R_sum_4 = 0.0;

        int i;
        for (i = 0; i < nR_0; i++)
        {
            R_sum_0 = R_sum_0 + R_TERMS_0[i, 0] * Mathf.Cos((float)(R_TERMS_0[i, 1] + R_TERMS_0[i, 2] * jme));
        }
        for (i = 0; i < nR_1; i++)
        {
            R_sum_1 = R_sum_1 + R_TERMS_1[i, 0] * Mathf.Cos((float)(R_TERMS_1[i, 1] + R_TERMS_1[i, 2] * jme));
        }
        for (i = 0; i < nR_2; i++)
        {
            R_sum_2 = R_sum_2 + R_TERMS_2[i, 0] * Mathf.Cos((float)(R_TERMS_2[i, 1] + R_TERMS_2[i, 2] * jme));
        }
        for (i = 0; i < nR_3; i++)
        {
            R_sum_3 = R_sum_3 + R_TERMS_3[i, 0] * Mathf.Cos((float)(R_TERMS_3[i, 1] + R_TERMS_3[i, 2] * jme));
        }
        for (i = 0; i < nR_4; i++)
        {
            R_sum_4 = R_sum_4 + R_TERMS_4[i, 0] * Mathf.Cos((float)(R_TERMS_4[i, 1] + R_TERMS_4[i, 2] * jme));
        }

        double R = (R_sum_0 + R_sum_1 * jme + R_sum_2 * jme * jme + R_sum_3 * jme * jme * jme + R_sum_4 * jme * jme * jme * jme) / 100000000.0;

        return R; // au
    }

    double[,] L_TERMS_0 =
        {
    { 175347046.0, 0, 0 },
    { 3341656.0, 4.6692568, 6283.07585 },
    { 34894.0, 4.6261, 12566.1517 },
    { 3497.0, 2.7441, 5753.3849 },
    { 3418.0, 2.8289, 3.5231 },
    { 3136.0, 3.6277, 77713.7715 },
    { 2676.0, 4.4181, 7860.4194 },
    { 2343.0, 6.1352, 3930.2097 },
    { 1324.0, 0.7425, 11506.7698 },
    { 1273.0, 2.0371, 529.691 },
    { 1199.0, 1.1096, 1577.3435 },
    { 990, 5.233, 5884.927 },
    { 902, 2.045, 26.298 },
    { 857, 3.508, 398.149 },
    { 780, 1.179, 5223.694 },
    { 753, 2.533, 5507.553 },
    { 505, 4.583, 18849.228 },
    { 492, 4.205, 775.523 },
    { 357, 2.92, 0.067 },
    { 317, 5.849, 11790.629 },
    { 284, 1.899, 796.298 },
    { 271, 0.315, 10977.079 },
    { 243, 0.345, 5486.778 },
    { 206, 4.806, 2544.314 },
    { 205, 1.869, 5573.143 },
    { 202, 2.4458, 6069.777 },
    { 156, 0.833, 213.299 },
    { 132, 3.411, 2942.463 },
    { 126, 1.083, 20.775 },
    { 115, 0.645, 0.98 },
    { 103, 0.636, 4694.003 },
    { 102, 0.976, 15720.839 },
    { 102, 4.267, 7.114 },
    { 99, 6.21, 2146.17 },
    { 98, 0.68, 155.42 },
    { 86, 5.98, 161000.69 },
    { 85, 1.3, 6275.96 },
    { 85, 3.67, 71430.7 },
    { 80, 1.81, 17260.15 },
    { 79, 3.04, 12036.46 },
    { 71, 1.76, 5088.63 },
    { 74, 3.5, 3154.69 },
    { 74, 4.68, 801.82 },
    { 70, 0.83, 9437.76 },
    { 62, 3.98, 8827.39 },
    { 61, 1.82, 7084.9 },
    { 57, 2.78, 6286.6 },
    { 56, 4.39, 14143.5 },
    { 56, 3.47, 6279.55 },
    { 52, 0.19, 12139.55 },
    { 52, 1.33, 1748.02 },
    { 51, 0.28, 5856.48 },
    { 49, 0.49, 1194.45 },
    { 41, 5.37, 8429.24 },
    { 41, 2.4, 19651.05 },
    { 39, 6.17, 10447.39 },
    { 37, 6.04, 10213.29 },
    { 37, 2.57, 1059.38 },
    { 36, 1.71, 2352.87 },
    { 36, 1.78, 6812.77 },
    { 33, 0.59, 17789.85 },
    { 30, 0.44, 83996.85 },
    { 30, 2.74, 1349.87 },
    { 25, 3.16, 4690.48 }
};

    double[,] L_TERMS_1 ={

        {628331966747.0, 0, 0},

        {206059.0, 2.678235, 6283.07585},

        {4303.0, 2.6351, 12566.1517},

        {425.0, 1.59, 3.523},

        {119.0, 5.796, 26.298},

        {109.0, 2.966, 1577.344},

        {93, 2.59, 18849.23},

        {72, 1.14, 529.69},

        {68, 1.87, 398.15},

        {67, 4.41, 5507.55},

        {59, 2.89, 5223.69},

        {56, 2.17, 155.42},

        {45, 0.4, 796.3},

        {36, 0.47, 775.52},

        {29, 2.65, 7.11},

        {21, 5.34, 0.98},

        {19, 1.85, 5486.78},

        {19, 4.97, 213.3},

        {17, 2.99, 6275.96},

        {16, 0.03, 2544.31},

        {16, 1.43, 2146.17},

        {15, 1.21, 10977.08},

        {12, 2.83, 1748.02},

        {12, 3.26, 5088.63},

        {12, 5.27, 1194.45},

        {12, 2.08, 4694},

        {11, 0.77, 553.57},

        {10, 1.3, 3286.6},

        {10, 4.24, 1349.87},

        {9, 2.7, 242.73},

        {9, 5.64, 951.72},

        {8, 5.3, 2352.87},

        {6, 2.65, 9437.76},

        {6, 4.67, 4690.48},
    };

    double[,] L_TERMS_2 = {

        {52919.0, 0, 0},

        {8720.0, 1.0721, 6283.0758},

        {309.0, 0.867, 12566.152},

        {27, 0.05, 3.52},

        {16, 5.19, 26.3},

        {16, 3.68, 155.42},

        {10, 0.76, 18849.23},

        {9, 2.06, 77713.77},

        {7, 0.83, 775.52},

        {5, 4.66, 1577.34},

        {4, 1.03, 7.11},

        {4, 3.44, 5573.14},

        {3, 5.14, 796.3},

        {3, 6.05, 5507.55},

        {3, 1.19, 242.73},

        {3, 6.12, 529.69},

        {3, 0.31, 398.15},

        {3, 2.28, 553.57},

        {2, 4.38, 5223.69},

        {2, 3.75, 0.98}
};

    double[,] L_TERMS_3 = {

        {289.0, 5.844, 6283.076},

        {35, 0, 0},

        {17, 5.49, 12566.15},

        {3, 5.2, 155.42},

        {1, 4.72, 3.52},

        {1, 5.3, 18849.23},

        {1, 5.97, 242.73}
};

    double[,] L_TERMS_4 = {

        {114.0, 3.142, 0},

        {8, 4.13, 6283.08},

        {1, 3.84, 12566.15}
    };

    double[,] L_TERMS_5 = {

        {1, 3.14, 0}
    };


    double[,] R_TERMS_0 = {
    
        {100013989.0, 0, 0},
    
        {1670700.0, 3.0984635, 6283.07585},
    
        {13956.0, 3.05525, 12566.1517},
    
        {3084.0, 5.1985, 77713.7715},
    
        {1628.0, 1.1739, 5753.3849},
    
        {1576.0, 2.8469, 7860.4194},
    
        {925.0, 5.453, 11506.77},
    
        {542.0, 4.564, 3930.21},
    
        {472.0, 3.661, 5884.927},
    
        {346.0, 0.964, 5507.553},
    
        {329.0, 5.9, 5223.694},
    
        {307.0, 0.299, 5573.143},
    
        {243.0, 4.273, 11790.629},
    
        {212.0, 5.847, 1577.344},
    
        {186.0, 5.022, 10977.079},
    
        {175.0, 3.012, 18849.228},
    
        {110.0, 5.055, 5486.778},
    
        {98, 0.89, 6069.78},
    
        {86, 5.69, 15720.84},
    
        {86, 1.27, 161000.69},
    
        {85, 0.27, 17260.15},
    
        {63, 0.92, 529.69},
    
        {57, 2.01, 83996.85},
    
        {56, 5.24, 71430.7},
    
        {49, 3.25, 2544.31},
    
        {47, 2.58, 775.52},
    
        {45, 5.54, 9437.76},
    
        {43, 6.01, 6275.96},
    
        {39, 5.36, 4694},
    
        {38, 2.39, 8827.39},
    
        {37, 0.83, 19651.05},
    
        {37, 4.9, 12139.55},
    
        {36, 1.67, 12036.46},
    
        {35, 1.84, 2942.46},
    
        {33, 0.24, 7084.9},
    
        {32, 0.18, 5088.63},
    
        {32, 1.78, 398.15},
    
        {28, 1.21, 6286.6},
    
        {28, 1.9, 6279.55},
    
        {26, 4.59, 10447.39}
};

    double[,] R_TERMS_1 = {
    
        {103019.0, 1.10749, 6283.07585},
    
        {1721.0, 1.0644, 12566.1517},
    
        {702.0, 3.142, 0},
    
        {32, 1.02, 18849.23},
    
        {31, 2.84, 5507.55},
    
        {25, 1.32, 5223.69},
    
        {18, 1.42, 1577.34},
    
        {10, 5.91, 10977.08},
    
        {9, 1.42, 6275.96},
    
        {9, 0.27, 5486.78}
};

    double[,] R_TERMS_2 = {
    
        {4359.0, 5.7846, 6283.0758},
    
        {124.0, 5.579, 12566.152},
    
        {12, 3.14, 0},
    
        {9, 3.63, 77713.77},
    
        {6, 1.87, 5573.14},
    
        {3, 5.47, 18849}
};

    double[,] R_TERMS_3 = {
    
        {145.0, 4.273, 6283.076},
    
        {7, 3.92, 12566.15}
};

    double[,] R_TERMS_4 = {
    
        {4, 2.56, 6283.08}
};

    double[,] B_TERMS_0 = {
    {280.0, 3.199, 84334.662},
    {102.0, 5.422, 5507.553},
    {80, 3.88, 5223.69},
    {44, 3.7, 2352.87},
    {32, 4, 1577.34}
};

    double[,] B_TERMS_1 = {
    
        {9, 3.9, 5507.55},
    
        {6, 1.73, 5223.69}
};
    double get_abrration_correction_delta_tau(double jme)
    {
        return -1.0 * 20.4898 / (3600.0 * get_earth_radius_vector_R(jme));
    }
    */
}
