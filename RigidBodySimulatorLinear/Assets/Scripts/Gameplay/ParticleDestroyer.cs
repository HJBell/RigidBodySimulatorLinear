using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDestroyer : MonoBehaviour {


    //--------------------------------------Unity Functions--------------------------------------

    private void Update()
    {
        if (!this.GetComponent<ParticleSystem>().IsAlive())
            Destroy(this.gameObject);
    }
}
