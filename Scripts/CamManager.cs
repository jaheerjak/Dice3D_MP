using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering.PostProcessing;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class CamManager : MonoBehaviour
{
    public static CamManager Instance;
    [SerializeField] public GameObject diceCamera;
    [SerializeField] public GameObject playerCamera;
    [SerializeField] public GameObject resultCamera;
    [SerializeField] public GameObject EnvironmentCamera;
    [SerializeField] private Color[] postProcessColor;
    [SerializeField] private int[] postProcessTemp;
    private ColorGrading colorGrading;


    public float moveSpeed = 2f;
    private void Awake()
    {
        Instance = this;
        diceCamera.SetActive(false);
        EnvironmentCamera.SetActive(true);
        playerCamera.SetActive(false);
        resultCamera.SetActive(false);
        //ChangeColorGrading();
        playerCamera.GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGrading);
        //EnvironmentCamera.GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGrading);
        ChangeColorGrading();
    }
    public void ChangeColorGrading()
    {
        colorGrading.temperature.value = postProcessTemp[CommonData.CurrentYugamId - 1];
        //colorGrading.tint.value = tint;
        //colorGrading.contrast.value = contrast;
        colorGrading.colorFilter.value = postProcessColor[CommonData.CurrentYugamId - 1];
    }
    public void GetResultCamera()
    {
        diceCamera.SetActive(false);
        playerCamera.SetActive(false);
        resultCamera.SetActive(true);
        GameManager.Instance.fogParicles.SetActive(false);
    }
    bool isPlayerSeen = false;
    public float playerDistance;
    public void LateUpdate()
    {
        /*if(GameManager.Instance.isIntroDone==false)
        {

            if (isPlayerSeen == false)
            {
                float dist = Vector3.Distance(EnvironmentCamera.transform.position, PlayerManager.Instance.CurrentPlayer.transform.position);
                if (playerDistance > dist)
                {
                    isPlayerSeen = true;
                }
                else
                {
                    EnvironmentCamera.transform.position = Vector3.MoveTowards(EnvironmentCamera.transform.position,
                       PlayerManager.Instance.CurrentPlayer.transform.position,
                       moveSpeed * Time.deltaTime);
                }
                
            }
            if (isPlayerSeen)
            {
                EnvironmentCamera.transform.position = Vector3.MoveTowards(EnvironmentCamera.transform.position,
                   introCameraTarget.transform.position,
                   moveSpeed * Time.deltaTime);

                if (EnvironmentCamera.transform.position == introCameraTarget.transform.position)
                {
                    EnvironmentCamera.SetActive(false);
                    if (PlayerManager.Instance.CurrentPlayerIndex == PhotonManager.Instance.playerId)
                    {
                        diceCamera.SetActive(true);
                        
                        GameManager.Instance.diceMenuUI.SetActive(true);
                        GameManager.Instance.isIntroDone = true;
                    }
                    else
                    {
                        playerCamera.SetActive(true);
                    }
                }
            }
        }*/
    }
    Coroutine coroutine;
    public void OnCameraCompleted()
    {
        GameManager.Instance.isIntroDone = true;
        if(UserDataManager.Instance.GameMode!= GameMode.FriendsMatch)
        {
            PhotonManager.Instance.playerId = 1;
            SetGameStart();
        }
        else
        {
            Hashtable initialProps = new Hashtable();
            initialProps["Player_Joined"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            GameManager.Instance.waitingScreen.SetActive(true);
            coroutine = StartCoroutine(CanGameStart());
        }
        
    }

    IEnumerator CanGameStart()
    {
        yield return new WaitUntil(() => AllPlayersJoined());
        SetGameStart();
    }
    bool AllPlayersJoined()
    {
        int count = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {            
            Hashtable playerCustomProps = player.CustomProperties;
            // Now you can access and use the custom properties of the other player
            // For example, to get a specific custom property:
            if (playerCustomProps.ContainsKey("Player_Joined"))
            {
                count++;
            }
        }
        if(count>=PhotonNetwork.PlayerList.Length)
        {
            return true;
        }
        return false;
    }
    private void SetGameStart()
    {
        GameManager.Instance.waitingScreen.SetActive(false);
        EnvironmentCamera.SetActive(false);
        diceCamera.SetActive(true);
        GameManager.Instance.gameEnvironment.SetActive(false);
        GameManager.Instance.diceMenuUI.SetActive(true);
        /*
         if (PlayerManager.Instance.CurrentPlayerIndex == PhotonManager.Instance.playerId)
        {
            diceCamera.SetActive(true);
            GameManager.Instance.gameEnvironment.SetActive(false);
            GameManager.Instance.diceMenuUI.SetActive(true);

        }
        else
        {
            playerCamera.SetActive(true);
        }*/
    }
    public void ChangeFieldOfView(CameraNames camera, int index = 0)
    {
        if(camera==CameraNames.Dice)
        {
            if(index==0)
            {

            }
        }
    }
}
public enum CameraNames
{
    Dice,
    Player,
    Environment
}
