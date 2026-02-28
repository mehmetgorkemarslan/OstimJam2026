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

    [SerializeField] private float _animSpeedMultiplier = 0.5f; // Adjust to feel right

    private Rigidbody2D _rb;
    private Vector2 _targetPos;
    Vector2 input;

    private Animator _animator;

    [SerializeField]
    Image staminaBar;

    // --- Add these new variables to your class ---
    private Vector2 _currentInputVelocity; // Used internally by SmoothDamp
    private Vector2 _smoothedInput;
    [SerializeField] private float _inputSmoothTime = 0.1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        _currentStamina = _maxStamina;
    }

    private void FixedUpdate()
    {
        // 1. Smooth the raw input
        _smoothedInput = Vector2.SmoothDamp(_smoothedInput, input, ref _currentInputVelocity, _inputSmoothTime);

        staminaBar.fillAmount = _currentStamina / _maxStamina;
        float staminaMultiplier = (_currentStamina > 0) ? 1f : 0f;

        // 2. Use _smoothedInput instead of input for the math
        Vector2 movement = _smoothedInput * staminaMultiplier * _speed;
        Vector2 bloodFlow = new Vector2(_bloodflowSpeed, 0);

        _targetPos = _rb.position + (movement + bloodFlow) * Time.fixedDeltaTime;

        // Update stamina based on smoothed input so it matches the visuals
        _currentStamina -= new Vector2(_smoothedInput.x, _smoothedInput.y * 0.5f).magnitude * _staminaDepletionRate * Time.fixedDeltaTime;
        _currentStamina += _staminaRegenRate * Time.fixedDeltaTime;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);

        _rb.MovePosition(_targetPos);

        // --- ROTATION LOGIC ---
        // Use the smoothed direction for rotation to prevent jittery turning
        Vector2 movementDirection = movement + bloodFlow;

        if (movementDirection.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            _rb.MoveRotation(angle);
        }

        _animator.SetFloat("speedMultiplier", movementDirection.magnitude * _animSpeedMultiplier);
    }

    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        
        
    }

}

    
