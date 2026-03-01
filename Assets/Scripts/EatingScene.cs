using UnityEngine;
using UnityEngine.SceneManagement;

public class EatingScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(LoadNextScene), 3f); // Delay to allow any end-of-level effects to play

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
