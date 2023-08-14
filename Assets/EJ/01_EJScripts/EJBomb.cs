using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBomb : MonoBehaviour
{
    //bomb ����
    Rigidbody rb;
    float bombSpeed;
    GameObject bomExploImpact;
    float bombDestroyTime = 0.2f;
    float bombRadius = 2;

    //���� Trail����
    //public TrailRenderer bombTrail;

    // Start is called before the first frame update
    void Start()
    {
        //bombTrail = GetComponent<TrailRenderer>();
        //bombTrail.enabled = true;

        //bombFire Speed, Angle Random�ϰ�
        bombSpeed = Random.Range(5,10);
      
        Vector3 rot = transform.eulerAngles;
        rot.x += Random.Range(-5, 5);
        rot.y += Random.Range(-5, 5);
        rot.z += Random.Range(-5, 5);
        transform.eulerAngles = rot;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * bombSpeed * Time.deltaTime;
    }

/*    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            transform.position = other.transform.position;
            //���� �ε����� bomb ���ֱ�
            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);

            //04.coroutine���� ���� �� ������Ʈ�� ���� GameObject�� ���� �Ŀ��� �۵��ϵ��� �Ѵ�.
            EJGlobalCoroutine.instance.StartCoroutine(bombExplode(other));
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
 

            //04.coroutine���� ���� �� ������Ʈ�� ���� GameObject�� ���� �Ŀ��� �۵��ϵ��� �Ѵ�.
           StartCoroutine(bombExplode(collision));

            //���� �ε����� bomb ���ֱ�
            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);
        }
    }



    //bomb �ܻ��� ������ ������ �Լ�
    IEnumerator bombExplode (Collision collision)
    {
        bomExploImpact = EJObjectPoolMgr.instance.GetbombExploImpactQueue();

        bomExploImpact.transform.position = transform.position;
        bomExploImpact.transform.localScale = Vector3.one *3;
        bomExploImpact.transform.forward = collision.GetContact(0).normal;

        yield return new WaitForSeconds(bombDestroyTime);

        //bomb�ݰ� ���� player damage // ���� �ȵǰ� ����
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        foreach (RaycastHit hitObj in bombHits)
        {
            hitObj.transform.GetComponent<EJPlayerHPforTest>().DamageHP(3);
        }

        EJObjectPoolMgr.instance.ReturnbombExploImpactQueue(bomExploImpact);
        yield return null;

         BossFSM.Sflag = false;
    }

}

