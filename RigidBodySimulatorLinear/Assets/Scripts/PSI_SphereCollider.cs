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
        switch(collider.pColliderType)
        {
            case ColliderType.Sphere:
                HandleSphereCollision((PSI_SphereCollider)collider);
                break;
            case ColliderType.AABB:
                // Implement sphere on AABB collision.
                break;
        }
    }


	//------------------------------------Private Functions-------------------------------------

	private void HandleSphereCollision(PSI_SphereCollider collider)
	{
		// Calculating the difference between the two collider positions.
		var differenceVector = this.pPosition - collider.pPosition;
		var differenceLength = differenceVector.magnitude;

		// Calculating the minimum translation vector needed to reset the two colliders positions.
		var minTranslationVector = differenceVector * (((this.Radius + collider.Radius) - differenceLength) / differenceLength);

		// Calculating the inverse masses once for efficiency.
		var inverseMass1 = 1.0f / this.pRigidBody.pMass;
		var inverseMass2 = 1.0f / collider.pRigidBody.pMass;

		// Moving the two colliders so they are not overlapping.
		this.transform.Translate(minTranslationVector * (inverseMass1 / (inverseMass1 + inverseMass2)));
		collider.transform.Translate(-minTranslationVector * (inverseMass2 / (inverseMass1 + inverseMass2)));

		// Calculating the impact velocity.
		var impactVelocity = this.pRigidBody.Velocity - collider.pRigidBody.Velocity;

		var vn = Vector3.Dot(impactVelocity, minTranslationVector.normalized);
		if (vn > 0.0f) return;

        // Calculating the impulse of the collision.
        var finalCoeffOfRest = this.pRigidBody.pCoeffOfRest * collider.pRigidBody.pCoeffOfRest;
        var i = (-(1.0f + finalCoeffOfRest) * vn) / (inverseMass1 + inverseMass2);
		var impulse = minTranslationVector * i;

		// Applying the impulse to the rigid bodies.
		this.pRigidBody.Velocity += impulse * inverseMass1;
		collider.pRigidBody.Velocity -= impulse * inverseMass2;
	}
}
