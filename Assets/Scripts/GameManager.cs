using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    [SerializeField] private Launcher launcher;
    [SerializeField] private Light_Gun gun;

    public bool _2Ducks;
    private int hitThisLevel;
    public int destroyedThisLevel;
    [SerializeField] private int hitTotal;
    public int level = 0;
    [SerializeField] private int launchedThisLevel;

    [SerializeField] public static float difficultyModifier = 1;
    [SerializeField] private float timeBetweenRounds;
    [SerializeField] private float timeBetweenLevels;

    [SerializeField] private GameObject tempStartButton;
    [SerializeField] private GameObject tempStartButton2;
    [SerializeField] private TextMeshPro TempLevelText;
    [SerializeField] private TextMeshPro TempOutOFText;

    [SerializeField] private int[] DucksToHit;
    [SerializeField] private int[] BonusPoints;
    private bool once;

    private bool gameStarted = false;
    // Start is called before the first frame update
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (launcher == null) {
            Debug.LogError("Launcher is not added to gamemanager");
        }
        if (gun == null) {
            Debug.LogError("Gun is not added to gamemanager");
        }
        if(tempStartButton == null && tempStartButton2 == null) {
            Debug.LogError("You can't start without a start button silly");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted) {
            if (launchedThisLevel >= 10 && destroyedThisLevel >= 10) {
                NextLevel();
            } else CheckBirds();
        } else ReloadPistol();
    }
    

    private void CheckBirds() {
        if (launcher.DuckCheck() == 0 && !once) {
            once = true;
            StartCoroutine("NewRound");
        }
    }

    [ContextMenu("Start Game")]
    private void _Start() { StartGame(false); } // temp
    public void StartGame(bool multiDuck) {
        _2Ducks = multiDuck;
        IncreaseDifficulty();
        StartCoroutine("NewRound");
        gameStarted = true;
    }

    // needs multi duck support
    private void launchDucks(bool MultiDuck) {
        if (MultiDuck) {
            launcher.LaunchDucks();
            launchedThisLevel += 2;
        } else {
            launcher.LaunchDuck();
            launchedThisLevel++;
        }
        once = false;
    }

    private void ReloadPistol() {
        gun.Reload();
    }

    public void Hit(bool isSuperDuck = false) {
        hitThisLevel++;
        hitTotal++;
        ScoreManager.instance.AddScore();
        TempOutOFText.text = hitThisLevel + " / 10";
    }

    [ContextMenu("Increase Difficulty")]
    private void IncreaseDifficulty() {
        level++;
        if (difficultyModifier < 2.5)
            difficultyModifier *= 1.05f;
    }

    private IEnumerator NewRound() {
        ReloadPistol();
        yield return new WaitForSeconds(timeBetweenRounds);
        launchDucks(_2Ducks);
        StopCoroutine("NewRound");
    }

    private void NextLevel() {
        int temp = level - 1;
        if (temp > DucksToHit.Length) {
            temp = DucksToHit.Length;
        }
        if (hitThisLevel >= DucksToHit[temp]) {
            if (hitThisLevel == 10) {
                ScoreManager.instance.AddBonusPoints();
            }
            IncreaseDifficulty();
            hitThisLevel = 0;
            launchedThisLevel = 0;
            destroyedThisLevel = 0;
            TempLevelText.text = "Level - " + level;
            TempOutOFText.text = hitThisLevel + " / 10";
        } else GameOver();
    }


    private void GameOver() {
        tempStartButton.SetActive(true);
        tempStartButton2.SetActive(true);
        SaveHighScore();
        ResetGame();
        TempLevelText.text = "Select Mode";
        TempOutOFText.text = " ";
    }

    private void SaveHighScore() {
        ScoreManager.instance.SetHighScore();
    }

    private void ResetGame() {
        gameStarted = false;
        hitTotal = 0;
        hitThisLevel = 0;
        launchedThisLevel = 0;
        destroyedThisLevel = 0;
        difficultyModifier = 0;
        level = 0;
    }
}
