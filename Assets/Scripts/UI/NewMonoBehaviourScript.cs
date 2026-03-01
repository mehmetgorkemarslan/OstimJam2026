using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void LoadFirstScene()
    {
        // Zamanı sıfırlamak çok önemli, yoksa oyun donuk kalabilir
        Time.timeScale = 1f; 
        SceneManager.LoadScene(1);
    }

    // Oyundan tamamen çıkar
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Unity Editor içindeysek oynatmayı durdurur
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Gerçek oyun çıktısında uygulamayı kapatır
            Application.Quit();
#endif
    }
}
