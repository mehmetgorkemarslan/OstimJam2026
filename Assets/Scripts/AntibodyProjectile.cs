using UnityEngine;

public class AntibodyProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;
    private float _speed;
    private Vector2 _direction;

    public void Setup(Vector2 dir, float speed)
    {
        _direction = dir;
        _speed = speed;

        // Point the "Y" shape toward the direction of travel
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifeTime); // Auto-cleanup
    }

    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Trigger damage logic on player here
            Debug.Log("Player hit by Antibody!");
            Destroy(gameObject);
        }
    }
}