using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Billboard : MonoBehaviour
{

    public Transform mainCameraTransform; // ���� ī�޶��� Transform ������Ʈ

    private void Start()
    {
        //mainCameraTransform = Camera.main.transform; // ���� ī�޶��� Transform�� ������

    }

    private void LateUpdate()
    {
        //// UI ������Ʈ�� ���� ��ǥ�� ���
        //Vector3 worldPosition = transform.position;

        //if (GetComponent<RectTransform>() != null)
        //{
        //    worldPosition = GetComponent<RectTransform>().position;
        //}

        //// ī�޶� ������ ���ϴ� ȸ�� ���� ���
        //Quaternion lookRotation = Quaternion.LookRotation(mainCameraTransform.forward, Vector3.up);

        //// UI ������Ʈ�� ȸ���� ������Ʈ�Ͽ� ������ ȿ���� ����
        //transform.rotation = lookRotation;
        //lookPos = new Vector3 (0, lookPos.y-90, 0);

        //Vector3 lookPos = mainCameraTransform.position;
        //transform.DOLookAt(lookPos, 1);

        //transform.LookAt(transform.position + mainCameraTransform.transform.rotation * Vector3.forward,
        //         mainCameraTransform.transform.rotation * Vector3.up);

        Vector3 lookPos = mainCameraTransform.position;
        // x�� z ���� ȸ�� ���� 0���� ������Ŵ
        lookPos.x = transform.position.x;
        lookPos.z = transform.position.z;

        // ȸ�� �ִϸ��̼��� �����Ͽ� UI ��Ҹ� ī�޶� �������� ȸ����Ŵ
        transform.DOLookAt(lookPos, 1);
    }
}
