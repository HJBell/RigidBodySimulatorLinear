﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ImpulseResult
{
    public Vector3 Impulse1;
    public Vector3 Impulse2;
}

public static class PSI_Physics {


    //-----------------------------------Impulse Calculation------------------------------------

    public static ImpulseResult CalculateImpulse(PSI_RigidBody rb1, PSI_RigidBody rb2, Vector3 minTranslation)
    {
        ImpulseResult impulseResult;
        impulseResult.Impulse1 = Vector3.zero;
        impulseResult.Impulse2 = Vector3.zero;

        var impactVelocity = rb1.Velocity - rb2.Velocity;
        var inverseMass1 = 1.0f / rb1.pMass;
        var inverseMass2 = 1.0f / rb2.pMass;

        var vn = Vector3.Dot(impactVelocity, minTranslation.normalized);
        if (vn > 0.0f) return impulseResult;

        var finalCoeffOfRest = rb1.pCoeffOfRest * rb2.pCoeffOfRest;
        var i = (-(1.0f + finalCoeffOfRest) * vn) / (inverseMass1 + inverseMass2);
        var impulse = minTranslation.normalized * i;

        impulseResult.Impulse1 = impulse * inverseMass1;
        impulseResult.Impulse2 = -impulse * inverseMass2;

        return impulseResult;
    }

    public static Vector3 CalculateImpulseAgainstStaticCollider(PSI_RigidBody rb, Vector3 minTranslation)
    {
        var impactVelocity = rb.Velocity;
        var inverseMass = 1.0f / rb.pMass;

        var vn = Vector3.Dot(impactVelocity, minTranslation.normalized);
        if (vn > 0.0f) return Vector3.zero;
        
        var i = (-(1.0f + rb.pCoeffOfRest) * vn) / inverseMass;
        var impulse = minTranslation.normalized * i;

        return impulse * inverseMass;
    }

    public static Vector3 CalculateImpulseAgainstStaticRB(PSI_RigidBody rb1, PSI_RigidBody rb2, Vector3 minTranslation)
    {
        ImpulseResult impulseResult;
        impulseResult.Impulse1 = Vector3.zero;
        impulseResult.Impulse2 = Vector3.zero;

        var impactVelocity = rb1.Velocity - rb2.Velocity;
        var inverseMass1 = 1.0f / rb1.pMass;
        var inverseMass2 = 1.0f / rb2.pMass;

        var vn = Vector3.Dot(impactVelocity, minTranslation.normalized);
        if (vn > 0.0f) return Vector3.zero;

        var finalCoeffOfRest = rb1.pCoeffOfRest * rb2.pCoeffOfRest;
        var i = (-(1.0f + finalCoeffOfRest) * vn) / (inverseMass1 + inverseMass2);
        var impulse = minTranslation.normalized * i;

        return impulse * inverseMass1;
    }


    //---------------------------------Checking For Collisions----------------------------------

    public static bool SphereSphereCollisionOccured(PSI_SphereCollider col1, PSI_SphereCollider col2)
    {
        return Vector3.Distance(col1.pPosition, col2.pPosition) <= Mathf.Abs(col1.Radius) + Mathf.Abs(col2.Radius);
    }

    public static bool AABBAABBCollisionOccured(PSI_AABBCollider col1, PSI_AABBCollider col2)
    {
        return ((col1.pPosition.x < col2.pPosition.x + col2.Size.x) &&
                 col1.pPosition.x + col1.Size.x > col2.pPosition.x &&
                 col1.pPosition.y < col2.pPosition.y + col2.Size.y &&
                 col1.pPosition.y + col1.Size.y > col2.pPosition.y &&
                 col1.pPosition.z < col2.pPosition.z + col2.Size.z &&
                 col1.pPosition.z + col1.Size.z > col2.pPosition.z);
    }

