using UnityEngine;
using DG.Tweening;
using System.Collections;
using Mono.Cecil.Cil;
using RootMotion.Demos;

public class Test1 : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Plane")
        {

        Debug.Log(transform.position);
        }
    }

}
