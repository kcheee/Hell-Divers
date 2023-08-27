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
        // 스케일 0으로 만들어 없는것 처럼 보이게
        transform.localScale = Vector3.zero;
        Destroy(this);
    }
    private void Update()
    {

    }
}
