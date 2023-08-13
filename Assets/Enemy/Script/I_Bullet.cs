using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Bullet : MonoBehaviour
{
    public GameObject eft;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.ToString());
        if (collision.gameObject.name == "Plane")
        {
            //Debug.Log(collision.transform.position);
            Instantiate(eft,gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
