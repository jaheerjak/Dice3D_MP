using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAnimation : MonoBehaviour
{
    public void OnDiceAnimComp()
    {
        CamManager.Instance.diceCamera.GetComponent<Camera>().fieldOfView = 60f;
        DiceHandler.instance.OnSetFinalRollDice();
    }
}
