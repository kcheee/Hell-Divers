using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBomb : MonoBehaviour
{
    //bomb 변수
    Rigidbody rb;
    float bombSpeed;
    GameObject bomExploImpact;
    float bombDestroyTime = 0.2f;
    float bombRadius = 2;

    //궤적 Trail변수
    public TrailRenderer bombTrail;

    // Start is called before the first frame update
    void Start()
    {
        bombTrail = GetComponent<TrailRenderer>();
        bombTrail.enabled = true;

        //bombFire Speed, Angle Random하게
        bombSpeed = Random.Range(10,30);
      
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //땅과 부딪히면 bomb 없애기
            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject); 
            
            //04.coroutine만을 위한 빈 오브젝트를 만들어서 GameObject가 꺼진 후에도 작동하도록 한다.
            EJGlobalCoroutine.instance.StartCoroutine(bombExplode(collision));          
        }
    }

    //bomb 잔상이 켜졌다 꺼지는 함수
    IEnumerator bombExplode (Collision collision)
    {
        bomExploImpact = EJObjectPoolMgr.instance.GetbombExploImpactQueue();

        bomExploImpact.transform.position = transform.position;
        bomExploImpact.transform.localScale = Vector3.one * 10;
        bomExploImpact.transform.forward = collision.GetContact(0).normal;

        yield return new WaitForSeconds(bombDestroyTime);

        //bomb반경 안의 player damage // 지금 안되고 있음
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        foreach (RaycastHit hitObj in bombHits)
        {
            hitObj.transform.GetComponent<EJPlayerHPforTest>().DamageHP(3);
        }

        EJObjectPoolMgr.instance.ReturnbombExploImpactQueue(bomExploImpact);
        yield return null;
    }

}

