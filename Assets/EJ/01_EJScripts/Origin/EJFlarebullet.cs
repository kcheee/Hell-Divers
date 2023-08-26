using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJFlarebullet : MonoBehaviourPun
{
    #region flarebullet변수
    float flareBulletSpeed = 50f;
    float destroyTime = 1f;

    public GameObject floorEffectFactroy;

    public PhotonView tankPv;

    Rigidbody rb;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf());

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * flareBulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            //floor에 닿았을 때 생기는 효과 왜 안생김?
            GameObject floorEffect = Instantiate(floorEffectFactroy);
            floorEffect.transform.position = transform.position;
            floorEffect.transform.forward = other.transform.up;
            floorEffect.transform.localScale = Vector3.one * 2;

            StartCoroutine(DestroySelf4Trigger(other));
        }

        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, transform.position /*+ Vector3.up * 1.6f*/, 3);
        }
    }


    IEnumerator DestroySelf()
    {
        Destroy(gameObject, 2.5f);
        yield return null;
    }


    IEnumerator DestroySelf4Trigger(Collider other)
    {
        tankPv.RPC("ShowGausCannonImpact", RpcTarget.All, transform.position,other.transform.up);

        //HP테스트 필요
        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, other, 3);
        }      

        flareBulletSpeed = 0;
        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }
}
