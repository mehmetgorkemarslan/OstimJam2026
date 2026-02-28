using UnityEngine;
using System.Collections;

public class RadialEnemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject antibodyPrefab;
    [SerializeField] private int antibodyCount = 8;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float fireRate = 3f;
    [SerializeField] private float range = 8f;
    Transform player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnRoutine());
        
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            if(Vector2.Distance(player.position, transform.position) < range)
                SpawnRadialBurst();
        }
    }

    // Visualize the range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void SpawnRadialBurst()
    {
        float angleStep = 360f / antibodyCount;
        float angle = 0f;

        for (int i = 0; i < antibodyCount; i++)
        {
            // Calculate direction vector from angle
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 moveVector = new Vector2(dirX, dirY);

            // Spawn and setup
            GameObject bullet = Instantiate(antibodyPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<AntibodyProjectile>().Setup(moveVector, projectileSpeed);

            angle += angleStep;
        }
    }
}