using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSI_AABBCollider : PSI_Collider {

    public Vector3 Size = Vector3.one;


    //--------------------------------------Unity Functions--------------------------------------

    private void Awake()
    {
        this.mType = ColliderType.AABB;
    }

    protected override sealed void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(pPosition, Size);
    }


    //-------------------------------------Public Functions-------------------------------------    

    public override sealed void DrawDebug(){}
}
