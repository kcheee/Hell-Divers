using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake_Control : MonoBehaviour
{
   public static CameraShake_Control Instance;

    private void Awake()
    {
        Instance = this;
    }


}
