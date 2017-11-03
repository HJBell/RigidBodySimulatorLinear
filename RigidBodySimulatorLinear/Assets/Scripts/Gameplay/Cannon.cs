using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    [SerializeField]
    private Transform PivotTransform;

    [SerializeField]
    private Transform ProjectileSpawnPoint;

    [SerializeField]
    private float RotateSpeed = 1.0f;

    [SerializeField]
    private GameObject ProjectilePrefab;

    [SerializeField]
    private float ProjectileSpeed;

    //--------------------------------------Unity Functions--------------------------------------

    private void Update()
    {
        float rotInput = Input.GetAxis("Horizontal");
        PivotTransform.Rotate(Vector3.up * rotInput * RotateSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity) as GameObject;
            obj.GetComponent<PSI_RigidBody>().Velocity = ProjectileSpawnPoint.transform.forward * ProjectileSpeed;
        }
    }
}
