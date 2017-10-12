using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderType
{
    Sphere
};

public abstract class PSI_Collider : MonoBehaviour
{
    public ColliderType ColliderType { get { return mType; } }
    public Vector3 Position { get { return this.transform.position + LocalPosition; } }
    public PSI_RigidBody RigidBody { get { return GetComponent<PSI_RigidBody>(); } }

    public Vector3 LocalPosition;

    protected ColliderType mType;


    //--------------------------------------Unity Functions--------------------------------------

    private void OnEnable()
    {
        FindObjectOfType<PSI_CollisionManager>().AddCollider(this);
    }

    private void OnDisable()
    {
        FindObjectOfType<PSI_CollisionManager>().RemoveCollider(this);
    }


    //-------------------------------------Public Functions-------------------------------------

    public abstract void OnCollision(PSI_Collider collider);
}
