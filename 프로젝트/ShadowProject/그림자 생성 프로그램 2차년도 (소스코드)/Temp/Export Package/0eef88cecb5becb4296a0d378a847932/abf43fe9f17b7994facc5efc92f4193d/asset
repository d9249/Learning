using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class XDO {
    /// <summary>
    /// XDO Real3D Model은 8로 고정
    /// </summary>
    public const byte XDO_FILE_TYPE = 8;

    private List<XDOMesh> m_meshList = new List<XDOMesh>();

    public byte XDOType { get; set; }

    public uint ObjectID { get; set; }

    public string Key { get; set; }
    
    public double MinX { get; set; }
    public double MinY { get; set; }
    public double MinZ { get; set; }
    public double MaxX { get; set; }
    public double MaxY { get; set; }
    public double MaxZ { get; set; }


    public float Altitude { get; set; }
    
    public List<XDOMesh> Meshes {  get { return m_meshList; } }

    public XDO(System.IO.BinaryReader reader, uint version)
    {
        // Type
        this.XDOType = reader.ReadByte();
        if (this.XDOType != XDO_FILE_TYPE)
            throw new System.IO.FileLoadException();

        // ObjectID
        this.ObjectID = reader.ReadUInt32();

        // 키읽기
        var keyLen = reader.ReadByte();
        this.Key = System.Text.Encoding.UTF8.GetString(
            reader.ReadBytes(keyLen));

        // Obj BOX 읽기
        this.MinX = reader.ReadDouble();
        this.MinY = reader.ReadDouble();
        this.MinZ = reader.ReadDouble();

        this.MaxX = reader.ReadDouble();
        this.MaxY = reader.ReadDouble();
        this.MaxZ = reader.ReadDouble();

        // Altitude
        this.Altitude = reader.ReadSingle();

        var faceNum = 1;
        
        if (version != 16777219)
        {
            // FaceNum
            faceNum = reader.ReadByte();
        }

        // Mesh 읽기
        for (int i = 0; i < faceNum; i++)
            this.Meshes.Add(new XDOMesh(reader));



        reader.Close();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb
            .Append("Type : ").Append(this.XDOType).Append(Environment.NewLine)
            .Append("Object ID : ").Append(this.ObjectID).Append(Environment.NewLine)
            .Append("Key : ").Append(this.Key).Append(Environment.NewLine)
            .Append("Min(x, y, z) : ").Append(string.Format("({0}, {1}, {2})", this.MinX, this.MinY, this.MinZ)).Append(Environment.NewLine)
            .Append("Max(x, y, z) : ").Append(string.Format("({0}, {1}, {2})", this.MaxX, this.MaxY, this.MaxZ)).Append(Environment.NewLine)
            .Append("Altitude : ").Append(this.Altitude).Append(Environment.NewLine)
            .Append("FaceNum : ").Append(this.Meshes.Count).Append(Environment.NewLine);

        for (int i = 0; i < this.Meshes.Count; i++)
        {
            sb.Append("Face ").Append(i).Append(Environment.NewLine)
                .Append(m_meshList[i].ToString()).Append(Environment.NewLine);
        }

        return sb.ToString();
    }

    public void Serialize(System.IO.BinaryWriter writer)
    {
        // Validation 수행
        if (Meshes.Count == 0)
            throw new InvalidOperationException();
        if (string.IsNullOrEmpty(this.Key))
            throw new InvalidOperationException();
        var keyBuf = Encoding.UTF8.GetBytes(this.Key);
        if (keyBuf.Length > (int)System.Byte.MaxValue)
            throw new System.InvalidOperationException("Key가 너무 깁니다.");
        
        writer.Write(this.XDOType);
        writer.Write(this.ObjectID);
        writer.Write((byte)keyBuf.Length);
        writer.Write(keyBuf);
        writer.Write(this.MinX);
        writer.Write(this.MinY);
        writer.Write(this.MinZ);
        writer.Write(this.MaxX);
        writer.Write(this.MaxY);
        writer.Write(this.MaxZ);
        writer.Write(this.Altitude);
        writer.Write((byte)this.Meshes.Count);
        
        foreach (var mesh in m_meshList)
        {
            mesh.Serialize(writer);
        }

        writer.Flush();
        writer.Close();
    }

}
