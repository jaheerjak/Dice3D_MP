using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

    private Sprite[] diceSides;
    private Image rend;
    private int whosTurn = 1;
    private bool coroutineAllowed = true;

    // Use this for initialization
    [SerializeField] private List<Vector3> diceFaces = new List<Vector3>();
	private void Start () {
        rend = GetComponent<Image>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
	}
    int randomDiceSide = 0;
    public void OnClickDice()
    {
        //if (!GameControl.gameOver && coroutineAllowed)
        randomDiceSide = Random.Range(0, 6);
        if ( coroutineAllowed)
            StartCoroutine("RollTheDice");

        Debug.Log("Start"+randomDiceSide);
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int rand = 0;
        for (int i = 0; i <= 20; i++)
        {
            rand = Random.Range(0, 6);
            rend.sprite = diceSides[rand];
            yield return new WaitForSeconds(0.05f);
        }

        /* GameControl.diceSideThrown = randomDiceSide + 1;
         if (whosTurn == 1)
         {
             GameControl.MovePlayer(1);
         } else if (whosTurn == -1)
         {
             GameControl.MovePlayer(2);
         }*/
        rend.sprite = diceSides[randomDiceSide];
        whosTurn *= -1;
        coroutineAllowed = true;
    }
}
