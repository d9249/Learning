using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// XDO 3D Mesh Data Block
/// </summary>
public class XDOMesh
{
    /// <summary>
    /// Vertex 목록
    /// </summary>
    private List<Vector3> m_vertex = new List<Vector3>();
    /// <summary>
    /// Normal 목록
    /// </summary>
    private List<Vector3> m_normals = new List<Vector3>();
    /// <summary>
    /// UV 목록
    /// </summary>
    private List<Vector2> m_uvs = new List<Vector2>();
    /// <summary>
    /// Index 목록
    /// </summary>
    private List<int> m_index = new List<int>();
    /// <summary>
    /// Mesh Color
    /// </summary>
    public Color32 Color { get; set; }

    public byte ImageLevel { get; set; }
    /// <summary>
    /// Texture 이미지 파일명
    /// </summary>
    public string ImageName { get; set; }
    /// <summary>
    /// Texture Nail 이미지, jpg 바이너리
    /// </summary>
    public byte[] Image { get; set; }

    public XDOMesh(System.IO.BinaryReader reader)
    {
        // Vertex 읽기
        var vertexCount = reader.ReadUInt32();
        for (int i = 0; i < vertexCount; i++)
        {
            // Vertex
            m_vertex.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            // normal
            m_normals.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            // uv
            m_uvs.Add(new Vector2(reader.ReadSingle(), 1f - reader.ReadSingle()));
        }
        
        // Index 읽기
        var indexCount = reader.ReadUInt32();
        for (int i = 0; i < indexCount; i++)
        {
            m_index.Add(reader.ReadUInt16());
        }

        // Color 읽기
        var c = reader.ReadUInt32();
        byte A = (byte)((c >> 24) & 0xFF);
        byte R = (byte)((c >> 16) & 0xFF);
        byte G = (byte)((c >> 8) & 0xFF);
        byte B = (byte)((c) & 0xFF);
        this.Color = new Color32(R, G, B, A);

        // Image Level
        this.ImageLevel = reader.ReadByte();
        
        // Image Name
        var imgNameLen = reader.ReadByte();
        if (imgNameLen > 0)
        {
            this.ImageName = Encoding.UTF8.GetString(reader.ReadBytes(imgNameLen));
            // Nail Image 읽기
            var nailLen = reader.ReadUInt32();
            this.Image = reader.ReadBytes((int)nailLen);
        }
        else
        {
            this.ImageName = null;
        }
    }

    public List<Vector3> Vertex {  get { return m_vertex; } }
    public List<Vector3> Normals { get { return m_normals; } }
    public List<Vector2> UV { get { return m_uvs; } }
    public List<int> Index { get { return m_index; } }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb
            .Append("Vertex Count : " + m_vertex.Count).Append(Environment.NewLine)
            .Append("Index Count : " + m_index.Count).Append(Environment.NewLine)
            .Append("Color : ").Append(this.Color).Append(Environment.NewLine)
            .Append("Image Level : ").Append(this.ImageLevel).Append(Environment.NewLine)
            .Append("Image Name : ").Append(this.ImageName).Append(Environment.NewLine)
            .Append("Nail Length : ").Append(this.Image.Length).Append(Environment.NewLine);

        return sb.ToString();
    }

    public void Serialize(System.IO.BinaryWriter writer)
    {
        if (string.IsNullOrEmpty(this.ImageName))
            throw new InvalidOperationException();
        var imgNameBuf = Encoding.UTF8.GetBytes(this.ImageName);
        if ((int)byte.MaxValue < imgNameBuf.Length)
            throw new InvalidOperationException();

        if (this.Vertex.Count != this.Normals.Count ||
            this.Vertex.Count != this.UV.Count)
            throw new InvalidOperationException();

        if (this.Vertex.Count == 0 || this.Index.Count == 0)
            throw new InvalidOperationException();

        if (string.IsNullOrEmpty(this.ImageName) ||
            this.Image.Length == 0)
            throw new InvalidOperationException();

        writer.Write((uint)this.Vertex.Count);
        for (int i = 0; i < this.Vertex.Count; i++)
        {
            var v = this.Vertex[i];
            var n = this.Normals[i];
            var uv = this.UV[i];

            writer.Write(v.x);
            writer.Write(v.y);
            writer.Write(v.z);

            writer.Write(n.x);
            writer.Write(n.y);
            writer.Write(n.z);

            writer.Write(uv.x);
            writer.Write(uv.y);
        }

        writer.Write((uint)this.Index.Count);
        foreach (var idx in this.Index)
        {
            writer.Write((ushort)idx);
        }

        // Color
        var color = ((uint)this.Color.a << 24) + ((uint)this.Color.r << 16) + 
            ((uint)this.Color.g << 8) + (uint)this.Color.b;
        writer.Write(color);

        // Image Level
        writer.Write(this.ImageLevel);
        // Image Name Length
        writer.Write((byte)imgNameBuf.Length);
        // Image Name
        writer.Write(imgNameBuf);

        // Nail Byte Length
        writer.Write((uint)this.Image.Length);
        // Nail Byte
        writer.Write(this.Image);
    }

}
