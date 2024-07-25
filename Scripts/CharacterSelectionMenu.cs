using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionMenu : MonoBehaviour
{
    [SerializeField] CharacterCustomizer CharacterCustomizer;
    [SerializeField] GameObject leftBtn;
    [SerializeField] GameObject rightBtn;
    [SerializeField] GameObject tickBtn;
    [SerializeField] GameObject selectBtn;
    int CurrentCount = 0;
    public void InitCharacterMenu()
    {
        gameObject.SetActive(true);
        CurrentCount = CommonData.CurrentSelectedCharacterID;
        CharacterCustomizer.SetUpCharacter(CurrentCount);
        CheckSelection();
    }
    public void OnClick_Left()
    {
        CurrentCount--;
        if (CurrentCount < 1)
            CurrentCount = 1;
        CharacterCustomizer.SetUpCharacter(CurrentCount);
        CheckSelection();
    }
    public void OnClick_Right()
    {
        if (CurrentCount < 5)
            CurrentCount++;

        CharacterCustomizer.SetUpCharacter(CurrentCount);
        CheckSelection();
    }
    public void OnClick_SelectPlayer()
    {
        CheckSelection();
    }
    void CheckSelection()
    {

        leftBtn.SetActive(CurrentCount>1);
        rightBtn.SetActive(CurrentCount < 5);
        tickBtn.SetActive(CurrentCount == CommonData.CurrentSelectedCharacterID);
        selectBtn.SetActive(!tickBtn.activeInHierarchy);
    }
    public void OnClick_Back()
    {
        MainHandler.Instance.menuHandler.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
