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

    //enable�Ǹ� muzzleFXTime�Ŀ� ������ �Լ�
    private void Update()
    {
        if (isActiveAndEnabled)
        {
            StartCoroutine(OffMuzzleFX());
        }
    }

    //muzzleFXTime���� ȿ���� ������ �Լ�
    IEnumerator OffMuzzleFX()
    {
        yield return new WaitForSeconds(muzzleFXTime);
        EJObjectPoolMgr.instance.ReturnGausCannonMuzzleImpactQueue(gameObject);
    }
}
