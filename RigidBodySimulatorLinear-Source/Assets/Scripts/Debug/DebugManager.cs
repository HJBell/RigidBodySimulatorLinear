using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {

    [SerializeField]
    private Text ObjectNametext;

    [SerializeField]
    private Text ColliderTypeText;

    [SerializeField]
    private Text VelocityText;

    [SerializeField]
    private Text MassText;

    [SerializeField]
    private Text CoeffOfRestText;

    [SerializeField]
    private Text CollisionText;

    [SerializeField]
    private Image DebugBackground;

    private GameObject mSelectedObject;
    private List<PSI_Collider> mCollisionObjects = new List<PSI_Collider>();


    //--------------------------------------Unity Functions--------------------------------------

    private void LateUpdate()
    {
        DebugBackground.gameObject.SetActive(false);
        ObjectNametext.text = "";
        ColliderTypeText.text = "";
        VelocityText.text = "";
        MassText.text = "";
        CoeffOfRestText.text = "";
        CollisionText.text = "";

        if (mSelectedObject == null) return;

        DebugBackground.gameObject.SetActive(true);

        ObjectNametext.text = mSelectedObject.name;
        ColliderTypeText.text = "COLLIDER TYPE:   " + mSelectedObject.GetComponent<PSI_Collider>().pColliderType.ToString();
        
        if(mSelectedObject.GetComponent<PSI_RigidBody>() != null)
        {
            VelocityText.text = "VELOCITY:   " + mSelectedObject.GetComponent<PSI_RigidBody>().Velocity.ToString();
            MassText.text = "MASS:   " + mSelectedObject.GetComponent<PSI_RigidBody>().pMass.ToString();
            CoeffOfRestText.text = "COEFF OF REST:   " + mSelectedObject.GetComponent<PSI_RigidBody>().pCoeffOfRest.ToString();
        }

        {
            int i = 0;
            while (i < mCollisionObjects.Count)
            {
                if (mCollisionObjects[i] == null) mCollisionObjects.RemoveAt(i);
                else i++;
            }
        }
            
        if (mCollisionObjects.Count > 5)
            mCollisionObjects.RemoveRange(5, mCollisionObjects.Count - 5);

        CollisionText.text = "RECENT COLLISIONS:\n";
        int collisionCount = mCollisionObjects.Count;
        if (collisionCount > 0)
            for (int i = collisionCount - 1; i >= 0; i--)
                CollisionText.text += mCollisionObjects[i].name + "\n";

        mSelectedObject.GetComponent<PSI_Collider>().DrawDebug();
    }


    //-------------------------------------Public Functions-------------------------------------    

    public void ObjectSelected(GameObject obj)
    {
        if (mSelectedObject == obj) return;

        mSelectedObject = obj;
        mCollisionObjects.Clear();
    }

    public void DeselectObject(GameObject obj)
    {
        if (mSelectedObject == obj)
            mSelectedObject = null;
    }

    public void CollisionOccured(PSI_Collider col1, PSI_Collider col2)
    {
        PSI_Collider colToAdd = null;
        if (col1.gameObject == mSelectedObject)
            colToAdd = col2;
        else if (col2.gameObject == mSelectedObject)
            colToAdd = col1;
        if(colToAdd != null)
        {
            mCollisionObjects.Remove(colToAdd);
            mCollisionObjects.Add(colToAdd);
        }        
    }

    public void ExitDebug()
    {
        DeselectObject(mSelectedObject);
    }
}
