using UnityEngine;
using System.Collections;

public class Dasher : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private Transform playerTransform;
    private LineRenderer _lineRenderer;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float pauseBeforeDash = 1.0f;
    [SerializeField] private float dashCooldown = 2.0f;

    private Rigidbody2D _rb;
    private Vector2 _targetPosition;
    private bool _isActionInProgress = false;

    [SerializeField] private GameObject explosionObj;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        // If not assigned in inspector, try to find the player by tag
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Ensure the line is off at the start
        _lineRenderer.enabled = false;
    }

    void FixedUpdate()
    {
        if (_isActionInProgress) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // State: Sleeping vs Searching
        if (distanceToPlayer <= detectionRange)
        {
            StartCoroutine(PerformDashSequence());
        }
    }

    private IEnumerator PerformDashSequence()
    {
        _isActionInProgress = true;

        // 1. Aiming Phase (Store position)
        _targetPosition = playerTransform.position;

        // Show the Telegraph Line
        _lineRenderer.enabled = true;

        // Update line points: Start at enemy, end at target position
        // We can even extend the line further to show the "overshoot"
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, _targetPosition + ((_targetPosition - (Vector2)transform.position).normalized * 2f));

        Debug.Log("Dasher Locked On!");




        // 2. Wait Phase
        yield return new WaitForSeconds(pauseBeforeDash);

        // Hide the line right as we launch
        _lineRenderer.enabled = false;

        // 3. Dash Phase
        Vector2 startPos = transform.position;
        Vector2 direction = (_targetPosition - startPos).normalized;

        // Use velocity for a snappy, non-physics-fighting dash
        _rb.linearVelocity = direction * dashSpeed;


        float dashDuration = 0.5f; // Max time to dash
        yield return new WaitForSeconds(dashDuration);

        // 4. Cool Down Phase
        _rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);

        _isActionInProgress = false;
    }

    // Visualize the range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Instantiate(explosionObj, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void Update()
    {
        if (_lineRenderer.enabled)
        {
            _lineRenderer.SetPosition(0, transform.position);
            // Optional: Comment the line below if you want the dash 
            // to lock onto the INITIAL spot and not track the player
          //  _lineRenderer.SetPosition(1, playerTransform.position);
        }
    }
}