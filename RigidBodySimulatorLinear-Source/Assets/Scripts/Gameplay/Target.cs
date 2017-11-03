using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {

    [SerializeField]
    private float PointsRadius1 = 1.0f;

    [SerializeField]
    private float PointsRadius2 = 2.0f;

    [SerializeField]
    private float PointsRadius3 = 3.0f;

    [SerializeField]
    private float PointsRadius4 = 4.0f;


    //--------------------------------------Unity Functions--------------------------------------

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, PointsRadius1);
        Gizmos.DrawWireSphere(this.transform.position, PointsRadius2);
        Gizmos.DrawWireSphere(this.transform.position, PointsRadius3);
        Gizmos.DrawWireSphere(this.transform.position, PointsRadius4);
    }


    //-------------------------------------Public Functions-------------------------------------

    public void AddProjectileScore(Transform projectileTrans)
    {
        var dist = Vector3.Distance(this.transform.position, projectileTrans.position);
        string scoreString = "";
        if (dist <= PointsRadius1) scoreString = "+50";
        else if (dist <= PointsRadius2) scoreString = "+20";
        else if (dist <= PointsRadius3) scoreString = "+5";
        else if (dist <= PointsRadius4) scoreString = "+1";

        var obj = Instantiate(Resources.Load("Score Text"), projectileTrans.position, Quaternion.identity) as GameObject;
        obj.GetComponent<TextMesh>().text = scoreString;
    }
}
