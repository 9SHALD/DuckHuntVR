using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;
    [SerializeField] private int[] Points;
    [SerializeField] private int[] BonusPoints;
    [SerializeField] private float score;
    [SerializeField] private float topScore;
    [SerializeField] private TextMeshPro ScoreText;
    [SerializeField] private TextMeshPro ScoreTextGameOver;
    [SerializeField] private TextMeshPro TopScoreText;
    [SerializeField] private TextMeshPro TopScoreTextGameOver;


    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
            topScore = PlayerPrefs.GetFloat("HighScore");
        else topScore = 0;
        TopScoreText.text = "Top Score \n" + topScore.ToString();
    }

    public void AddScore(bool superDuck = false) {
        int pointsToAdd = GameManager.instance.level - 1;
        if (pointsToAdd > Points.Length) {
            pointsToAdd = Points.Length - 1;
        }
        if (!superDuck) {
            score += Points[pointsToAdd];
        } else {
            score += (Points[pointsToAdd] * 2);
        }
        ScoreText.text = "Score: \n" + score;
            
    }

    public void SetHighScore() {
        if (score > topScore) {
            topScore = score;
            TopScoreText.text = "Top Score \n" + topScore.ToString();
            TopScoreTextGameOver.text = "Top Score \n" + topScore.ToString();
        }
    }

    public void AddBonusPoints() {
        int bonus = GameManager.instance.level - 1;
        if (bonus > BonusPoints.Length) {
            bonus = BonusPoints.Length - 1;
        }
        score += BonusPoints[bonus];
        ScoreText.text = "Score: \n" + score;
    }

    public void ResetTopScores() {
        topScore = 0;
        PlayerPrefs.SetFloat("HighScore", 0);
    }

    private void OnDestroy() {
        SetHighScore();
        PlayerPrefs.SetFloat("HighScore", topScore);
    }

    public void ClearOldScore() {
        ScoreTextGameOver.text = "Score \n" + score;
        TopScoreTextGameOver.text = "Top Score \n" + topScore.ToString();
        score = 0;
        ScoreText.text = "Score: \n" + score;
    }
}
