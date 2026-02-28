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
        staminaBar.fillAmount = _currentStamina / _maxStamina;
        float staminaMultiplier = (_currentStamina > 0) ? 1f : 0f;
        _targetPos = _rb.position + input * staminaMultiplier * _speed * Time.fixedDeltaTime + new Vector2(_bloodflowSpeed * Time.fixedDeltaTime, 0);
        _currentStamina -= new Vector2(input.x, input.y * 0.5f).magnitude * _staminaDepletionRate * Time.fixedDeltaTime;
        _currentStamina += _staminaRegenRate * Time.fixedDeltaTime;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
        _rb.MovePosition(_targetPos);

        // --- ROTATION LOGIC ---
        // We calculate the actual velocity vector (Input + Bloodflow)
        Vector2 movementDirection = (input * staminaMultiplier * _speed) + new Vector2(_bloodflowSpeed, 0);

        // Only rotate if there is meaningful movement to avoid "snapping" to zero
        if (movementDirection.sqrMagnitude > 0.01f)
        {
            // Atan2 returns the angle in radians. 
            // For "Right" as forward, we don't need an offset.
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;

            // MoveRotation is better for Rigidbodies than transform.rotation
            _rb.MoveRotation(angle);
        }

        _animator.SetFloat("speedMultiplier", movementDirection.magnitude * _animSpeedMultiplier);
    }

    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        
        
    }

}

    
