using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update


    public MeshRenderer t;

    Vector2 offset;
    Vector2 offset1;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            t.material.mainTextureOffset += new Vector2(0, -0.006f);
        }
        if (Input.GetKey(KeyCode.B))
        {

        }
        if (Input.GetKey(KeyCode.C))
        {

        }
    }
}
