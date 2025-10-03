using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Dan.Main;
using Dan.Models;

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
                names[i].text = ForDisplay(msg[i].Username);
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(int score)
    {
        PlayerPrefs.SetInt("SavedHighScore", Mathf.Max(score, PlayerPrefs.GetInt("SavedHighScore", 0)));

        string display = PlayerPrefs.GetString("Username", "Tapper");
        string unique = BuildUniqueUsername(display);

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, unique, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }

    public void RemoveLeaderboardEntry()
    {
        Debug.Log("Deleting entry...");
        LeaderboardCreator.DeleteEntry(publicLeaderboardKey, (success) =>
        {
            if (success)
            {
                Debug.Log("SUCCESSFULLY DELETED!!!");
                GetUsername();
            }
            else
            {
                Debug.Log("FAILED DELETION!!!");
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
        return (id.Substring(id.Length - 6).ToUpperInvariant());
    }

    private string BuildUniqueUsername(string displayName)
    {
        string id = EnsurePlayerID();
        string tag = GetTagFromID(id);
        return ($"{displayName}#{tag}");
    }

    private string ForDisplay(string uniqueName)
    {
        int i = uniqueName.LastIndexOf('#');
        return (i >= 0) ? uniqueName.Substring(0, i) : uniqueName;
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
        string newUnique = $"{newDisplayName}#{tag}";

        PlayerPrefs.SetString("Username", newDisplayName);

        int scoreToKeep = PlayerPrefs.GetInt("SavedHighScore", 0);

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, newUnique, scoreToKeep, _ =>
        {
            GetLeaderboard();
        });

        // string id = EnsurePlayerID();
        // string tag = GetTagFromID(id);
        // string oldDisplay = PlayerPrefs.GetString("Username", "Tapper");
        // string oldUnique = $"{oldDisplay}#{tag}";
        // string newUnique = $"{newDisplayName}#{tag}";

        // int scoreToKeep = PlayerPrefs.GetInt("SavedHighScore", 0);

        // RemoveLeaderboardEntry();
        // PlayerPrefs.SetString("Username", newDisplayName);

        // LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, newUnique, scoreToKeep, (msg) =>
        // {
        //     GetLeaderboard();
        // });
    }

    public void GetPersonalData()
    {
        string displayName = PlayerPrefs.GetString("Username", "Tapper");
        string uniqueName = BuildUniqueUsername(displayName);
        int localBestScore = PlayerPrefs.GetInt("SavedHighScore", 0);


        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (entries) =>
        {
            Entry? mine = null;
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Username == uniqueName)
                {
                    mine = entries[i];
                    break;
                }
            }

            if (mine.HasValue)
            {
                if (mine.Value.Score > localBestScore)
                {
                    PlayerPrefs.SetInt("SavedHighScore", mine.Value.Score);
                }
                UIManager.Singleton.UpdatePersonalLeaderboard(mine.Value.Rank, displayName, mine.Value.Score);
            }
            else
            {
                UIManager.Singleton.UpdatePersonalLeaderboard(-1, displayName, localBestScore);
            }
        });
    }
}
