using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Firebase;
using Google.MiniJSON;
using System;

public class FirebaseManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static FirebaseManager instance;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    public DatabaseReference dbreference;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
        
    }
    public void InitializeFirebase()
    {
        Debug.LogError("Firebase initialized: " + dependencyStatus);
        
        dbreference = FirebaseDatabase.DefaultInstance.RootReference;


    }
    public void Register_Or_Update_Database()
    {
        string userid = UserDataManager.Instance._UserData.userID;
        string json = JsonUtility.ToJson(UserDataManager.Instance._UserData);
        dbreference.Child("users").Child(userid).SetRawJsonValueAsync(json);
    }
    public void LoadRealtimeDatabase(Action onDataLoaded)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child("users");
        UserDataManager.Instance.ShowLoadingPanel(true);
        reference.OrderByChild("playerHigScore").ValueChanged += (object sender, ValueChangedEventArgs e) =>
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError("Firebase Database Error: " + e.DatabaseError.Message);
                UserDataManager.Instance.ShowLoadingPanel(false);
                return;
            }

            if (e.Snapshot.Exists)
            {
                // Iterate through the children (users) and retrieve their data
                int count = 0;
                foreach (var childSnapshot in e.Snapshot.Children)
                {
                    // Deserialize the JSON data into a leaderboard entry object
                    if (int.Parse(childSnapshot.Child("playerHigScore").GetValue(true).ToString()) > 0)
                    {
                        UserData leaderboardData = new UserData
                        {
                            playerName = childSnapshot.Child("playerName").GetValue(true).ToString(),
                            playerHigScore = int.Parse(childSnapshot.Child("playerHigScore").GetValue(true).ToString()),
                            userID= childSnapshot.Child("userID").GetValue(true).ToString(),
                            profilePicID= int.Parse(childSnapshot.Child("profilePicID").GetValue(true).ToString())
                        };
                        LeaderBoardHandler.instance.allUserData.Add(leaderboardData);
                        count++;
                        if (count >= 100)
                            break;
                        Debug.Log("Player Name: " + leaderboardData.playerName);
                        Debug.Log("Score: " + leaderboardData.playerHigScore);
                    }
                    // Now you can work with the leaderboard entry object (e.g., add it to a list)
                   
                }
                if(onDataLoaded!=null)
                {
                    LeaderBoardHandler.instance.allUserData.Sort((a, b) => b.playerHigScore.CompareTo(a.playerHigScore));
                    onDataLoaded?.Invoke();
                }
                else
                    UserDataManager.Instance.ShowLoadingPanel(false);
            }
            else
            {
                Debug.Log("No data exists at this location.");
            }
        };

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