    public static bool SpherePlaneCollisionOccured(PSI_SphereCollider sphereCol, PSI_PlaneCollider planeCol)
    {
        float distToProjectedPoint = Vector3.Dot(planeCol.pNormal, (sphereCol.pPosition - planeCol.pPosition));
        Vector3 projectedPoint = sphereCol.pPosition - distToProjectedPoint * planeCol.pNormal;

        // Generate 4 triangles between the corners of the plane and the projected point.
        var planeVerts = planeCol.GetPlaneVertices();
        var triangles = new Vector3[4, 3];
        for (int i = 0; i < 4; i++)
        {
            triangles[i, 0] = projectedPoint;
            triangles[i, 1] = planeVerts[i];
            triangles[i, 2] = planeVerts[(i==3)?0:i+1];
        }

        // Sum the area of the traingles.
        float totalTriArea = 0.0f;
        for(int i = 0; i < 4; i++)
        {
            float a = Vector3.Distance(triangles[i, 0], triangles[i, 1]);
            float b = Vector3.Distance(triangles[i, 1], triangles[i, 2]);
            float c = Vector3.Distance(triangles[i, 2], triangles[i, 0]);
            float s = (a + b + c) / 2;
            totalTriArea += Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));
        }

        // Check if the sum area is equal to the area of the plane. 
        // If so then the projected point is within the bounds of the plane.
        if (Mathf.Abs(totalTriArea - planeCol.pArea) > sphereCol.Radius) return false;

        // Check if the sphere is intersecting with the plane.
        return (Mathf.Abs(distToProjectedPoint) <= sphereCol.Radius);
    }


    //-----------------------------------Handling Collisions------------------------------------

    public static void HandleSphereSphereCollision(PSI_SphereCollider col1, PSI_SphereCollider col2)
    {
        // Calculating the collision vector.
        var colVector = col1.pPosition - col2.pPosition;

        // Calculating the minimum translation vector needed to reset the two colliders positions.
        var minTranslationVector = colVector * (((col1.Radius + col2.Radius) - colVector.magnitude) / colVector.magnitude);

        // Calculating the inverse masses once for efficiency.
        var inverseMass1 = 1.0f / col1.pRigidBody.pMass;
        var inverseMass2 = 1.0f / col2.pRigidBody.pMass;

        // Moving the two colliders so they are not overlapping.
        col1.transform.Translate(minTranslationVector * (inverseMass1 / (inverseMass1 + inverseMass2)));
        col2.transform.Translate(-minTranslationVector * (inverseMass2 / (inverseMass1 + inverseMass2)));

        // Applying the impulse to the bodies.
        var impulse = PSI_Physics.CalculateImpulse(col1.pRigidBody, col2.pRigidBody, minTranslationVector);
        col1.pRigidBody.Velocity += impulse.Impulse1;
        col2.pRigidBody.Velocity += impulse.Impulse2;
    }

    public static void HandleAABBAABBCollision(PSI_AABBCollider col1, PSI_AABBCollider col2)
    {
        // Calculating the collision vector.
        Vector3 colVector = col1.pPosition - col2.pPosition;

        // Determining what axis the collision occured on.
        List<float> colVectorAxes = new List<float>() { Mathf.Abs(colVector.x), Mathf.Abs(colVector.y), Mathf.Abs(colVector.z) };
        colVectorAxes.Sort();
        string axisName = "";
        if (Mathf.Abs(colVector.x) == colVectorAxes[2]) axisName = "x";
        if (Mathf.Abs(colVector.y) == colVectorAxes[2]) axisName = "y";
        if (Mathf.Abs(colVector.z) == colVectorAxes[2]) axisName = "z";

        // Generating an axis aligned collision vector.
        object tempVector = Vector3.zero;
        typeof(Vector3).GetField(axisName).SetValue(tempVector, typeof(Vector3).GetField(axisName).GetValue(colVector));
        Vector3 aaColVector = (Vector3)tempVector;

        // Getting the size of each collider on the collision axis.
        float size1 = (float)col1.Size.GetType().GetField(axisName).GetValue(col1.Size);
        float size2 = (float)col2.Size.GetType().GetField(axisName).GetValue(col2.Size);

        // Calculating the minimum translation vector needed to reset the two colliders positions.
        var minTranslationVector = aaColVector * ((((size1 + size2) * 0.5f) - aaColVector.magnitude) / aaColVector.magnitude);

        // Calculating the inverse masses once for efficiency.
        var inverseMass1 = 1.0f / col1.pRigidBody.pMass;
        var inverseMass2 = 1.0f / col2.pRigidBody.pMass;

        // Moving the two colliders so they are not overlapping.
        col1.transform.Translate(minTranslationVector * (inverseMass1 / (inverseMass1 + inverseMass2)));
        col2.transform.Translate(-minTranslationVector * (inverseMass2 / (inverseMass1 + inverseMass2)));

        // Applying the impulse to the bodies.
        var impulse = PSI_Physics.CalculateImpulse(col1.pRigidBody, col2.pRigidBody, minTranslationVector);
        col1.pRigidBody.Velocity += impulse.Impulse1;
        col2.pRigidBody.Velocity += impulse.Impulse2;
    }

    public static void HandleSpherePlaneCollision(PSI_SphereCollider sphereCol, PSI_PlaneCollider planeCol)
    {
        float distToProjectedPoint = Vector3.Dot(planeCol.pNormal, (sphereCol.pPosition - planeCol.pPosition));
        Vector3 projectedPoint = sphereCol.pPosition - distToProjectedPoint * planeCol.pNormal;

        // Calculating the collision vector.
        var colVector = sphereCol.pPosition - projectedPoint;

        // Calculating the minimum translation vector needed to reset the sphere.
        var minTranslationVector = colVector * ((sphereCol.Radius - colVector.magnitude) / colVector.magnitude);

        // Moving the sphere collider so it is not overlapping.
        sphereCol.transform.Translate(minTranslationVector);

        // Applying the impulse to the bodies.
        if(planeCol.pRigidBody != null)
            sphereCol.pRigidBody.Velocity += PSI_Physics.CalculateImpulseAgainstStaticRB(sphereCol.pRigidBody, planeCol.pRigidBody, minTranslationVector);
        else
            sphereCol.pRigidBody.Velocity += PSI_Physics.CalculateImpulseAgainstStaticCollider(sphereCol.pRigidBody, minTranslationVector);
    }
}
