using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // TextMeshPro kullanıyorsan

public class LevelSelector : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string scenePrefix = "Level"; // Sahne isimlerinin başı (Örn: Level1, Level2)
    [SerializeField] private int totalLevels = 3; // Toplam kaç level butonu olacak

    [Header("UI References")]
    [SerializeField] private GameObject buttonPrefab; // Oluşturulacak butonun prefabı
    [SerializeField] private Transform contentParent; // Butonların içine dizileceği yer (Grid veya Vertical Layout)

    private void Start()
    {
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        // Önce içeriyi temizle
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Levelleri oluştur
        for (int i = 1; i <= totalLevels; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, contentParent);
            
            // Butonun üzerindeki yazıyı güncelle (TextMeshPro varsa)
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null) buttonText.text = i.ToString();

            // Butona tıklama olayını kodla ekle
            int levelIndex = i; // Closure sorunu yaşamamak için yerel değişken
            Button btn = newButton.GetComponent<Button>();
            btn.onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    public void LoadLevel(int index)
    {
        // Zamanı sıfırlamayı unutmuyoruz
        Time.timeScale = 1f;
        
        // Örnek: "Level1" sahnesini yükler
        string sceneName = scenePrefix + index;
        
        // Sahnenin Build Settings'de olduğundan emin olmalısın
        SceneManager.LoadScene(sceneName);
    }
}