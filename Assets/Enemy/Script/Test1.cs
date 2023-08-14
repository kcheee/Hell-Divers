using UnityEngine;
using DG.Tweening;
using System.Collections;
using Mono.Cecil.Cil;
using RootMotion.Demos;

public class Test1 : MonoBehaviour
{
    MeshRenderer mesh;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Plane")
        {

        Debug.Log(transform.position);
        }
    }


    private void Update()
    {
        if(Input.GetKey(KeyCode.M))
        {
            mesh.material.mainTextureOffset += new Vector2(0,0.5f);
        }
    }

}
