using TMPro;
using UnityEngine;

public class WinScore : MonoBehaviour
{
    TextMeshProUGUI textScore;

    private void Awake()
    {
        textScore = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        textScore.text = $"Score: {PlayerPrefs.GetInt("Score")} \nBest Score: {PlayerPrefs.GetInt("Best Score")}";
    }
}
