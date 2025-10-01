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
        PlayerPrefs.SetInt("PlayerBestScore", Mathf.Max(score, PlayerPrefs.GetInt("PlayerBestScore", 0)));

        string display = PlayerPrefs.GetString("Username", "Tapper");
        string unique = BuildUniqueUsername(display);

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, unique, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }

    public void RemoveLeaderboardEntry()
    {
        LeaderboardCreator.DeleteEntry(publicLeaderboardKey, (success) =>
        {
            if (success)
            {
                GetUsername();
            }
        });
    }

    private string EnsurePlayerID()
    {
        if (!PlayerPrefs.HasKey("PlayerID"))
        {
            string id = System.Guid.NewGuid().ToString("N");
            PlayerPrefs.SetString("PlayerID", id);
        }
        return PlayerPrefs.GetString("PlayerID");
    }

    private string GetTagFromID(string id)
    {
        return (id.Substring(id.Length - 4).ToUpperInvariant());
    }

    private string BuildUniqueUsername(string displayName)
    {
        string id = EnsurePlayerID();
        string tag = GetTagFromID(id);
        return ($"{displayName}#{tag}");
    }

    public void GetUsername()
    {
        if (!PlayerPrefs.HasKey("Username"))
        {
            PlayerPrefs.SetString("Username", "Tapper");
            SetLeaderboardEntry(0);
        }
    }

    public void SetUsername(string newDisplayName)
    {
        string id = EnsurePlayerID();
        string tag = GetTagFromID(id);
        string oldDisplay = PlayerPrefs.GetString("Username", "Tapper");
        string oldUnique = $"{oldDisplay}#{tag}";
        string newUnique = $"{newDisplayName}#{tag}";

        int scoreToKeep = PlayerPrefs.GetInt("PlayerBestScore", 0);

        LeaderboardCreator.DeleteEntry(publicLeaderboardKey, (success) =>
        {
            PlayerPrefs.SetString("Username", newDisplayName);

            LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, newUnique, scoreToKeep, (msg) =>
            {
                GetLeaderboard();
            });
        });
    }

    public bool FindUsername(string username)
    {
        bool found = false;
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (entries) =>
        {
            foreach (var entry in entries)
            {
                if (entry.Username == username && username != PlayerPrefs.GetString("Username"))
                {
                    found = true;
                    break;
                }
            }
        });
        return (found);
    }

    public void GetPersonalData()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (entries) =>
        {
            foreach (var entry in entries)
            {
                if (entry.Username == PlayerPrefs.GetString("Username"))
                {
                    UIManager.Singleton.UpdatePersonalLeaderboard(entry.Rank, entry.Username, entry.Score);
                    break;
                }
            }
        });
    }
}
