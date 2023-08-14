using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJWheel : MonoBehaviour
{
    public MeshRenderer wheelMeshR;
    public MeshRenderer wheelMeshL;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wheelMeshR.material.mainTextureOffset += new Vector2(0, -0.006f);
        wheelMeshL.material.mainTextureOffset += new Vector2(0, -0.006f);
    }
}
