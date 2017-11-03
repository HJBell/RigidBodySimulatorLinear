using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obsticle : MonoBehaviour {

    [SerializeField]
    private float MoveSpeed;

    [SerializeField]
    private float MoveAmplitude;

    [SerializeField]
    private float Offset;


    //--------------------------------------Unity Functions--------------------------------------

    private void Update()
    {
        var pos = this.transform.position;
        pos.x = Mathf.Sin((Time.time + Offset * Mathf.PI) * MoveSpeed) * MoveAmplitude;
        this.transform.position = pos;
    }
}
