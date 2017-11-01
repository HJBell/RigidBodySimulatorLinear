using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSI_PlaneCollider : PSI_Collider {

    public Vector3 pNormal { get { return this.transform.TransformDirection(Vector3.up); } }

    public Vector2 Size;


    //--------------------------------------Unity Functions--------------------------------------

    private void Awake()
    {
        this.mType = ColliderType.Plane;
    }

    protected override sealed void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(pPosition, pPosition + pNormal);
        Gizmos.matrix = Matrix4x4.TRS(pPosition, this.transform.rotation, new Vector3(Size.x, 0.05f, Size.y));
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}