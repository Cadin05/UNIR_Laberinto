using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    int score;
    TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        PlayerPrefs.SetInt("Score", 0);
    }

    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        scoreText.text = $"Score: {score}";
        PlayerPrefs.SetInt("Score", score);

        if (score > PlayerPrefs.GetInt("Best Score"))
        {
            PlayerPrefs.SetInt("Best Score", score);
        }
    }
}
