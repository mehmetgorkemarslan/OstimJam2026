using UnityEngine;

public class YProtein : MonoBehaviour
{
    public float speed;
    public GameObject explosionObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody2D>().MoveRotation(Random.Range(0f, 360f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GetComponent<Rigidbody2D>().linearVelocity = (col.transform.position - transform.position) * speed;
            Invoke("DestroySelf", 10f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //do damage to player
            Instantiate(explosionObj, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void DestroySelf()
    {
        Instantiate(explosionObj, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
