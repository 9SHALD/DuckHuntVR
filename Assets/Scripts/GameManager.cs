using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    [SerializeField] private Launcher launcher;
    [SerializeField] private Light_Gun gun;

    public bool _2Ducks;
    private int hitThisLevel;
    public int destroyedThisLevel;
    [SerializeField] private int hitTotal;
    [SerializeField] private int level = 25;
    [SerializeField] private int launchedThisLevel;

    [SerializeField] public static float difficultyModifier = 1;
    [SerializeField] private float timeBetweenRounds;
    [SerializeField] private float timeBetweenLevels;

    [SerializeField] private GameObject tempStartButton;
    [SerializeField] private GameObject tempStartButton2;

    [SerializeField] private int[] DucksToHit;
    [SerializeField] private int[] BonusPoints;

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
        if (launcher.DuckCheck() == 0) {
            StartCoroutine("NewRound");
        }
    }

    [ContextMenu("Start Game")]
    private void _Start() { StartGame(false); }
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
    }

    private void ReloadPistol() {
        gun.Reload();
    }

    public void Hit() {
        hitThisLevel++;
        hitTotal++;
    }

    [ContextMenu("Increase Difficulty")]
    private void IncreaseDifficulty() {
        level++;
        if (difficultyModifier < 2)
            difficultyModifier *= 1.02f;
    }

    private IEnumerator NewRound() {
        ReloadPistol();
        yield return new WaitForSeconds(timeBetweenRounds);
        hitThisLevel = 0;
        launchDucks(_2Ducks);
        StopCoroutine("NewRound");
    }

    private void NextLevel() {
        int temp = level - 1;
        if (temp > DucksToHit.Length) {
            temp = DucksToHit.Length;
        }
        if (hitTotal >= DucksToHit[temp]) {
            IncreaseDifficulty();
            hitTotal = 0;
            launchedThisLevel = 0;
            destroyedThisLevel = 0;
        } else GameOver();
    }

    private void AddBonusPoints() {
        int bonus = level - 1;
        if (bonus > BonusPoints.Length) {
            bonus = BonusPoints.Length;
        }

        //Add temp to points
    }

    private void GameOver() {
        tempStartButton.SetActive(true);
        tempStartButton2.SetActive(true);
        SaveHighScore();
        ResetGame();
    }

    private void SaveHighScore() {
        //save score if highscore
    }

    private void ResetGame() {
        gameStarted = false;
        hitTotal = 0;
        launchedThisLevel = 0;
        destroyedThisLevel = 0;
        difficultyModifier = 0;
        level = 0;
    }
}
