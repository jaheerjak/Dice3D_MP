using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DiceHandler : MonoBehaviour
{
    [SerializeField] TMP_Text playerTurnText;
    [SerializeField] GameObject tutorialInfo;
    public static DiceHandler instance;
    public Animator diceAnimator;
    private Vector2 initialPos;
    public TMP_Text scoreTxt;
    public TMP_Text predictPointTxt;

    [SerializeField] GameObject scoreBar;
    [SerializeField] GameObject predictButton;
    [SerializeField] GameObject predictPanel;
    [SerializeField] GameObject diceRollCountHolder;
    [SerializeField] TMP_Text diceCount;
    [SerializeField] Image fillImage;
    [SerializeField] TMP_Text fillCount;
    private void Awake()
    {
        instance = this;
        diceAnimator.enabled = false;
    }
    bool isPlayerSwipe = false;
    bool dicePlaying = false;
    bool isSwipeDone = false;
    bool isCurrentPlayer = false;
    int dcCount;
    int musicval;
    private void OnEnable()
    {
        if (PlayerManager.Instance)
        {
            Debug.Log("dddd " + PlayerManager.Instance.CurrentPlayerIndex);
            GameManager.Instance.WaitingCanvas.SetActive(false);
            Invoke(nameof(HideWait),0f);
            if (CommonData.musicVolume > 0.7)
            {
                CommonData.musicVolume = 0.4f;
                AudioManager.Instance.SetBgmVolume(0.4f);
            }
            isSwipeDone = false;
            diceRollCountHolder.SetActive(false);
            predictPanel.SetActive(false);
            dicePlaying = false;
            CommonData.isPredictClicked = false;
            diceAnimator.StopPlayback();
            predictButton.SetActive(true);
            GameManager.Instance.fogParicles.SetActive(false);
            
            if (UserDataManager.Instance)
            {
                    isCurrentPlayer = (PlayerManager.Instance.CurrentPlayerIndex == PhotonManager.Instance.playerId);
                if (UserDataManager.Instance.GameMode == GameMode.Player)
                    isCurrentPlayer = true;
                    isPlayerSwipe = true;
                predictButton.SetActive(isCurrentPlayer);
                tutorialInfo.SetActive(false);
                if (isCurrentPlayer)
                {
                    scoreTxt.text = "" + PlayerManager.Instance.CurrentPlayer.GetComponent<CharacterManager>().myScore;
                    tutorialInfo.SetActive(PlayerManager.Instance.waypointIndex <= 0);
                }

                CommonData.diceRandAnim = Random.Range(0, diceAnimList.Length);
                CommonData.diceVal1 = Random.Range(1, 4);
                CommonData.diceVal2 = Random.Range(1, 4);
                if (PhotonManager.Instance.playerId == PlayerManager.Instance.CurrentPlayerIndex)
                    dcCount++;
                if (dcCount >= 4)
                {
                    dcCount = 0;
                    CommonData.diceVal1 = 4;
                    CommonData.diceVal2 = 3;
                }
                if (UserDataManager.Instance.GameMode == GameMode.Computer && PlayerManager.Instance.CurrentPlayerIndex != 1)
                {
                    isPlayerSwipe = false;
                    playerTurnText.text = "Computer's Turn";                   
                }
                else
                {
                    playerTurnText.text = "Player " + PlayerManager.Instance.CurrentPlayerIndex + "'s Turn";                    
                    
                }
                if(UserDataManager.Instance.GameMode==GameMode.Player)
                {
                    scoreBar.SetActive(false);
                    predictButton.SetActive(false);
                }
            }
            if (GameManager.Instance.resetDiceScreen)
            {
                if (!gameObject.activeInHierarchy)
                GameManager.Instance.resetDiceScreen = false;
            }
            GameManager.Instance.diceHolder.SetActive(true);
            SwitchPlayerStatus();
        }
    }
    int delayCount = 10;
    void SwitchPlayerStatus()
    {
        delayCount = 10;
        if (UserDataManager.Instance.GameMode==GameMode.Computer && PlayerManager.Instance.CurrentPlayerIndex != 1)
        {
            fillImage.gameObject.SetActive(false);
        }
        else
        {
            fillImage.gameObject.SetActive(true);
            fillCount.text = "" + delayCount;
            if (delayRoutine != null)
                StopCoroutine(delayRoutine);
            delayRoutine =StartCoroutine(waitDelayDice());
        }
    }
    void HideWait()
    {
       
        GameManager.Instance.WaitingCanvas.SetActive(false);
    }
    Coroutine delayRoutine;
    IEnumerator waitDelayDice()
    {
       
        yield return new WaitUntil(()=> !GameManager.Instance.WaitingCanvas.activeInHierarchy);
        yield return new WaitForSeconds(1f);
        if(delayCount<=0)
        {
            if (delayRoutine != null)
                StopCoroutine(delayRoutine);
            PlayerManager.Instance.RefreshPlayerMove();
            
        }
        else
        {
            delayCount--;
            fillCount.text = "" + delayCount;
            delayRoutine = StartCoroutine(waitDelayDice());
        }
    }
    public void OnClick_predict()
    {
        predictPanel.SetActive(true);
        predictPointTxt.text = "+" + UserDataManager.Instance.GameConfigData._gameConfig.prediction_Points+"Points";
        CommonData.isPredictClicked = true;
    }
    
    public void OnClick_ODD_EVEn(int index)
    {
        predictPanel.SetActive(false);
        CommonData.predictIndex = index;
    }
    public float rotationSpeed=1f;
    public GameObject dice1;
    public GameObject dice2;

    void Update()
    {
        if (dicePlaying || predictPanel.activeInHierarchy || GameManager.Instance.WaitingCanvas.activeInHierarchy ) return;
        if(!isPlayerSwipe)
        {
            StartPlayDice();
            return;
        }
        if (isCurrentPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                initialPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                Calculate(Input.mousePosition);
            }
        }
       
    }
    void Calculate(Vector3 finalPos)
    {
        float disX = Mathf.Abs(initialPos.x - finalPos.x);
        float disY = Mathf.Abs(initialPos.y - finalPos.y);
        if (disX > 0 || disY > 0)
        {
            if (disX > disY)
            {
                if (initialPos.x > finalPos.x)
                {
                    Debug.Log("Left");
                }
                else
                {
                    Debug.Log("Right");
                }
            }
            else
            {
                if (initialPos.y > finalPos.y)
                {
                    Debug.Log("Down");
                }
                else
                {
                    Debug.Log("Up"+ disY);
                    if(disY>200f && !isSwipeDone)
                    {
                        isSwipeDone = true;
                        if (UserDataManager.Instance.GameMode == GameMode.FriendsMatch)
                        {
                            
                            PlayerManager.Instance.dicePlayCallBack(CommonData.diceRandAnim, CommonData.diceVal1, CommonData.diceVal2);
                        }
                        else
                            StartPlayDice();    

                    }
                }
            }
        }
    }
    [SerializeField] string[] diceAnimList;

    public void StartPlayDice()
    {
        
        dicePlaying = true;
        diceAnimator.enabled = true;
        tutorialInfo.SetActive(false);
        int rand = CommonData.diceRandAnim;
        if (rand >= diceAnimList.Length)
            rand = 0;
        if (rand == 0)
        {
            CamManager.Instance.diceCamera.GetComponent<Camera>().fieldOfView = 60f;
            AudioManager.Instance.PlayDiceSound1();
        }
        else
        {
            CamManager.Instance.diceCamera.GetComponent<Camera>().fieldOfView = 40f;
            AudioManager.Instance.PlayDiceSound();
        }
        
        diceAnimator.Play(diceAnimList[rand]);
        UIManager.Instance.OnClick_Play();
        /*dice1.DOMove(diceMoveUplPos.position, 0.6f);
        dice2.DOMove(diceMoveUplPos.position*2f, 0.6f).OnComplete(() =>
        {
            if (UserDataManager.Instance)
            {
                UIManager.Instance.OnClick_Play();
            }
            else
            {
                
                //dice2.GetComponent<DiceManager>().GetDiceData(2);
            }
        });*/
    }
    public void ShowCount()
    {
        diceRollCountHolder.SetActive(true);
        diceRollCountHolder.transform.localScale = Vector3.zero;
        diceRollCountHolder.transform.DOScale(Vector3.one, 0.2f);
        diceCount.text = "" + UIManager.Instance.diceVal;
    }
    public void OnSetFinalRollDice()
    {
        diceAnimator.enabled = false;
        
        dice1.GetComponent<DiceManager>().RollFinalAnim();
        dice2.GetComponent<DiceManager>().RollFinalAnim();
    }
    private void OnDisable()
    {
        isPlayerSwipe = false;
        if (delayRoutine != null)
            StopCoroutine(delayRoutine);
        delayRoutine = null;
        AudioManager.Instance.SetBgmVolume(CommonData.musicVolume);
        //AudioManager.Instance.StartBGM();
    }

}