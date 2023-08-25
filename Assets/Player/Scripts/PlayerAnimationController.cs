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

    public void Reloading() {
        Player.currentGun.Reload();
        Player.reload = false;
    }



    public void fire() {
        Debug.Log("FIreGrenade!");
        Player.FireGrenade();
    }
    public void Grenade() {
        Player.PlayerInfoUI.gameObject.SetActive(false);
        testGun.SetActive(false);
    
    }

    public void GrenadeOff()
    {
        testGun.SetActive(true);
        Player.current_stratagem = null;
    }
}
