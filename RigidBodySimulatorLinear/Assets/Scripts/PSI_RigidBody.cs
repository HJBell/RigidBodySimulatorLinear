using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSI_RigidBody : MonoBehaviour {

    [SerializeField]
    [Range(1.0f, 20.0f)]
    private float Mass = 1.0f;

    [SerializeField]
    private bool IsStatic = false;

    [SerializeField]
    private bool UseGravity = true;

    private Vector3 mVelocity = Vector3.zero;
    private Vector3 mForceThisFrame = Vector3.zero;


    //--------------------------------------Unity Functions--------------------------------------

    private void LateUpdate()
    {
        var acceleration = this.mForceThisFrame / this.Mass;
        this.mVelocity += acceleration * Time.deltaTime;
        this.mForceThisFrame = Vector3.zero;

        if (this.UseGravity) this.mVelocity.y += Physics.gravity.y * Time.deltaTime;

        if(!IsStatic) this.transform.Translate(this.mVelocity * Time.deltaTime);
    }


    //-------------------------------------Public Functions-------------------------------------

    public void AddForce(Vector3 force)
    {
        this.mForceThisFrame += force;
    }
}
