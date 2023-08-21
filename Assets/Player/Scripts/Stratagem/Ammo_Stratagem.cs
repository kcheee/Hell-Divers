using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Stratagem : Stratagems
{
    // Start is called before the first frame update
    void Start()
    {
        start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void SpawnAction(Vector3 pos,Quaternion rot)
    {
        base.SpawnAction(pos,rot);
        GameObject platObj = Instantiate(Platform, pos + Vector3.up * 20, Quaternion.Euler(90,0,0));
        Platform platform = platObj.GetComponent<Platform>();
        platform.Item = this.Item;

    }
}
