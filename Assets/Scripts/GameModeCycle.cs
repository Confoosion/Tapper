using UnityEngine;

public class GameModeCycle : MonoBehaviour
{
    public GameModeSO[] gameModeList;
    private int modeIndex = 0;

    public void SwitchGameMode(int direction)
    {
        modeIndex = (modeIndex + direction) % gameModeList.Length;
        if (modeIndex == -1) modeIndex = gameModeList.Length - 1;

        GameModeSO currMode = gameModeList[modeIndex];

        GameManager.Singleton.currentGameMode = currMode;
        UIManager.Singleton.ChangeGameModeUI(currMode);
    }
}
