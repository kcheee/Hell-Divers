using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTest : MonoBehaviour
{
    public GameObject Entity;
    public static SpawnerTest instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(float t) {
        StartCoroutine(SpawnTime(t, ()=> {
            Instantiate(Entity, transform.position, transform.rotation);
        }));
        

    }

    IEnumerator SpawnTime(float t, System.Action action) {
        yield return new WaitForSeconds(t);
        action();
        
    }
}
