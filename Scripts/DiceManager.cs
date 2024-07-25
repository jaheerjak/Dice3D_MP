using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DiceManager : MonoBehaviour
{

    [SerializeField] private int numberOfSides_in_Dice=4;
    [SerializeField] private float timeToRotate;
    [SerializeField] private float numberOfRotation;
    [SerializeField] private float CIRCLE=360f;
    [SerializeField] private float angleofOneSide;
    [SerializeField] private float currentTime;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private List<float> diceFaces = new List<float>();
    public int id;
    int getRandomSide = 0;
    Vector3 diceStartPos;
    Vector3 diceAnglePos;
    private void Start()
    {
        
        diceStartPos = transform.localPosition;
        diceAnglePos = transform.localEulerAngles;
    }
    private void OnReset()
    {
       
        transform.localPosition= diceStartPos;
        transform.localEulerAngles= diceAnglePos;
    }
    [ContextMenu("PlayDice")]
    public void TestPlayDIce()
    {
        GetDiceData(id);
    }
    Tweener rotationTweener1;
    Tweener rotationTweener;
    float rotationDuration = 2f;
    int dcCount = 0;
    int ccCount = 0;
    public void GetDiceData(int idNo)
    {
        angleofOneSide = CIRCLE / numberOfSides_in_Dice;
        transform.localPosition = diceStartPos;
        transform.localEulerAngles = diceAnglePos;
        id = idNo;
        if (!PlayerManager.Instance.isPlayerStarted() || PlayerManager.Instance.CurrentPlayerPos()>=99)
        {
            ccCount++;
            if (ccCount >= 4)
            {
                ccCount = 0;
                if (id == 1)
                    CommonData.diceVal1 = 3;
                else
                    CommonData.diceVal2 = 4;
            }
        }
        if (id==1)
        getRandomSide = CommonData.diceVal1;
       else
        getRandomSide = CommonData.diceVal2;
        
        UIManager.Instance.getDiceCount(getRandomSide);

    }
    IEnumerator PlayDiceAnim()
    {
        float startangle = this.transform.eulerAngles.x;
        currentTime = 0;
        
        
        float getAngle = (numberOfRotation * CIRCLE) * angleofOneSide * getRandomSide-startangle;
        getAngle = -90*getRandomSide;
        while(currentTime<timeToRotate)
        {
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
            float angleCurrent = getAngle * curve.Evaluate(currentTime / timeToRotate);
            this.transform.eulerAngles = new Vector3(angleCurrent , 0, 90);
        }
        Debug.Log("angle dice is " + this.transform.eulerAngles.x);
        if (currentTime>timeToRotate)
        {
            CommonData.isDicePlayed = true;
            yield return new WaitForSeconds(1f);
            //OnReset();
            UIManager.Instance.ActiveGameArea(true);
            CommonData.isDicePlayed = false;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Grounde")
        {
           
        }
    }
    public float targetAngle = 90.0f; // The angle to stop at after 4 rotations
    public float rotDuration = 0.3f; // Duration of each rotation
    public int rotationCount = 4; // Number of rotations before stopping

    private int currentRotationCount = 0;
    public void RollFinalAnim()
    {
        
        /*bool isPlaying = DiceHandler.instance.diceAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;

        // If isPlaying is true, it means the Animator is currently playing an animation
        if (isPlaying)
        {
            DiceHandler.instance.diceAnimator.StopPlayback();
        }*/
        /*animator.enabled = true;
        if(id==1)
            animator.Play("dhayam");
        else if (id == 2)
            animator.Play("dhayam2");*/
        currentRotationCount = 0;
        CommonData.diceCount = 0;
        RotateObject();
        //StartCoroutine(PlayDiceAnim());
    }
  
    void RotateObject()
    {
        
        float finalAngle = (currentRotationCount) * 90;
        if (currentRotationCount >= 3)
            finalAngle = 90 * getRandomSide;
        Quaternion targetRotation;
        targetRotation = Quaternion.Euler(new Vector3(finalAngle, 0.0f, 90f));

        if (currentRotationCount < rotationCount)
        {
            // Calculate the target rotation based on the current rotation count
            Debug.Log("Check dice val " + currentRotationCount + ":::" + finalAngle);

            // Use DoTween to smoothly rotate the object to the target angle
            this.transform.DORotateQuaternion(targetRotation,rotDuration)
                .OnComplete(() =>
                {
                    currentRotationCount++;
                    RotateObject(); // Call the function recursively for the next rotation
                });
        }
        else
        {
            CommonData.isDicePlayed = true;
            CommonData.diceCount++;
            if (CommonData.diceCount >= 2)
            {
                if(CommonData.isPredictClicked)
                {
                    if((UIManager.Instance.diceVal%2==0 && CommonData.predictIndex==2) || (UIManager.Instance.diceVal % 2 != 0 && CommonData.predictIndex == 1))
                    {
                        PlayerManager.Instance.CurrentPlayer.GetComponent<CharacterManager>().myScore += UserDataManager.Instance.GameConfigData._gameConfig.prediction_Points;
                    }
                    if (PlayerManager.Instance.CurrentPlayerIndex == PhotonManager.Instance.playerId)
                        DiceHandler.instance.scoreTxt.text = "" + PlayerManager.Instance.CurrentPlayer.GetComponent<CharacterManager>().myScore;
                    PlayerManager.Instance.UpdateScore();
                }
                DiceHandler.instance.ShowCount();
                Invoke(nameof(DelayActivegame), 1.2f);
                
            }
        }
    }
    void DelayActivegame()
    {
        //OnReset();
        UIManager.Instance.ActiveGameArea(true);


        CommonData.isDicePlayed = false;
    }
}
