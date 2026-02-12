using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

        if (PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("Best Score"))
        {
            PlayerPrefs.SetInt("Best Score", PlayerPrefs.GetInt("Score"));
        }
        textScore.text = $"Score: {PlayerPrefs.GetInt("Score")} \nBest Score: {PlayerPrefs.GetInt("Best Score")}";
    }
}
