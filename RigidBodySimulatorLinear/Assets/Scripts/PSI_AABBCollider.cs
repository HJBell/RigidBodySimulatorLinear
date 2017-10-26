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

    public override sealed void HandleCollision(PSI_Collider collider)
    {
        switch (collider.pColliderType)
        {
            case ColliderType.Sphere:
                // Implement sphere on AABB collision.
                break;
            case ColliderType.AABB:
                HandleAABBCollision((PSI_AABBCollider)collider);
                break;
        }
    }


    //------------------------------------Private Functions-------------------------------------

    private void HandleAABBCollision(PSI_AABBCollider collider)
    {
        print("AABB on AABB collision!");

        // Calculating the translation vector between the two collider positions.
        var transV = this.pPosition - collider.pPosition; // Translation vector.
        if (Mathf.Abs(transV.x) <= Mathf.Abs(transV.y) && Mathf.Abs(transV.x) <= Mathf.Abs(transV.z)) transV.x = 0.0f;
        if (Mathf.Abs(transV.y) <= Mathf.Abs(transV.x) && Mathf.Abs(transV.y) <= Mathf.Abs(transV.z)) transV.y = 0.0f;
        if (Mathf.Abs(transV.z) <= Mathf.Abs(transV.x) && Mathf.Abs(transV.z) <= Mathf.Abs(transV.y)) transV.z = 0.0f;
        print(transV);

        // Move the boxes back to the point of collision.

        // Calculate the impulse.

        // Apply the impulse.
    }
}
