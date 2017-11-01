﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSI_SphereCollider : PSI_Collider {

    public float Radius;


    //--------------------------------------Unity Functions--------------------------------------

    private void Awake()
    {
        this.mType = ColliderType.Sphere;
    }

    protected override sealed void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(pPosition, Radius);
    }
}
