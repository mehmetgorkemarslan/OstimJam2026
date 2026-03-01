using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InGameUiController : MonoBehaviour
{
    [SerializeField] private GameObject barsContainer;
    [SerializeField] private GameObject stopMenuContainer;

    [SerializeField] private Image healthSlider;
    [SerializeField] private Image energySlider;

    [SerializeField] private GameObject player;
    private IHealth _playerHealth;
    private IEnergy _playerEnergy;
    private int _maxHealth;
    private int _maxEnergy;

    private bool _isGameStoped = false;

    private void OnValidate()
    {
        if (player != null && (player.GetComponent<IHealth>() == null || player.GetComponent<IEnergy>() == null))
        {
            Debug.LogError("Player does not have IHealth or IEnergy Component");
            player = null;
        }
    }

    private void Awake()
    {
        if (player != null)
        {
            _playerHealth = player.GetComponent<IHealth>();
            _playerEnergy = player.GetComponent<IEnergy>();
            _maxHealth = _playerHealth.getMaxHealth();
            _maxEnergy = _playerEnergy.getMaxEnergy();
        }

        if (!_isGameStoped)
        {
            SetBarsActive();
        }
    }

    private void OnEnable()
    {
        _playerHealth.OnHealthChanced += HandleHealthChance;
    }

    private void OnDisable()
    {
        _playerHealth.OnHealthChanced -= HandleHealthChance;
    }

    private void HandleHealthChance(int newHealth)
    {
        //TODO: Smooth Animation
        healthSlider.fillAmount = ((float)newHealth) / ((float)_maxHealth);
    }

    private void Update()
    {
        if (!_isGameStoped)
        {
            energySlider.fillAmount = ((float)_playerEnergy.getEnergy()) / ((float)_maxEnergy);
        }
    }

    public void SetMenuActive()
    {
        Time.timeScale = 0f;
        stopMenuContainer.SetActive(true);
        barsContainer.SetActive(false);
    }

    public void SetBarsActive()
    {
        Time.timeScale = 1f;
        stopMenuContainer.SetActive(false);
        barsContainer.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}