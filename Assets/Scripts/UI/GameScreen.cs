using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button randomizeButton;
    [SerializeField] private Button addPawnButton;

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStartClick);
        randomizeButton.onClick.AddListener(PawnsManager.Instance.RandomizePawns);
        addPawnButton.onClick.AddListener(PawnsManager.Instance.CreateAdditionalPawn);
        addPawnButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartClick);

        if (PawnsManager.Instance != null)
        {
            randomizeButton.onClick.RemoveListener(PawnsManager.Instance.RandomizePawns);
            addPawnButton.onClick.RemoveListener(PawnsManager.Instance.CreateAdditionalPawn);
        }
    }

    private void OnStartClick()
    {
        startButton.gameObject.SetActive(false);
        randomizeButton.gameObject.SetActive(false);
        addPawnButton.gameObject.SetActive(true);
        PawnsManager.Instance.StartSimulation();
    }
}
