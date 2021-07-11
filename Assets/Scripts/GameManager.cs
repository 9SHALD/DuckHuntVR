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

    [SerializeField] private GameObject uiMenu;
    [SerializeField] private GameObject onPlayUI;
    [SerializeField] private TextMeshPro LevelText;
    [SerializeField] private TextMeshPro DucksOutOFText;

    [SerializeField] private int[] DucksToHit;
    [SerializeField] private int[] BonusPoints;

    [SerializeField] AudioClip gameOverClip;
    [SerializeField] AudioClip hitClip;

    private bool once;
    private bool firstDuckLaunched;

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
        if(uiMenu == null) {
            Debug.LogError("You can't start without the start menu silly");
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

    public void StartGame(bool multiDuck = false) {
        firstDuckLaunched = false;
        onPlayUI.SetActive(true);
        _2Ducks = multiDuck;
        level++;
        StartCoroutine("NewRound");
        LevelText.text = "Level " + level;
        DucksOutOFText.text = "0 / 10";
        gameStarted = true;
    }

    private void launchDucks(bool MultiDuck) {
        if (MultiDuck) {
            launcher.SetDucksToLaunchInt(2);
            launcher.LaunchDucks();
            launchedThisLevel += 2;
        } else {
            launcher.SetDucksToLaunchInt(1);
            launcher.LaunchDuck();
            launchedThisLevel++;
        }
        once = false;
    }

    private void ReloadPistol() {
        gun.Reload();
    }

    public void Hit(bool isSuperDuck = false) {
        SoundManager.Instance.Play(hitClip, .5f);
        hitThisLevel++;
        hitTotal++;
        ScoreManager.instance.AddScore(isSuperDuck);
        DucksOutOFText.text = hitThisLevel + " / 10";
    }

    [ContextMenu("Increase Difficulty")]
    private void IncreaseDifficulty() {
        level++;
        if (difficultyModifier < 2.5)
            difficultyModifier *= 1.05f;
    }

    private IEnumerator NewRound() {
        ReloadPistol();
        if(firstDuckLaunched)
            DogManager.instance.LaunchDog();
        yield return new WaitForSeconds(timeBetweenRounds);
        if (gameStarted) {
            firstDuckLaunched = true;
            launchDucks(_2Ducks);
        }
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
            LevelText.text = "Level " + level;
            DucksOutOFText.text = hitThisLevel + " / 10";
        } else GameOver();
    }


    public void GameOver() {
        SoundManager.Instance.Play(gameOverClip, 0.5f);
        uiMenu.SetActive(true);
        onPlayUI.SetActive(false);
        SaveHighScore();
        ResetGame();
        LevelText.text = " ";
        DucksOutOFText.text = " ";
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
        difficultyModifier = 1;
        level = 0;
        ScoreManager.instance.ClearOldScore();
    }
}
