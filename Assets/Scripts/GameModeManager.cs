using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Singleton;

    [SerializeField] GameModeSO currentGameMode;
    public GameModeSO[] gameModeList;
    public GameObject[] arcadeDetails;
    private int modeIndex = 0;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
    }

    public GameModeSO GetCurrentMode()
    {
        return(currentGameMode);
    }

    public void SwitchArcadeMode(int direction)
    {
        SwitchMode(direction, true);
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

        if(arcadeMode)
            arcadeDetails[modeIndex].SetActive(false);

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

        // ScoreManager.UpdateHighscore();
        GameManager.Singleton.SetLives(currentGameMode.lives);
        SpawnManager.Singleton.SetSpawnVariables(currMode.badSpawnPercentage, currMode.decayRate, currMode.doGraceSpawns, currMode.isTimed);
        UIManager.Singleton.ChangeGameModeUI(currMode, modeIndex, arcadeMode);

        if(!currMode.isTimed)
            arcadeDetails[modeIndex].SetActive(true);
        currentGameMode = currMode;
    }
}
