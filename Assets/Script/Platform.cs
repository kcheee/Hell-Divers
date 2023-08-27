using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Platform : MonoBehaviour
{
    public GameObject particle1;
    public GameObject particle2;
    AudioSource audiosource;

    

    //player add
    public GameObject Item;
    public System.Action action;

    public AudioClip myClip;

    IEnumerator move()
    {
        SoundManager.instance.SfxPlay(myClip);
        //audiosource.Play();
        //yield return new WaitForSeconds(1.8f);
        yield return null;
        transform.DOMove(new Vector3(transform.position.x, 0.5f, transform.position.z), 2.3f).SetEase(Ease.InCirc).OnComplete(() => {
            // 실행시킬 내용 
            

            //player add
            if (action != null) {
                Debug.Log("action");
                action();
            }

            fun();

            if (Item && PhotonNetwork.IsMasterClient)
            {
                //이것으로 같은 위치에 생성됨을 보장한다.
                PhotonNetwork.Instantiate("Item",transform.position + Vector3.up , Quaternion.identity);

            }
            //Instantiate(Item, transform.position, Quaternion.identity);
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
