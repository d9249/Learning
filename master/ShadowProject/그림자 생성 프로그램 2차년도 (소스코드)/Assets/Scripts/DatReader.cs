using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DatReader {

    /// <summary>
    /// 타일의 인덱스에 해당하는 DAT파일을 요청하여 
    /// </summary>
    /// <param name="wwwDat">Dat파일요청정보</param>
    /// <returns></returns>
    public static List<XDOInfo> readDat(WWW wwwDat)
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

}
