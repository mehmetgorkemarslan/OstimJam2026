using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private float _bloodflowSpeed = 2f;

    private Rigidbody2D _rb;
    private Vector2 _targetPos;
    Vector2 input;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _targetPos = _rb.position + input * _speed * Time.fixedDeltaTime + new Vector2(_bloodflowSpeed * Time.fixedDeltaTime, 0);

        _rb.MovePosition(_targetPos);
    }

    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        
        
    }

}

    
