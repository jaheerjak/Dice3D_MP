using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonData : MonoBehaviour
{
    public static bool isDicePlayed = false;
    public static string Username = "jak";
    public static string FriendRoomCode = "12345";
    public static string defCode = "myroom";
    public static int maxPlayers=2;
    public static bool onJoinRoomClicked = false;
    public static int CurrentYugamId=1;
    public static int diceCount=1;
    public static int predictIndex;
    public static int diceRandAnim;
    public static int diceVal1;
    public static int diceVal2;
    public static bool isPredictClicked = false;
    public static string GameLink= "https://www.gameverkz.com/Mokshapata/privacy.html";
    public static int isProfileViewed
    {
        get => PlayerPrefs.GetInt(nameof(isProfileViewed), 0);
        set => PlayerPrefs.SetInt(nameof(isProfileViewed), value);
    }
    public static float musicVolume
    {
        get=> PlayerPrefs.GetFloat(nameof(musicVolume), 1f);        
        set=> PlayerPrefs.SetFloat(nameof(musicVolume), value);
    }
    public static float soundVolume
    {
        get=> PlayerPrefs.GetFloat(nameof(soundVolume), 1f);        
        set=> PlayerPrefs.SetFloat(nameof(soundVolume), value);
    }
    public static string PlayerGender
    {
        get => PlayerPrefs.GetString(nameof(PlayerGender), Gender.Male.ToString());
        set => PlayerPrefs.SetString(nameof(PlayerGender), value);
    }
    public static int CurrentSelectedCharacterID
    {
        get => UserDataManager.Instance._UserData.profilePicID;
    }
}
