using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBodyExplosion : MonoBehaviour
{
    //explosion ����
    float radius = 5f;
    float bombExploDamage = 3f;     //**player�� maxHP�� ����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(BodyExplosionCoroutine());
        }
    }

    //explosion �Լ�
    IEnumerator BodyExplosionCoroutine()
    {
        //01. Effect Enqueue
        GameObject bodyExploImpact = EJObjectPoolMgr.instance.GetbodyExploImpactQueue();

        //02. �ݰ� �ȿ� ���� �͵� ����
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, radius, Vector3.up, 0f, LayerMask.GetMask("Player"));
        
        //03. ����� �͵鿡 Damage
        foreach (RaycastHit hitObj in rayHits)
        {
            //**Player HP ������ �Լ�, ������ ������ ����
            hitObj.transform.GetComponent<EJPlayerHPforTest>().SetHP(3);
        }

        //04. Effect Dequeue
        yield return new WaitForSeconds(0.3f);
        EJObjectPoolMgr.instance.ReturnbodyExploImpactQueue(bodyExploImpact);
    }
}
