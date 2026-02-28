using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private int totalLevels = 10;
    [SerializeField] private GameData gameData;
    
    private VisualElement _root;

    private VisualElement _mainMenu;
    private VisualElement _levelMenu;
    private VisualElement _levelContainer;
    private VisualElement _creditsContainer;

    private Button _startBtn;
    private Button _creditsBtn;
    private Button _levelBtn;
    private Button _exitBtn;
    private Button _backBtn;
    private Button _creditsExitBtn;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        QueryElements();
    }

    private void OnEnable()
    {
        SubscribeButtons();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void QueryElements()
    {
        _mainMenu = _root.Q<VisualElement>("TextAndButton");
        _levelMenu = _root.Q<VisualElement>("LevelMenu");
        _levelContainer = _root.Q<VisualElement>("LevelContainer");
        _creditsContainer = _root.Q<VisualElement>("CreditsContainer");
        
        _startBtn = _root.Q<Button>("StartBtn");
        _creditsBtn = _root.Q<Button>("CreditsBtn");
        _levelBtn = _root.Q<Button>("LevelBtn");
        _exitBtn = _root.Q<Button>("ExitBtn");
        _backBtn = _root.Q<Button>("BackBtn");
        _creditsExitBtn = _root.Q<Button>("ExitCreditsBtn");
    }

    private void SubscribeButtons()
    {
        _startBtn?.RegisterCallback<ClickEvent>(OnStartClick);
        _creditsBtn?.RegisterCallback<ClickEvent>(OnCreditsClick);
        _levelBtn?.RegisterCallback<ClickEvent>(OnLevelClick);
        _exitBtn?.RegisterCallback<ClickEvent>(OnExitClick);
        _backBtn?.RegisterCallback<ClickEvent>(ShowMainMenu);
        _creditsExitBtn?.RegisterCallback<ClickEvent>(ShowMainMenu);
    }

    private void UnsubscribeEvents()
    {
        _startBtn?.UnregisterCallback<ClickEvent>(OnStartClick);
        _creditsBtn?.UnregisterCallback<ClickEvent>(OnCreditsClick);
        _levelBtn?.UnregisterCallback<ClickEvent>(OnLevelClick);
        _exitBtn?.UnregisterCallback<ClickEvent>(OnExitClick);
        _backBtn?.UnregisterCallback<ClickEvent>(ShowMainMenu);
        _creditsExitBtn?.UnregisterCallback<ClickEvent>(ShowMainMenu);

    }

    private void ShowMainMenu(ClickEvent evt)
    {
        HideAllExcept(_mainMenu);
    }

    private void OnExitClick(ClickEvent evt)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void OnLevelClick(ClickEvent evt)
    {
        HideAllExcept(_levelMenu);

        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        _levelContainer.Clear();
        _levelContainer.style.display = DisplayStyle.Flex;

        for (int i = 1; i <= totalLevels; i++)
        {
            Button levelBox = new Button();
            levelBox.text = i.ToString();

            levelBox.AddToClassList("level-box");
            levelBox.AddToClassList("button");

            int index = i;
            levelBox.RegisterCallback<ClickEvent>(e => LoadLevel(index));

            _levelContainer.Add(levelBox);
        }
    }

    private void LoadLevel(int index)
    {
        Debug.Log($"Loading Level {index}");
        SceneManager.LoadScene(index);
    }

    private void OnCreditsClick(ClickEvent evt)
    {
        HideAllExcept(_creditsContainer);
    }

    private void OnStartClick(ClickEvent evt)
    {
        SceneManager.LoadScene(gameData.lastLevel);
    }

    #region Helper

    private void HideAllContainers()
    {
        _mainMenu.style.display = DisplayStyle.None;
        _levelMenu.style.display = DisplayStyle.None;
        _levelContainer.style.display = DisplayStyle.None;
        _creditsContainer.style.display = DisplayStyle.None;
    }

    private void HideAllExcept(in VisualElement element)
    {
        HideAllContainers();
        element.style.display = DisplayStyle.Flex;
    }

    #endregion
}