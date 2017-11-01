using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderType
{
    Sphere, AABB, Plane
};

public abstract class PSI_Collider : MonoBehaviour
{
    public ColliderType pColliderType { get { return mType; } }
    public Vector3 pPosition { get { return this.transform.position + LocalPosition; } }
    public PSI_RigidBody pRigidBody { get { return GetComponent<PSI_RigidBody>(); } }

    public Vector3 LocalPosition;

    protected ColliderType mType;


    //--------------------------------------Unity Functions--------------------------------------

    private void OnEnable()
    {
        FindObjectOfType<PSI_CollisionManager>().AddCollider(this);
    }

    private void OnDisable()
    {
        var collisionManager = FindObjectOfType<PSI_CollisionManager>();
        if(collisionManager != null)
            FindObjectOfType<PSI_CollisionManager>().RemoveCollider(this);
    }

    protected abstract void OnDrawGizmos();
}
