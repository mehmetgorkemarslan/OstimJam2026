using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

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

    [Header("Stun Effects")]
    [SerializeField] private Volume _postProcessVolume;
    private ChromaticAberration _chromatic;
    private LensDistortion _lensDistort;
    private Vignette _vignette;

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

    private float _normalSpeed;

    private float _stunnedSpeedMultiplier = 0.3f;
    private float _stunnedTimer;
    private bool _isStunned;
    private float _stunTime = 2;


    private float _initialChromatic;
    private float _initialDistort;
    private float _initialVignette;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        _currentStamina = _maxStamina;
        _normalSpeed = _speed;

        if (_postProcessVolume != null && _postProcessVolume.profile != null)
        {
            _postProcessVolume.profile.TryGet(out _chromatic);
            _postProcessVolume.profile.TryGet(out _lensDistort);
            _postProcessVolume.profile.TryGet(out _vignette);



            if (_chromatic != null)
                _initialChromatic = _chromatic.intensity.value; // Store your starting value

            if (_lensDistort != null)
                _initialDistort = _lensDistort.intensity.value; // Store your starting value

            if (_vignette != null)
                _initialVignette = _vignette.intensity.value; // Store your starting value
        }
    }

    private void FixedUpdate()
    {
        float staminaMultiplier = Mathf.Clamp01( _currentStamina / 10);
        // 1. Smooth the raw input
        _smoothedInput = Vector2.SmoothDamp(_smoothedInput, input * staminaMultiplier, ref _currentInputVelocity, _inputSmoothTime);

        staminaBar.fillAmount = _currentStamina / _maxStamina;
        

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

    private void Update()
    {
        if(_isStunned)
        {
            _stunnedTimer -= Time.deltaTime;

            // Calculate "t" (1.0 = just stunned, 0.0 = recovered)
            float t = Mathf.Clamp01(_stunnedTimer / _stunTime);
            UpdatePostProcessing(t);

            if (_stunnedTimer <= 0)
            {
                _isStunned = false;
                _speed = _normalSpeed;
                UpdatePostProcessing(0);

            }
        }
    }

    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        
        
    }

    public void Stun()
    {
        _isStunned = true;
        _stunnedTimer = _stunTime; 
        _speed = _normalSpeed * _stunnedSpeedMultiplier;
    }

    private void UpdatePostProcessing(float intensity)
    {
        // intensity is 1.0 at start of stun, 0.0 at end

        if (_chromatic != null)
        {
            // Smoothly blend from (Initial + 1.0) back down to Initial
            _chromatic.intensity.Override(_initialChromatic + intensity);
        }

        if (_lensDistort != null)
        {
            // Blend from (Initial - 0.4) back to Initial
            _lensDistort.intensity.Override(_initialDistort + (intensity * -0.4f));
        }

        if (_vignette != null)
        {
            // Blend from (Initial + 0.4) back to Initial
            _vignette.intensity.Override(_initialVignette + (intensity * 0.4f));
        }
    }
}

    
