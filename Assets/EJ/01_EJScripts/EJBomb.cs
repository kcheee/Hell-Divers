using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBomb : MonoBehaviour
{
    Rigidbody rb;
    public GameObject bombImpactFactory;
    float speed;

    //궤적 Trail변수
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
        //계속 앞으로 가고 싶다.
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //이줄로 enqueue를 넣어주는 방법
            GameObject bombImpact = Instantiate(bombImpactFactory);

            bombImpact.transform.position = transform.position;
            bombImpact.transform.forward = collision.GetContact(0).normal;

            //normal을 써주는 방법

            //destroy가 dequeue?
            Destroy(bombImpact, 3);

            Destroy(gameObject);
            bombTrail.enabled = false;
        }
    }

    IEnumerator enqueueFX()
    {

            EJObjectPoolMgr.instance.InsertbombImpactQueue(bombImpactFactory);
            bombImpactFactory.transform.position = transform.position;
            bombImpactFactory.transform.forward = collision.GetContact(0).normal;

            yield return new WaitForSeconds(3f);

            EJObjectPoolMgr.instance.GetbombImpactQueue();

            yield return new WaitForSeconds(0.2f);
            
    }
}

