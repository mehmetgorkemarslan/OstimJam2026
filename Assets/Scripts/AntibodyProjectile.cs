using UnityEngine;

public class AntibodyProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;
    private float _speed;
    private Vector2 _direction;
    public GameObject explosionObj;
    
    public void Setup(Vector2 dir, float speed)
    {
        _direction = dir;
        _speed = speed;

        // Point the "Y" shape toward the direction of travel
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Invoke(nameof(DestroySelf), lifeTime);


    }

    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("RadialEnemy") && !collision.gameObject.CompareTag("RadialEnemyProjectile"))
        {
            // Trigger damage logic on player here
            if(collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player hit by Antibody!");
                collision.gameObject.GetComponent<PlayerController>().Stun();
            }
            
            // Spawn explosion effect
            if (explosionObj != null)
            {
                Instantiate(explosionObj, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    void DestroySelf()
    {
        if (explosionObj != null)
        {
            Instantiate(explosionObj, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); 
    }
}