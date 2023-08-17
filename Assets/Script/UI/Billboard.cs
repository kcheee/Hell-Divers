using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Transform mainCameraTransform; // ���� ī�޶��� Transform ������Ʈ

    private void Start()
    {
        //mainCameraTransform = Camera.main.transform; // ���� ī�޶��� Transform�� ������

    }

    private void LateUpdate()
    {
        // UI ������Ʈ�� ���� ��ǥ�� ���
        Vector3 worldPosition = transform.position;

        if (GetComponent<RectTransform>() != null)
        {
            worldPosition = GetComponent<RectTransform>().position;
        }

        // ī�޶� ������ ���ϴ� ȸ�� ���� ���
        Quaternion lookRotation = Quaternion.LookRotation(mainCameraTransform.forward, Vector3.up);

        // UI ������Ʈ�� ȸ���� ������Ʈ�Ͽ� ������ ȿ���� ����
        transform.rotation = lookRotation;
    }
}
