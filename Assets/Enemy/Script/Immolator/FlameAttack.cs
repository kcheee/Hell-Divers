using DG.Tweening;
using DigitalRuby.PyroParticles;
using RootMotion.Demos;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlameAttack : MonoBehaviour
{
    public Transform player; // �÷��̾� ������Ʈ
    public float rotationAngle = 40f; // ȸ���� ���� (40��)
    public float rotationDuration = 2f; // ȸ���� �ð� (2��)
    public AudioSource audioSource;
    FireBaseScript FBS;
    static public FlameAttack instance;
    private void Awake()
    {
        instance = this;
        FBS = GetComponent<FireBaseScript>();
    }

    IEnumerator delay()
    {
        yield return null;
        FBS.FlameStart();

    }

    private void OnEnable()
    {

        // �ڽ��� ���� forward ���� ����
        Vector3 forwardDirection = transform.forward;

        // ���������� ȸ��
        RotateRight(forwardDirection);
        StartCoroutine(delay());
    }
    private void OnDisable()
    {
        // ���� 0���� ����.
        //transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // �ڽ��� �������� ���������� ȸ��
    private void RotateRight(Vector3 forwardDirection)
    {
        // 1�ʵ��� ���������� ���ٰ� �������� ��
        //Quaternion targetRotation = Quaternion.Euler(0, rotationAngle, 0) * Quaternion.LookRotation(forwardDirection);

        Vector3 targetRotation = new Vector3(0, rotationAngle, 0);
        Quaternion originpos = transform.localRotation;
        audioSource.Play();
        transform.DOLocalRotate(targetRotation, 1f).OnComplete(() =>
        {

            audioSource.Play();
            targetRotation = new Vector3(0, -rotationAngle, 0); /** Quaternion.LookRotation(forwardDirection)*/;
            transform.DOLocalRotate(targetRotation, 1f).OnComplete(() =>
            {

                FBS.FlameStop();
                transform.localRotation = originpos;

                // �ڱ��ڽ� false
                GetComponent<FlameAttack>().enabled = false;

            });
        });
    }

}
