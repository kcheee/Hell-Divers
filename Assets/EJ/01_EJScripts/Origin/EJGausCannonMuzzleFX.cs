using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJGausCannonMuzzleFX : MonoBehaviour
{
    float muzzleFXTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {

    }

    //enable되면 muzzleFXTime후에 꺼지는 함수
    private void Update()
    {
        if (isActiveAndEnabled)
        {
            StartCoroutine(OffMuzzleFX());
        }
    }

    //muzzleFXTime이후 효과가 꺼지는 함수
    IEnumerator OffMuzzleFX()
    {
        yield return new WaitForSeconds(muzzleFXTime);
        EJObjectPoolMgr.instance.ReturnGausCannonMuzzleImpactQueue(gameObject);
    }
}
