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

    protected override sealed void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(pPosition, Radius);
    }


    //-------------------------------------Public Functions-------------------------------------

    public override sealed void HandleCollision(PSI_Collider collider)
    {
        var differenceVector = this.pPosition - collider.pPosition;
        var differenceLength = differenceVector.magnitude;
        var minTranslationVector = differenceVector * (((this.Radius + ((PSI_SphereCollider)collider).Radius) - differenceLength) / differenceLength);

        var inverseMass1 = 1.0f / this.pRigidBody.pMass;
        var inverseMass2 = 1.0f / collider.pRigidBody.pMass;

        this.transform.Translate(minTranslationVector * (inverseMass1 / (inverseMass1 + inverseMass2)));
        collider.transform.Translate(-minTranslationVector * (inverseMass2 / (inverseMass1 + inverseMass2)));

        var impactVelocity = this.pRigidBody.Velocity - collider.pRigidBody.Velocity;

        var vn = Vector3.Dot(impactVelocity, minTranslationVector.normalized);

        if (vn > 0.0f) return;

        var i = (-(1.0f + 1.0f) * vn) / (inverseMass1 + inverseMass2);
        var impulse = minTranslationVector * i;

        this.pRigidBody.Velocity += impulse * inverseMass1;
        collider.pRigidBody.Velocity -= impulse * inverseMass2;
    }
}
