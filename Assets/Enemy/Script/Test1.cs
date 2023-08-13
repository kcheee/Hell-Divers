using UnityEngine;
using DG.Tweening;
using System.Collections;
using Mono.Cecil.Cil;
using RootMotion.Demos;
using DigitalRuby.PyroParticles;

public class Test1 : MonoBehaviour
{

    Quaternion vec;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            RotateRight();
        }
    }

    private void RotateRight(/*Vector3 forwardDirection*/)
    {
        // 1초동안 오른쪽으로 갔다가 왼쪽으로 감
        //Quaternion targetRotation = Quaternion.Euler(0, rotationAngle, 0) * Quaternion.LookRotation(forwardDirection);

        Vector3 targetRotation = new Vector3(0, 40, 0);
        Quaternion originpos = transform.localRotation;
        vec = transform.localRotation;
        transform.DOLocalRotate(targetRotation, 1f).OnComplete(() =>
        {
            Debug.Log(transform.rotation);

            targetRotation = new Vector3(0, -40, 0); /** Quaternion.LookRotation(forwardDirection)*/;
            transform.DOLocalRotate(targetRotation, 1f).OnComplete(() =>
            {
                Debug.Log(transform.rotation);
                FireBaseScript.instance.FlameStop();
                Vector3 targetRotation = new Vector3(0, 40, 0);

                transform.localEulerAngles = new Vector3(0, 0, 0);
                Debug.Log(transform.rotation);

                // 자기자신 false
                GetComponent<FlameAttack>().enabled = false;

            }); ;
        });
    }
    private void OnDisable()
    {
                transform.localRotation = vec;

    }
}
