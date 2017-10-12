using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSI_SphereCollider : PSI_Collider {

    public float Radius;


    //--------------------------------------Unity Functions--------------------------------------

    private void Awake()
    {
        this.mType = ColliderType.Sphere;
    }


    //-------------------------------------Public Functions-------------------------------------

    public override void OnCollision(PSI_Collider collider)
    {
        if (this.RigidBody)
        {
            // TODO: Write accurate force calculation formula and move to 



            var forceDirection = (this.Position - collider.Position).normalized;
            var force = forceDirection * 100.0f;
            this.RigidBody.AddForce(force);
        }
    }
}
