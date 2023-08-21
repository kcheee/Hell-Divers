using DG.Tweening;
using DigitalRuby.PyroParticles;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class test : MonoBehaviourPun
{

    public GameObject Granade;

    [PunRPC]
    void pun_FireGrenada()
    {
        StartCoroutine(FireGrenada());
    }
    IEnumerator FireGrenada()
    {
        yield return null;
        GameObject Grenada = Instantiate(Granade,transform.position, Quaternion.identity);
        Grenada.GetComponent<GranadeLauncher>().value(transform.position+new Vector3(10,0,10));
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            photonView.RPC(nameof(pun_FireGrenada),RpcTarget.All);
        }
    }

}
