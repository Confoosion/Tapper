using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Dan.Main;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Singleton;
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;

    private string publicLeaderboardKey = "fae9568c27d3987720c81b07b6c34bcf3ef2a0102dd4420ad4eb399f6a39ccec";

    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        GetUsername();
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, PlayerPrefs.GetString("Username"), score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }

    public void RemoveLeaderboardEntry()
    {
        LeaderboardCreator.DeleteEntry(publicLeaderboardKey);
    }

    private void GetUsername()
    {
        string username;

        if (PlayerPrefs.HasKey("Username"))
        {
            username = PlayerPrefs.GetString("Username");
        }
        else
        {
            LeaderboardCreator.GetEntryCount(publicLeaderboardKey, ((count) =>
            {
                username = "Tapper" + (count + 1).ToString();
                PlayerPrefs.SetString("Username", username);
                SetLeaderboardEntry(0);
            }));
        }
    }
}
