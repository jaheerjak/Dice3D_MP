using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceCount : MonoBehaviour
{
    [SerializeField] Image numImg;
    public void UpdateSprite(Sprite spriteImg)
    {
        gameObject.SetActive(true);
        numImg.sprite = spriteImg;
    }
    public void HideObj()
    {
        gameObject.SetActive(false);
    }
}
