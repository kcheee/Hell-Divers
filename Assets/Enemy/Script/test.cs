using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform player; // �÷��̾� ������Ʈ
    public float rotationAngle = 40f; // ȸ���� ���� (40��)
    public float rotationDuration = 2f; // ȸ���� �ð� (2��)

    private void OnEnable()
    {

        // �ڽ��� ���� forward ���� ����
        Vector3 forwardDirection = transform.forward;

        // ���������� ȸ��
        RotateRight(forwardDirection);
    }
    private void OnDisable()
    {
        // ���� 0���� ����.
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    // �ڽ��� �������� ���������� ȸ��
    private void RotateRight(Vector3 forwardDirection)
    {
        // 1�ʵ��� ���������� ���ٰ� �������� ��
        Quaternion targetRotation = Quaternion.Euler(0, rotationAngle, 0) * Quaternion.LookRotation(forwardDirection);
        transform.DORotateQuaternion(targetRotation, 1f).OnComplete(() =>
        {
            targetRotation = Quaternion.Euler(0, -rotationAngle, 0) * Quaternion.LookRotation(forwardDirection);
            transform.DORotateQuaternion(targetRotation, 1f);
        });
    }

}
