using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJFireArmReaction : MonoBehaviour
{
    public GameObject rightArm;
    public GameObject leftArm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void fireRightArmReaction()
    {
        rightArm.transform.position = new Vector3(0, 0, 1);
        rightArm.transform.position = Vector3.Lerp(rightArm.transform.position, Vector3.one, 0.7f);
    }

    void fireLeftArmReaction()
    {
        leftArm.transform.position = new Vector3(0, 0, 1);
        leftArm.transform.position = Vector3.Lerp(leftArm.transform.position, Vector3.one, 0.7f);
    }
}
