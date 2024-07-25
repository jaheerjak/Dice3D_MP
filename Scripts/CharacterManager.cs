using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class CharacterManager : MonoBehaviour
{
    public int id;
    public int moveToPositionCount;
    public bool isWayPoint_Completed = false;
    public int waypointIndex;
    public int myScore;
    public int winOrder;
    public bool isWon;
    public bool isDisconnected;
    public bool isStarted;
    private void Start()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("idle");
    }
    private void Update()
    {
        if(PlayerManager.Instance.moveAllowed)
        {
            //if (PlayerManager.Instance.CurrentPlayerIndex!=id)
            //    transform.position = PlayerManager.Instance.gamePath.GetChild(waypointIndex).transform.position;

        }
    }
    [PunRPC]
    void UpdateCameraPos(Vector3 pos)
    {
        CamManager.Instance.playerCamera.transform.position = pos;
    }
    [PunRPC]
    void UpdateGamePathaPos(float pos,int index)
    {
        PlayerManager.Instance.gamePath.transform.GetChild(index).transform.DOMoveY(pos, 1f);
    }
}
