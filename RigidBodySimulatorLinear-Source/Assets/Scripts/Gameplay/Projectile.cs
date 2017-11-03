using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private float SelfDestructDuration = 0.5f;

    [SerializeField]
    private GameObject DeathParticles;

    private bool mCountingDown = false;


    //--------------------------------------Unity Functions--------------------------------------

    private void Update ()
    {
        if (this.transform.position.y < -20.0f)
            Destroy(this.gameObject);

        if (this.GetComponent<PSI_RigidBody>().Velocity.magnitude <= 0.5f && !mCountingDown)
            StartCoroutine(SelfDestruct());
    }


    //-------------------------------------Private Functions-------------------------------------

    private IEnumerator SelfDestruct()
    {
        mCountingDown = true;
        yield return new WaitForSeconds(SelfDestructDuration);
        if (this.GetComponent<PSI_RigidBody>().Velocity.magnitude <= 0.5f)
        {
            FindObjectOfType<Target>().AddProjectileScore(this.transform);
            Instantiate(DeathParticles, this.transform.position, DeathParticles.transform.rotation);
            Destroy(this.gameObject);
        }
        else
            mCountingDown = false;
    }
}
