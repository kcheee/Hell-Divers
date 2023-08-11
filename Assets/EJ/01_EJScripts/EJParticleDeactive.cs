using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJParticleDeactive : MonoBehaviour
{
    ParticleSystem effect;

    private void OnEnable()
    {
        effect.Play();
        StartCoroutine(makeDisableMe());
    }

    IEnumerator makeDisableMe()
    {
        yield return new WaitForSeconds(0.3f);
        EJObjectPoolMgr.instance.ReturnGausCannonMuzzleImpactQueue(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
