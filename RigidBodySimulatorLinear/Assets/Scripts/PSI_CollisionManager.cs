using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSI_CollisionManager : MonoBehaviour {

    private List<PSI_Collider> mColliders = new List<PSI_Collider>();


    //--------------------------------------Unity Functions--------------------------------------

    private void FixedUpdate()
    {
        for(int i = 0; i < mColliders.Count; i++)
        {
            for(int j = i+1; j < mColliders.Count; j++)
            {
                if(CheckCollision(mColliders[i], mColliders[j]))
                {
                    mColliders[i].HandleCollision(mColliders[j]);
                }
            }
        }
    }


    //-------------------------------------Public Functions-------------------------------------    

    public void AddCollider(PSI_Collider collider)
    {
        if (!mColliders.Contains(collider))
            mColliders.Add(collider);
    }

    public void RemoveCollider(PSI_Collider collider)
    {
        if (mColliders.Contains(collider))
            mColliders.Remove(collider);
    }


    //------------------------------------Private Functions-------------------------------------

    private bool CheckCollision(PSI_Collider col1, PSI_Collider col2)
    {
        if (col1.pColliderType == ColliderType.Sphere && col2.pColliderType == ColliderType.Sphere)
            return SphereSphereCollision((PSI_SphereCollider)col1, (PSI_SphereCollider)col2);
        return false;
    }

    private bool SphereSphereCollision(PSI_SphereCollider col1, PSI_SphereCollider col2)
    {
        return (Vector3.Distance(col1.pPosition, col2.pPosition) <= Mathf.Abs(col1.Radius) + Mathf.Abs(col2.Radius));
    }
}
