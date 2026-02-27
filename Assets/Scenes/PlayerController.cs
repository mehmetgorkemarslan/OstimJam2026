using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private float _bloodflowSpeed = 2f;

    [SerializeField]
    private float _maxStamina = 100f;

    [SerializeField]
    private float _staminaRegenRate = 10f;

    [SerializeField]
    private float _staminaDepletionRate = 20f;

    [SerializeField]
    private float _currentStamina;

    private Rigidbody2D _rb;
    private Vector2 _targetPos;
    Vector2 input;

    [SerializeField]
    Image staminaBar;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _currentStamina = _maxStamina;
    }

    private void FixedUpdate()
    {
        staminaBar.fillAmount = _currentStamina / _maxStamina;
        float staminaMultiplier = (_currentStamina > 0) ? 1f : 0f;
        _targetPos = _rb.position + input * staminaMultiplier * _speed * Time.fixedDeltaTime + new Vector2(_bloodflowSpeed * Time.fixedDeltaTime, 0);
        _currentStamina -= new Vector2(input.x, input.y * 0.5f).magnitude * _staminaDepletionRate * Time.fixedDeltaTime;
        _currentStamina += _staminaRegenRate * Time.fixedDeltaTime;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
        _rb.MovePosition(_targetPos);
    }

    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        
        
    }

}

    
