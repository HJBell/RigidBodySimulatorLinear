using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class ScoreText : MonoBehaviour {

    [SerializeField]
    private float AscendSpeed = 1.0f;

    [SerializeField]
    private float LifeTime = 1.0f;

    private TextMesh mTextMesh;
    private float mSpawnTime = 0.0f;
    private float mTargetSize = 0.0f;


    //--------------------------------------Unity Functions--------------------------------------

    private void Start()
    {
        mSpawnTime = Time.time;
        mTextMesh = GetComponent<TextMesh>();
        mTargetSize = mTextMesh.fontSize;
    }

    private void Update()
    {
        this.transform.Translate(Vector3.up * AscendSpeed * Time.deltaTime);

        var i = Time.time - mSpawnTime;
        var j = i / LifeTime;

        mTextMesh.fontSize = (int)(Mathf.Clamp01(j * 3.0f) * mTargetSize);

        var col = mTextMesh.color;
        col.a = 1.0f - j;
        mTextMesh.color = col;

        if (j >= 1.0f) Destroy(this.gameObject);
    }
}
