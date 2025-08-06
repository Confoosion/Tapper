using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GamemodeSwapper : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameMode selectedMode;
    private TextMeshProUGUI gameModeLabel;

    void Awake()
    {
        gameModeLabel = GetComponent<TextMeshProUGUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectedMode = GameMode.Classic;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (UIManager.Singleton.IsCoroutineActive())
        {
            return;
        }
        
        GameMode[] modes = (GameMode[])System.Enum.GetValues(typeof(GameMode));
        int nextMode = ((int)selectedMode + 1) % modes.Length;
        selectedMode = modes[nextMode];

        gameModeLabel.SetText(selectedMode.ToString());
        GameManager.Singleton.UpdateGameMode(selectedMode);
    }
}
