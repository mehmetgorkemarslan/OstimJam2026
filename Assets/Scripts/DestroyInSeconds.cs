using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{
    public float time = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(DestroySelf), time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

}
