using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class HexRenderer : MonoBehaviour
{
    public Mesh m_mesh; // сам меш
    public MeshFilter m_meshFilter; // хранение данных о меше
    public MeshRenderer m_meshRenderer; // отрисовка меша

    private List<Face> m_faces; // грани

    public Material material; // материал
    public float innerSize; // внешняя граница
    public float outerSize; // внутренняя граница
    public float height; // высота
    public bool isFlatTopped; // является ли плоской сверху

    // настройка хекса при создании объекта
    private void Awake()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        GetComponent<MeshCollider>().convex = true;
        GetComponent<MeshCollider>().isTrigger = true;

        m_mesh = new Mesh();
        m_mesh.name = "Hex";

        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = material;
    }

    // отрисовка меша граней
    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();

        Vector3 boundSize = new Vector3(outerSize, height, outerSize);
        if (isFlatTopped)
        {
            boundSize.x *= 2f;
            boundSize.z *= 1.75f;
        }
        else
        {
            boundSize.x *= 1.75f;
            boundSize.z *= 2f;
        }
        m_mesh.bounds = new Bounds(Vector3.zero, boundSize);

        GetComponent<MeshCollider>().sharedMesh = m_mesh;
    }

    // создание граней
    public void DrawFaces()
    {
        m_faces = new List<Face>();

        //Top faces
        for(int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
        }

        //Bottom faces
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, outerSize, -height / 2f, -height / 2f, point, true));
        }

        //Outer faces
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(outerSize, outerSize, height / 2f, -height / 2f, point, true));
        }

        //Inner faces
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, innerSize, height / 2f, -height / 2f, point, false));
        }
    }

    // создание одной грани
    private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRad, heightB, point);
        Vector3 pointB = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad, heightA, point);
        List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
        List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) }; 
        if (reverse)
        {
            vertices.Reverse();
        }
        return new Face(vertices, triangles, uvs);
    }

    // получение точки
    protected Vector3 GetPoint(float size, float height, int index)
    {
        float angle_deg = isFlatTopped ? 60 * index : 60 * index - 30;
        float angle_rad = Mathf.PI / 180f * angle_deg;
        return new Vector3((size * Mathf.Cos(angle_rad)), height, size * Mathf.Sin(angle_rad));
    }

    // складывание гранией в единую фигуру
    private void CombineFaces()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int i = 0; i < m_faces.Count; i++)
        {
            //Add the vertices
            vertices.AddRange(m_faces[i].vertices);
            uvs.AddRange(m_faces[i].uvs);

            //Offset the triangles
            int offset = (4 * i);
            foreach (int triangle in m_faces[i].triangles)
            {
                tris.Add(triangle + offset);
            }
        }

        m_mesh.vertices = vertices.ToArray(); 
        m_mesh.triangles = tris.ToArray(); 
        m_mesh.uv = uvs.ToArray(); 
        m_mesh.RecalculateNormals();
    }

    public void SetMaterial(Material mat)
    {
        m_meshRenderer.material = mat;
    } 
}

public struct Face
{
    public List<Vector3> vertices { get; private set; }
    public List<int> triangles { get; private set; }
    public List<Vector2> uvs { get; private set; }
    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        this.vertices = vertices;
        this.triangles = triangles; 
        this.uvs = uvs;
    }
}
