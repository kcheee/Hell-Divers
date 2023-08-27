using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    PlayerTest1 Player;
    public GameObject testGun;
    // Start is called before the first frame update
    void Start()
    {
        Player = transform.parent.GetComponent<PlayerTest1>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //��� pC���� �����.
    public void Reloading() {
        Debug.LogError("���� �ǽÿ��� ����");
        Player.currentGun.Reload();
        Player.reload = false;
        /*        if (PhotonNetwork.IsMasterClient) {
                    Player.currentGun.Reload();
                    Player.reload = false;
                    Player.photonView.RPC("manganizeRPC",RpcTarget.All, Player.currentGun.Current_Manganize);
                    Player.currentGun.photonView.RPC("Set_Bullet", RpcTarget.All, Player.currentGun.currentBullet);
                }*/

    }



    public void fire() {
        Debug.Log("FIreGrenade!");
        Player.FireGrenade();
    }
    public void Grenade() {
        
        testGun.SetActive(false);
    
    }

    public void GrenadeOff()
    {
        testGun.SetActive(true);
        Player.current_stratagem = null;
    }
}
