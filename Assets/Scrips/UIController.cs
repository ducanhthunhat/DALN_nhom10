using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text resourcesText;
    [SerializeField] private TMP_Text warningText;

    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private TowerData[] towers;
    private List<GameObject> activeCards = new List<GameObject>();
    private Platform _currentPlatform;
    [SerializeField] private Button speed1Button;
    [SerializeField] private Button speed2Button;
    [SerializeField] private Button speed3Button;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Color normalButtonColor = Color.white;
    [SerializeField] private Color selectedButtonColor = Color.green;
    [SerializeField] private Color normalTextColor = Color.black;
    [SerializeField] private Color selectedTextColor = Color.white;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TMP_Text objectiveText;
    private bool _isGamePaused = false;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject missionCompletePanel;
    private bool _missionCompleteSoundPlayed = false;
    [SerializeField] private ParticleSystem missionCompleteParticles;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveText;
        GameManager.OnLivesChanged += UpdateLiveText;
        GameManager.OnResourcesChanged += UpdateResourcesText;
        Platform.OnPlatformClicked += HandlePlatformClicked;
        TowerCard.OnTowerSelected += HandleTowerSelected;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Spawner.OnMissionCompleted += ShowMissionCompletePanel;
    }
    private void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLiveText;
        GameManager.OnResourcesChanged -= UpdateResourcesText;
        Platform.OnPlatformClicked -= HandlePlatformClicked;
        TowerCard.OnTowerSelected -= HandleTowerSelected;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Spawner.OnMissionCompleted -= ShowMissionCompletePanel;

    }
    private void Start()
    {
        speed1Button.onClick.AddListener(() =>
        {
            SetGameSpeed(0.2f);
            AudioManager.Instance.PlaySpeedSlow();
        });
        speed2Button.onClick.AddListener(() =>
        {
            SetGameSpeed(1f);
            AudioManager.Instance.PlaySpeedNormal();
        });
        speed3Button.onClick.AddListener(() =>
        {
            SetGameSpeed(2f);
            AudioManager.Instance.PlaySpeedFast();
        });

        HighlightSelectedSpeedButton(GameManager.Instance.GameSpeed);
    }
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }
    private void UpdateWaveText(int currentText)
    {
        waveText.text = $"Wave: {currentText + 1} ";
    }
    private void UpdateLiveText(int currentLives)
    {
        livesText.text = $"Lives: {currentLives} ";
        if (currentLives <= 0)
        {
            ShowGameOverPanel();
        }
    }
    private void UpdateResourcesText(int currentLives)
    {
        resourcesText.text = $"Resources: {currentLives} ";
    }

    private void HandlePlatformClicked(Platform platform)
    {
        _currentPlatform = platform;
        ShowTowerPanel();
    }
    private void ShowTowerPanel()
    {
        towerPanel.SetActive(true);
        Platform.towerPanelOpen = true;
        GameManager.Instance.SetTimeScale(0f);
        PopulateTowerCards();
        AudioManager.Instance.PlayPanelToggle();
    }

    public void HideTowerPanel()
    {
        towerPanel.SetActive(false);
        Platform.towerPanelOpen = false;
        GameManager.Instance.SetTimeScale(GameManager.Instance.GameSpeed);
    }

    private void PopulateTowerCards()
    {
        foreach (var card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();
        foreach (var data in towers)
        {
            GameObject cardGameObject = Instantiate(towerCardPrefab, cardsContainer);
            TowerCard card = cardGameObject.GetComponent<TowerCard>();
            card.Initialize(data);
            activeCards.Add(cardGameObject);
        }
    }
    private void HandleTowerSelected(TowerData towerData)
    {
        if (_currentPlatform.transform.childCount > 0)
        {
            HideTowerPanel();
            StartCoroutine(ShowNoWarningMessage("tower existed!"));
            return;
        }
        if (GameManager.Instance.Resources >= towerData.cost)
        {
            AudioManager.Instance.PlayTowerPlace();
            GameManager.Instance.SpendResources(towerData.cost);
            _currentPlatform.PlaceTower(towerData);
        }
        else
        {
            StartCoroutine(ShowNoWarningMessage("Not enough money!"));
        }
        HideTowerPanel();

    }
    private IEnumerator ShowNoWarningMessage(string message)
    {
        warningText.text = message;
        AudioManager.Instance.PlayWarning();
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        warningText.gameObject.SetActive(false);
    }
    private void SetGameSpeed(float timeScale)
    {
        HighlightSelectedSpeedButton(timeScale);
        GameManager.Instance.SetGameSpeed(timeScale);
    }

    private void UpdateButtonVisual(Button button, bool isSelected)
    {
        button.image.color = isSelected ? selectedButtonColor : normalButtonColor;
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.color = isSelected ? selectedTextColor : normalTextColor;
        }
    }

    private void HighlightSelectedSpeedButton(float selectedSpeed)
    {
        UpdateButtonVisual(speed1Button, Mathf.Approximately(selectedSpeed, 0.2f));
        UpdateButtonVisual(speed2Button, Mathf.Approximately(selectedSpeed, 1f));
        UpdateButtonVisual(speed3Button, Mathf.Approximately(selectedSpeed, 2f));
    }

    public void TogglePause()
    {
        if (towerPanel.activeSelf)
        {
            return;
        }
        if (_isGamePaused)
        {
            pausePanel.SetActive(false);
            _isGamePaused = false;
            GameManager.Instance.SetTimeScale(GameManager.Instance.GameSpeed);
            AudioManager.Instance.PlayUnpause();
        }
        else
        {
            pausePanel.SetActive(true);
            _isGamePaused = true;
            GameManager.Instance.SetTimeScale(0f);
            AudioManager.Instance.PlayPause();
        }
    }
    public void RestartGame()
    {
        LevelManager.Instance.LoadLevel(LevelManager.Instance.CurrentLevel);
        HighlightSelectedSpeedButton(1f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToMainMenu()
    {
        GameManager.Instance.SetTimeScale(1f);
        SceneManager.LoadScene("MainMenu");
    }
    public void ShowGameOverPanel()
    {
        GameManager.Instance.SetTimeScale(0f);
        gameOverPanel.SetActive(true);
        AudioManager.Instance.PlayGameOver();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = mainCamera;

        HidePanel();
        _isGamePaused = false;
        _missionCompleteSoundPlayed = false;
        if (scene.name == "MainMenu")
        {
            HideUI();
            return;
        }
        else
        {
            ShowUI();
            HighlightSelectedSpeedButton(1f);
            StartCoroutine(ShowObjectiveText($"Survive {LevelManager.Instance.CurrentLevel.wavesToWin} waves"));
        }
    }
    private IEnumerator ShowObjectiveText(string message)
    {
        objectiveText.text = $"Survive {LevelManager.Instance.CurrentLevel.wavesToWin} waves";
        objectiveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        objectiveText.gameObject.SetActive(false);
    }

    private void ShowMissionCompletePanel()
    {
        if (!_missionCompleteSoundPlayed)
        {
            _missionCompleteSoundPlayed = true;
            UpdateNextLevelButton();
            missionCompletePanel.SetActive(true);
            GameManager.Instance.SetTimeScale(0f);
            AudioManager.Instance.PlayMissionComplete();
            missionCompleteParticles.Play();
        }

    }

    public void EnterEndlessMode()
    {
        missionCompletePanel.SetActive(false);
        GameManager.Instance.SetTimeScale(GameManager.Instance.GameSpeed);
        Spawner.Instance.EnableEndlessMode();
        StartCoroutine(ShowObjectiveText("Endless Mode Activated!"));
    }
    private void HideUI()
    {
        HidePanel();
        waveText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        resourcesText.gameObject.SetActive(false);
        warningText.gameObject.SetActive(false);
        speed1Button.gameObject.SetActive(false);
        speed2Button.gameObject.SetActive(false);
        speed3Button.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
    }

    private void ShowUI()
    {
        waveText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        resourcesText.gameObject.SetActive(true);
        speed1Button.gameObject.SetActive(true);
        speed2Button.gameObject.SetActive(true);
        speed3Button.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
    }
    private void HidePanel()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        missionCompletePanel.SetActive(false);
    }

    public void LoadNextLevel()
    {
        var levelManager = LevelManager.Instance;
        int currentLevelIndex = Array.IndexOf(levelManager.allLevels, levelManager.CurrentLevel);
        int nextIndex = currentLevelIndex + 1;
        if (nextIndex < levelManager.allLevels.Length)
        {
            levelManager.LoadLevel(levelManager.allLevels[nextIndex]);
        }
    }
    private void UpdateNextLevelButton()
    {
        var levelManager = LevelManager.Instance;
        int currentIndex = Array.IndexOf(levelManager.allLevels, levelManager.CurrentLevel);
        nextLevelButton.interactable = currentIndex + 1 < levelManager.allLevels.Length;

    }
}
