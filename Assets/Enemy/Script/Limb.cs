using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    [SerializeField] Limb[] childLimbs;

    public void GetHit()
    {

        if (childLimbs.Length > 0)
        {
            foreach (Limb limb in childLimbs)
            {
                if (limb != null)
                    limb.GetHit();
            }
        }
        // ������ 0���� ����� ���°� ó�� ���̰�
        transform.localScale = Vector3.zero;
        Destroy(this);
    }
    private void Update()
    {

    }
}
