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
    [SerializeField] private float highScore;
    [SerializeField] private TextMeshPro ScoreText;


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
            highScore = PlayerPrefs.GetFloat("HighScore");
        else highScore = 0;
    }

    public void AddScore(bool superDuck = false) {
        int pointsToAdd = GameManager.instance.level - 1;
        if (pointsToAdd > Points.Length) {
            pointsToAdd = Points.Length;
        }
        if (!superDuck) {
            score += Points[pointsToAdd];
        } else {
            score += (Points[pointsToAdd] * 2);
        }
        ScoreText.text = "Score: \n" + score;
            
    }

    public void SetHighScore() {
        if (score > highScore) {
            highScore = score;
        }
    }

    public void AddBonusPoints() {
        int bonus = GameManager.instance.level - 1;
        if (bonus > BonusPoints.Length) {
            bonus = BonusPoints.Length;
        }
        score += BonusPoints[bonus];
    }

    public void ResetHighScores() {
        highScore = 0;
        PlayerPrefs.SetFloat("HighScore", 0);
    }

    private void OnDestroy() {
        SetHighScore();
        PlayerPrefs.SetFloat("HighScore", highScore);
    }

    public void ClearOldScore() {
        score = 0;
        ScoreText.text = "Score: \n" + score;
    }
}
