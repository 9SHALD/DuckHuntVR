using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Launcher launcher;
    [SerializeField] private Light_Gun gun;

    [SerializeField]private int ducksPerRound;
    private int hitThisRound;
    [SerializeField] private int hitTotal;
    [SerializeField] private int level = 25;
    [SerializeField] private int launchedThisRound;

    [SerializeField] public static float difficultyModifier;
    [SerializeField] private float timeBetweenRounds;
    [SerializeField] private float timeBetweenLevels;

    [SerializeField] private GameObject tempStartButton;

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
        //StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted) {
            if (launchedThisRound >= 10) {
                NextLevel();
            } else CheckBirds();
        }
    }
    

    private void CheckBirds() {
        if (gameStarted) { 
            if (hitThisRound == ducksPerRound || launcher.DuckCheck() == 0) { // May need to change this statement to only do the duck check
                StartCoroutine("NewRound");
            }
        } else ReloadPistol();
    }

    public void StartGame() {
        IncreaseDifficulty();
        StartCoroutine("NewRound");
        gameStarted = true;
    }

    // needs multi duck support
    private void launchDucks(int amount) {
        launcher.LaunchDuck();
        launchedThisRound++;
    }

    private void ReloadPistol() {
        gun.Reload();
    }

    public void Hit() {
        hitThisRound++;
        hitTotal++;
    }

    private void IncreaseDifficulty() {
        level++;
        if (difficultyModifier < 6)
        difficultyModifier = 1 + level / 5;
    }

    private IEnumerator NewRound() {
        ReloadPistol();
        yield return new WaitForSeconds(timeBetweenRounds);
        hitThisRound = 0;
        launchDucks(ducksPerRound);
        StopCoroutine("NewRound");
    }

    private void NextLevel() {
        if (hitTotal >= 5) {
            IncreaseDifficulty();
            hitTotal = 0;
            launchedThisRound = 0;
        } else GameOver();
    }

    private void GameOver() {
        tempStartButton.SetActive(true);
        SaveScore();
        ResetGame();
    }

    private void SaveScore() {
        //save score if highscore
    }

    private void ResetGame() {
        gameStarted = false;
        hitThisRound = 0;
        hitTotal = 0;
        launchedThisRound = 0;
        difficultyModifier = 0;
        level = 0;
    }
}
