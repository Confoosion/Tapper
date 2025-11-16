using UnityEngine;

public class GameModeCycle : MonoBehaviour
{
    private GameModeSO currGameMode;
    public GameModeSO[] gameModeList;
    public Sprite[] gameModeSprites;
    public GameObject[] gameModeDetails;

    private int modeIndex = 0;

    void Start()
    {
        currGameMode = gameModeList[0];
    }

    public void SwitchGameMode(int direction)
    {
        if(ScreenManager.Singleton.IsTransitionGoing())
        {
            return;
        }

        gameModeDetails[modeIndex].SetActive(false);

        modeIndex = (modeIndex + direction) % gameModeList.Length;
        if (modeIndex == -1) modeIndex = gameModeList.Length - 1;

        GameModeSO currMode = gameModeList[modeIndex];

        GameManager.Singleton.currentGameMode = currMode;
        UIManager.Singleton.ChangeGameModeUI(currMode, gameModeSprites[modeIndex]);

        gameModeDetails[modeIndex].SetActive(true);
        currGameMode = currMode;
    }

    public void RetrieveCurrentMode()
    {
        if(ScreenManager.Singleton.IsTransitionGoing())
        {
            return;
        }
        
        GameManager.Singleton.UpdateGameMode(currGameMode);
    }
}
