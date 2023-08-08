using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //자신의 앞방향으로 날아가는 성질
        transform.position += transform.forward * 10 * Time.deltaTime;
    }
}
