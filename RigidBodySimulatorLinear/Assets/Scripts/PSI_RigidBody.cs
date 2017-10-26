using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSI_RigidBody : MonoBehaviour {

    public float pMass { get { return Mass; } }
    public float pCoeffOfRest { get { return CoeffOfRest; } }

    public Vector3 Velocity = Vector3.zero;

    [SerializeField]
    [Range(1.0f, 20.0f)]
    private float Mass = 1.0f;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    [Tooltip("The coefficient of restitution for the body")]
    private float CoeffOfRest = 1.0f;

    [SerializeField]
    private bool UseGravity = true;

    private Vector3 mForceThisFrame = Vector3.zero;


    //--------------------------------------Unity Functions--------------------------------------

    private void LateUpdate()
    {
        var acceleration = this.mForceThisFrame / this.Mass;
        this.Velocity += acceleration * Time.deltaTime;
        this.mForceThisFrame = Vector3.zero;

        if (this.UseGravity) this.Velocity.y += Physics.gravity.y * Time.deltaTime;

        this.transform.Translate(this.Velocity * Time.deltaTime);
    }


    //-------------------------------------Public Functions-------------------------------------

    public void AddForce(Vector3 force)
    {
        this.mForceThisFrame += force;
    }
}
