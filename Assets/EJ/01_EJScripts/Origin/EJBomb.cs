using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

//Bomb의 충돌처리만 담당해주는 스크립트?

public class EJBomb : MonoBehaviourPun
{
    //bomb 변수
    public Rigidbody rb;
    float bombSpeed;
    GameObject bomExploImpact;
    float bombDestroyTime = 0.2f;
    float bombRadius = 2;

    public PhotonView tankPv;

    //궤적 Trail변수
    //public TrailRenderer bombTrail;

    GameObject player;

    //회전값
    Vector3 rot;
    float bombPower;

    // Start is called before the first frame update
    void Start()
    {
        bombPower = Random.Range(14f, 20f);
        rb.velocity = Vector3.zero;
        //bombTrail = GetComponent<TrailRenderer>();
        //bombTrail.enabled = true;

        //bombFire Speed, Angle Random하게
        //bombSpeed = Random.Range(5,10);

        //rot = transform.eulerAngles;
        //rot.x += Random.Range(-5, 5);
        //rot.y += Random.Range(-5, 5);
        //rot.z += Random.Range(-5, 5);
        //transform.eulerAngles = rot;

        //이걸 넘겨줘야 하는 게 fire?가 
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = Vector3.zero;
        //rb.AddForce(transform.forward * bombPower, ForceMode.Impulse);
    }

    public void BulletFire()
    {
        //이걸 넘겨줘야 하는 게 fire?가 
        rb = GetComponent<Rigidbody>();
        //rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * bombPower, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.up * bombSpeed * Time.deltaTime;
    }

    /*    private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                transform.position = other.transform.position;
                //땅과 부딪히면 bomb 없애기
                EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);

                //04.coroutine만을 위한 빈 오브젝트를 만들어서 GameObject가 꺼진 후에도 작동하도록 한다.
                EJGlobalCoroutine.instance.StartCoroutine(bombExplode(other));
            }
        }*/

    #region collision 함수

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if(PhotonNetwork.IsMasterClient)
            {
                //04.coroutine만을 위한 빈 오브젝트를 만들어서 GameObject가 꺼진 후에도 작동하도록 한다.
                StartCoroutine(bombExplode(collision));
            }

            //땅과 부딪히면 bomb 없애기
            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);
            EJBossSFX.instance.PlaybombExploSFX();
        }

        
    }



    //bomb 잔상이 켜졌다 꺼지는 함수
    IEnumerator bombExplode (Collision collision)
    {
        tankPv.RPC("ShowBombExploImpact", RpcTarget.All, transform.position, collision.GetContact(0).normal, bombDestroyTime);
        //bomExploImpact = EJObjectPoolMgr.instance.GetbombExploImpactQueue();

        //bomExploImpact.transform.position = transform.position;
        //bomExploImpact.transform.localScale = Vector3.one *3;
        //bomExploImpact.transform.forward = collision.GetContact(0).normal;

        yield return new WaitForSeconds(bombDestroyTime);

        //PlayerRPC로 바꿔주는 것
        

        //bomb반경 안의 player damage // 지금 안되고 있음
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));


        //!!!!!!!!!!Player가 맞았다 판정해서 구안에 들어오면 데미지를 받아야 한다. 
       // player.GetComponent<PlayerTest1>().photonView = photonView;

        foreach (RaycastHit hitObj in bombHits)
        {
            //pho
            //hitObj.transform.GetComponent<EJPlayerHPforTest>().DamageHP(3);
            hitObj.transform.GetComponent<PhotonView>().RPC("damaged",RpcTarget.All,hitObj.point, 3);
        }

        //EJObjectPoolMgr.instance.ReturnbombExploImpactQueue(bomExploImpact);
        yield return null;

        //BossFSM.Sflag = false;
    }

    #endregion


    #region Trigger함수 


    private void OnTriggerEnter(Collider other)
    {
        print("bomb터짐 범위 안에 들어온 것은" + other.gameObject);

        if (other.gameObject.tag == "Floor")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(bombExplodebyTrigger(other));
            }

            EJObjectPoolMgr.instance.ReturnbombQueue(transform.gameObject);
            EJBossSFX.instance.PlaybombExploSFX();
        }

        if (other.gameObject.tag == "Player")
        {
            PlayerDamage();
        }
    }

    //bomb 잔상이 켜졌다 꺼지는 함수
    IEnumerator bombExplodebyTrigger(Collider other)
    {
        tankPv.RPC("ShowBombExploImpact", RpcTarget.All, transform.position, other.transform, bombDestroyTime);
        //bomExploImpact = EJObjectPoolMgr.instance.GetbombExploImpactQueue();

        //bomExploImpact.transform.position = transform.position;
        //bomExploImpact.transform.localScale = Vector3.one *3;
        //bomExploImpact.transform.forward = collision.GetContact(0).normal;

        yield return new WaitForSeconds(bombDestroyTime);

        //PlayerRPC로 바꿔주는 것


    }

    void PlayerDamage()
    {

        //bomb반경 안의 player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));


        print("bomb에 맞은 것은 "+ bombHits[0].transform.gameObject.name);
        


        //!!!!!!!!!!Player가 맞았다 판정해서 구안에 들어오면 데미지를 받아야 한다. 
        // player.GetComponent<PlayerTest1>().photonView = photonView;

        foreach (RaycastHit hitObj in bombHits)
        {
            //pho
            //hitObj.transform.GetComponent<EJPlayerHPforTest>().DamageHP(3);
            hitObj.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, hitObj.point, 3);
        }

        //EJObjectPoolMgr.instance.ReturnbombExploImpactQueue(bomExploImpact);
        //yield return null;

        //BossFSM.Sflag = false;
    }
    #endregion



}

