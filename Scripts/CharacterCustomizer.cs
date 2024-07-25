using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
    // Start is called before the first frame update
    public int CurrentPlayerID = 1;
    [SerializeField] private Transform playerParent;
    [SerializeField] private RuntimeAnimatorController animatorController;
    void Start()
    {
        //SetUpCharacter(CurrentPlayerID);
    }
    [ContextMenu("ChangePlayer")]
    void ChangePlayer()
    {
        SetUpCharacter(CurrentPlayerID);
        CurrentPlayerID++;
    }
    public void SetUpCharacter(int playerID)
    {
        Debug.Log("came " + playerID);
        GameObject _go = Resources.Load("Players/Player"+ playerID) as GameObject;
        
        Destroy(playerParent.transform.GetChild(0).gameObject);        
        
        GameObject player= Instantiate(_go, playerParent);
        player.AddComponent<Animator>().runtimeAnimatorController=animatorController;
        Debug.Log("came 2" + playerID);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
