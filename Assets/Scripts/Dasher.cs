using UnityEngine;
using System.Collections;

public class Dasher : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private Transform playerTransform;

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
        // If not assigned in inspector, try to find the player by tag
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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

        // Visual cue: Maybe vibrate or change color here
        Debug.Log("Dasher Locked On!");

        // 2. Wait Phase
        yield return new WaitForSeconds(pauseBeforeDash);

        // 3. Dash Phase
        Vector2 startPos = transform.position;
        Vector2 direction = (_targetPosition - startPos).normalized;

        // Use velocity for a snappy, non-physics-fighting dash
        _rb.linearVelocity = direction * dashSpeed;

        // Wait until we are close to the target or a timeout occurs

        float dashTimer = 0;
        while (Vector2.Distance(transform.position, _targetPosition) > 0.5f && dashTimer < 1f)
        {
            dashTimer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

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
}