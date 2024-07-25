using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.diceHolder.SetActive(false);
    }

    // Update is called once per frame
    public void OnCameraCompleted()
    {
        CamManager.Instance.OnCameraCompleted();
    }
}
