using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJLineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

