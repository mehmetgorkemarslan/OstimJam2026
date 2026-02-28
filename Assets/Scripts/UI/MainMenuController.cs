using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private VisualElement _root;

    private Button _startBtn;
    private Button _creditsBtn;
    private Button _levelBtn;
    private Button _exitBtn;

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
        _startBtn = _root.Q<Button>("StartBtn");
        _creditsBtn = _root.Q<Button>("CreditsBtn");
        _levelBtn = _root.Q<Button>("LevelBtn");
        _exitBtn = _root.Q<Button>("ExitBtn");
    }

    private void SubscribeButtons()
    {
        _startBtn?.RegisterCallback<ClickEvent>(OnStartClick);
        _creditsBtn?.RegisterCallback<ClickEvent>(OnCreditsClick);
        _levelBtn?.RegisterCallback<ClickEvent>(OnLevelClick);
        _exitBtn?.RegisterCallback<ClickEvent>(OnExitClick);
    }

    private void UnsubscribeEvents()
    {
        _startBtn?.UnregisterCallback<ClickEvent>(OnStartClick);
        _creditsBtn?.UnregisterCallback<ClickEvent>(OnCreditsClick);
        _levelBtn?.UnregisterCallback<ClickEvent>(OnLevelClick);
        _exitBtn?.UnregisterCallback<ClickEvent>(OnExitClick);
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
        throw new NotImplementedException();
    }

    private void OnCreditsClick(ClickEvent evt)
    {
        throw new NotImplementedException();
    }

    private void OnStartClick(ClickEvent evt)
    {
        throw new NotImplementedException();
    }
}