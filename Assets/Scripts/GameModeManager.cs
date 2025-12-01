using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Singleton;

    [SerializeField] GameModeSO currentGameMode;
    public GameModeSO[] gameModeList;
    public GameObject[] arcadeDetails;
    private int modeIndex = 0;
    private GameModeSO currentArcadeMode;
    private GameObject currentArcadeDetails;
    private GameModeSO highscoreMode;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        currentArcadeMode = gameModeList[modeIndex];
        currentArcadeDetails = arcadeDetails[modeIndex];
    }

    public GameModeSO GetCurrentMode()
    {
        return(currentGameMode);
    }

    public void SwitchArcadeMode(int direction)
    {
        SwitchMode(direction, true);
    }

    public void SwitchArcadeMode()
    {
        int index = System.Array.IndexOf(gameModeList, currentArcadeMode);
        SwitchMode(index - modeIndex, true);
    }

    public void SwitchGameMode(int direction)
    {
        SwitchMode(direction, false);
    }

    void SwitchMode(int direction, bool arcadeMode)
    {
        if(ScreenManager.Singleton.IsTransitionGoing())
        {
            return;
        }

        int prevModeIndex = modeIndex;

        modeIndex = (modeIndex + direction) % gameModeList.Length;
        if (modeIndex == -1) modeIndex = gameModeList.Length - 1;

        if(arcadeMode)
        {
            while(gameModeList[modeIndex].isTimed)
            {
                modeIndex = (modeIndex + direction) % gameModeList.Length;
                if(modeIndex == -1) modeIndex = gameModeList.Length - 1;
            }
        }

        GameModeSO currMode = gameModeList[modeIndex];

        GameManager.Singleton.currentGameMode = currMode;
        currentGameMode = currMode;

        GameManager.Singleton.SetLives(currentGameMode.lives);
        SpawnManager.Singleton.SetSpawnVariables(currMode.badSpawnPercentage, currMode.decayRate, currMode.doGraceSpawns, currMode.isTimed);
        UIManager.Singleton.ChangeGameModeUI(currMode, modeIndex, !currMode.isTimed, highscoreMode == currentGameMode);

        if(!currMode.isTimed)
        {
            currentArcadeDetails.SetActive(false);
            arcadeDetails[modeIndex].SetActive(true);

            currentArcadeMode = currMode;
            currentArcadeDetails = arcadeDetails[modeIndex];
        }
    }

    public void SwitchSpecificMode(GameModeSO gameMode)
    {
        int index = System.Array.IndexOf(gameModeList, gameMode);
        SwitchMode(index - modeIndex, false);
    }

    public void SetHighscoreMode(bool reset = false)
    {
        if(reset)
            highscoreMode = null;
        else
            highscoreMode = currentGameMode;
    }
}
