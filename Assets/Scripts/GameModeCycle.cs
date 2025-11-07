using UnityEngine;

public class GameModeCycle : MonoBehaviour
{
    public GameModeSO[] gameModeList;
    public Sprite[] gameModeSprites;
    public GameObject[] gameModeDetails;

    private int modeIndex = 0;

    public void SwitchGameMode(int direction)
    {
        gameModeDetails[modeIndex].SetActive(false);

        modeIndex = (modeIndex + direction) % gameModeList.Length;
        if (modeIndex == -1) modeIndex = gameModeList.Length - 1;

        GameModeSO currMode = gameModeList[modeIndex];

        GameManager.Singleton.currentGameMode = currMode;
        UIManager.Singleton.ChangeGameModeUI(currMode, gameModeSprites[modeIndex]);

        gameModeDetails[modeIndex].SetActive(true);
    }
}
