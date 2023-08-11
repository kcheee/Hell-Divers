using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBomb : MonoBehaviour
{
    Rigidbody rb;
    //public GameObject bombImpactFactory;
    float speed;
    GameObject bombImpact;
    float bombDestroyTime = 2f;

    //���� Trail����
    public TrailRenderer bombTrail;

    // Start is called before the first frame update
    void Start()
    {
        bombTrail = GetComponent<TrailRenderer>();
        bombTrail.enabled = true;

        speed = Random.Range(10,30);

        Vector3 rot = transform.eulerAngles;
        rot.x += Random.Range(-5, 5);
        rot.y += Random.Range(-5, 5);
        rot.z += Random.Range(-5, 5);
        transform.eulerAngles = rot;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //02.trailrenderer�� �θ��ڽ� ���踦 ����
            var trail = transform.Find("trail");
            trail.parent = null;

            //03.���� �ð� �Ŀ� bombTrail�� ���ش�.
            Destroy(trail.gameObject, 3);
            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject); 
            
            //04.coroutine���� ���� �� ������Ʈ�� ���� GameObject�� ���� �Ŀ��� �۵��ϵ��� �Ѵ�.
            EJGlobalCoroutine.instance.StartCoroutine(bombImpactMake(collision));          
        }
    }

    //collision�̳� vector3�� �Ű������� �־��ִ� �Լ��� �־��ָ� �ȴ�.
    IEnumerator bombImpactMake (Collision collision)
    {
        bombImpact = EJObjectPoolMgr.instance.GetbombImpactQueue();

        bombImpact.transform.position = transform.position;
        bombImpact.transform.forward = collision.GetContact(0).normal;

        yield return new WaitForSeconds(bombDestroyTime);

        EJObjectPoolMgr.instance.ReturnbombImpactQueue(bombImpact);
        yield return null;
    }

}

