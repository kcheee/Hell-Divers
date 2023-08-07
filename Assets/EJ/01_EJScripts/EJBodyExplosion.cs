using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBodyExplosion : MonoBehaviour
{
    float radius = 15f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Explosion()
    {
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, radius, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        
        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<EJPlayerHPforTest>().HitByBodyExplosion(3);
        }
        yield return null;
    }
}
