using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button randomizeButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartClick);
        randomizeButton.onClick.AddListener(PawnsManager.Instance.RandomizePawns);
    }

    private void OnStartClick()
    {
        startButton.gameObject.SetActive(false);
        randomizeButton.gameObject.SetActive(false);
        PawnsManager.Instance.StartSimulation();
    }
}
