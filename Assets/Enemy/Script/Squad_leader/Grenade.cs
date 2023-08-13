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
        // ����ķ�� �޷��ִ� ������Ʈ ������
        //cam.transform.GetComponent<DOTweenAnimation>().DOPlay();
        p = smoke.GetComponent<ParticleSystem>();

    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1);
            smoke.SetActive(false);
        yield return new WaitForSeconds(1.5f);
            Instantiate(bombEft,transform.position, Quaternion.identity);
        // ���� �־�� ��.
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Plane")
        {
            StartCoroutine(delay());
        }
    }
}
