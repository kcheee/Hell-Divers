using UnityEngine;
using DG.Tweening;
using System.Collections;
using Mono.Cecil.Cil;

public class Test1 : MonoBehaviour
{
    public GameObject bullet;
    private Vector3 startPos;
    public Vector3 endPos;
    public float g;
    public float max_height;



    private void Start()
    {
        GameObject BULLET = Instantiate(bullet,transform.position,Quaternion.identity);

        // ��ź �߻�.
    }



    private void OnProjectileSimulationComplete()
    {
        Debug.Log("������ � �Ϸ�");
    }
}
