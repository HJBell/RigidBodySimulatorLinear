using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderType
{
    Sphere, AABB, Plane
};

[RequireComponent(typeof(Collider))]
public abstract class PSI_Collider : MonoBehaviour
{
    public ColliderType pColliderType { get { return mType; } }
    public Vector3 pPosition { get { return this.transform.position + this.transform.TransformDirection(LocalPosition); } }
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
        if(FindObjectOfType<PSI_CollisionManager>() != null)
            FindObjectOfType<PSI_CollisionManager>().RemoveCollider(this);
        if (FindObjectOfType<DebugManager>() != null)
            FindObjectOfType<DebugManager>().DeselectObject(this.gameObject);
    }

    private void OnMouseDown()
    {
        FindObjectOfType<DebugManager>().ObjectSelected(this.gameObject);
    }

    protected abstract void OnDrawGizmos();


    //-------------------------------------Public Functions-------------------------------------    

    public abstract void DrawDebug();
}
