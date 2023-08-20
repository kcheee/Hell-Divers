using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnerTest : MonoBehaviour
{
    public GameObject Entity;
    public static SpawnerTest instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Spawn(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(float t) {
        StartCoroutine(SpawnTime(t, ()=> {
            PhotonNetwork.Instantiate("dum", transform.position, transform.rotation);
        }));
        

    }

    IEnumerator SpawnTime(float t, System.Action action) {
        yield return new WaitForSeconds(t);
        action();
        
    }
}
