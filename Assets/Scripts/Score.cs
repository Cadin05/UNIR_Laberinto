using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] int score;
    TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        PlayerPrefs.SetInt("Score", 0);
        score = 0;
    }

    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        scoreText.text = $"Score: {score}";
        PlayerPrefs.SetInt("Score", score);
    }
}
