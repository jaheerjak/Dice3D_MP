using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanel : MonoBehaviour
{
    public void OnClick_Yes()
    {
        AudioManager.Instance.PlayClickSound();
        Application.Quit();
    }
    public void OnClick_No()
    {
        AudioManager.Instance.PlayClickSound();
        gameObject.SetActive(false);
    }
}
