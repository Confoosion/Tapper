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

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }

    public void RemoveLeaderboardEntry()
    {
        LeaderboardCreator.DeleteEntry(publicLeaderboardKey);
    }
}
