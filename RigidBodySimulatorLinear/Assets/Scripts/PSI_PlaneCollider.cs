using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSI_PlaneCollider : PSI_Collider {

    public Vector3 pNormal { get { return this.transform.TransformDirection(Vector3.up).normalized; } }
    public float pArea { get { return Size.x * Size.y; } }

    public Vector2 Size = Vector2.one;


    public Vector3 testP;


    //--------------------------------------Unity Functions--------------------------------------

    private void Awake()
    {
        this.mType = ColliderType.Plane;
    }

    protected override sealed void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        for (float i = 0.0f; i <= 1.01f; i += 0.1f)
        {
            Gizmos.matrix = Matrix4x4.TRS(pPosition, this.transform.rotation, new Vector3(Size.x*i, 0.05f, Size.y*i));
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }


    //-------------------------------------Public Functions-------------------------------------

    public Vector3[] GetPlaneVertices()
    {
        Vector3[] vertices = new Vector3[4];

        vertices[0] = pPosition + this.transform.TransformDirection(new Vector3(Size.x / 2, 0.0f, Size.y / 2));
        vertices[1] = pPosition + this.transform.TransformDirection(new Vector3(Size.x / 2, 0.0f, -Size.y / 2));
        vertices[2] = pPosition + this.transform.TransformDirection(new Vector3(-Size.x / 2, 0.0f, -Size.y / 2));
        vertices[3] = pPosition + this.transform.TransformDirection(new Vector3(-Size.x / 2, 0.0f, Size.y / 2));

        return vertices;
    }
}