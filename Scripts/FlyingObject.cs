using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    [SerializeField] Material[] yugamFaceMaterial;
    void Start()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material = yugamFaceMaterial[CommonData.CurrentYugamId-1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
