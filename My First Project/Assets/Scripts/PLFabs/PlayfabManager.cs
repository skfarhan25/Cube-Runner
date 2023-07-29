using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject rowPrefab;
    public Transform rowParent;

    void Awake()
    {
        PlayFabSettings.TitleId = "5EB62"; //your title id goes here.
    }

    public void Login()
    {
        var request = new LoginWithCustomIDRequest 
        {
            CustomId = gameManager.PlayerName, 
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        Debug.Log("Your PlayFab ID is: " + result.PlayFabId);
        string name = null;
        
        if (result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;

        UpdateName();
    }

    public void UpdateName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = gameManager.PlayerName,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Display name updated");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Error while logging in player with Custom ID:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "LevelCompleted", Value = score}
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Leaderboard updated");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest { StatisticName = "LevelCompleted", StartPosition = 0, MaxResultsCount = 10 };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform child in rowParent)
        {
            Destroy(child.gameObject);
        }
        foreach (var player in result.Leaderboard)
        {
            GameObject row = Instantiate(rowPrefab, rowParent);
            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (player.Position + 1).ToString();
            texts[1].text = player.DisplayName;
            texts[2].text = player.StatValue.ToString();

            Debug.Log(player.Position + " " + player.PlayFabId + " " + player.StatValue);
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
