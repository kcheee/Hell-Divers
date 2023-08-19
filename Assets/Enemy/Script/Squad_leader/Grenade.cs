using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public GameObject smoke;
    public GameObject bombEft;
    //public GameObject BombParticle2;
    ParticleSystem p;

    private void Start()
    {
        // 메인캠에 달려있는 오브젝트 가져옴
        //cam.transform.GetComponent<DOTweenAnimation>().DOPlay();
        p = smoke.GetComponent<ParticleSystem>();

    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1);
            //smoke.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        Camera.main.transform.DOShakePosition(0.3f, 0.5f);

        Instantiate(bombEft,transform.position, Quaternion.identity);
        // 사운드 넣어야 함.
            Destroy(gameObject,0.25f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Floor")
        {
            StartCoroutine(delay());
        }
    }
}
