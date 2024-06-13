using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogRenderer : HexRenderer
{
    public GameObject particles;
    private void Awake()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        GetComponent<MeshCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;

        m_mesh = new Mesh();
        m_mesh.name = "Hex";

        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = material;
    }

    private void Start()
    {
        var myPS = Instantiate(particles, gameObject.transform);
        var ps = myPS.GetComponent<ParticleSystem>();
        var sh = ps.shape;
        sh.enabled = true;
        sh.shapeType = ParticleSystemShapeType.MeshRenderer;
        sh.meshRenderer = GetComponent<MeshRenderer>();
    }
}
