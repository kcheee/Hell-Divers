using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject eft;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            //Debug.Log(collision.transform.position);
            Instantiate(eft,gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
