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
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //02.trailrenderer를 부모자식 관계를 끊고
            var trail = transform.Find("trail");
            trail.parent = null;

            //03.일정 시간 후에 bombTrail을 없앤다.
            Destroy(trail.gameObject, 3);
            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject); 
            
            //04.coroutine만을 위한 빈 오브젝트를 만들어서 GameObject가 꺼진 후에도 작동하도록 한다.
            EJGlobalCoroutine.instance.StartCoroutine(bombImpactMake(collision));          
        }
    }

    //collision이나 vector3를 매개변수로 넣어주는 함수를 넣어주면 된다.
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

