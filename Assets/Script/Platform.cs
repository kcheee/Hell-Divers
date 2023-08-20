using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject particle1;
    public GameObject particle2;
    AudioSource audiosource;

    IEnumerator move()
    {

        audiosource.Play();
        //yield return new WaitForSeconds(1.8f);
        yield return null;
        transform.DOMove(new Vector3(transform.position.x, 0.5f, transform.position.z), 2.3f).SetEase(Ease.InCirc).OnComplete(() => {
            // 실행시킬 내용 
            
            fun();

        });
    }

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        StartCoroutine(move());
    }
    void fun()
    {
        Camera.main.transform.DOShakePosition(0.5f);
        Instantiate(particle1, transform.position, Quaternion.identity);
        Instantiate(particle2, transform.position, Quaternion.identity);
    }
    public void T()
    {
        Debug.Log("tlfgod");
    }
}
