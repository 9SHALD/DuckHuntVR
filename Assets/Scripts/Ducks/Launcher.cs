using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {

    [Header("References")]
    public GameObject duckPrefab;
    public GameObject superDuckPrefab;

    public Transform launcherParent;

    private Transform[] launchers;

    [Header("Settings")]
    public Vector2 targetSpeedRange;
    public Vector2 duckTimerMinMax;
    public int maxTargets;

    public List<Duck> launchedTargets = new List<Duck>();

    internal bool gotLaunchers;

    private int ducksToLaunch;

    private void Awake() {
        GetLaunchers();
    }

    private void GetLaunchers() {
        if (launcherParent != null) {
            launchers = launcherParent.GetComponentsInChildren<Transform>();
            gotLaunchers = true;
        }
    }

    private void Update() { 

    }

    public void LaunchDucks() {
        LaunchDuck();
        StartCoroutine("LaunchTimer");
    }

    public void LaunchDuck() {
        int laucherIndex = Random.Range(1, launchers.Length);
        GameObject newTarget = null;
        bool superDuck = false;
        if (Random.Range(0, 100) < 90) {
            newTarget = Instantiate(duckPrefab, launchers[laucherIndex].position, launchers[laucherIndex].rotation);
        } else {
            newTarget = Instantiate(superDuckPrefab, launchers[laucherIndex].position, launchers[laucherIndex].rotation);
            superDuck = true;
        }
        Rigidbody rb = newTarget.GetComponent<Rigidbody>();

        rb.velocity = rb.transform.forward * Random.Range(targetSpeedRange.x, targetSpeedRange.y) * GameManager.difficultyModifier;

        Duck target = newTarget.GetComponent<Duck>();
        target.launchedBy = this;

        launchedTargets.Add(newTarget.GetComponent<Duck>());
        ducksToLaunch -= 1;
        if (superDuck)
            target.MakeSuperDuck();
    }

    private IEnumerator LaunchTimer() {
        float timer = Random.Range(duckTimerMinMax.x, duckTimerMinMax.y);
        yield return new WaitForSeconds(timer);

        LaunchDuck();
        StopCoroutine("LaunchTimer");
    }

    public int DuckCheck() {
        if (ducksToLaunch == 0)
            return launchedTargets.Count;
        else return 1;
    }

    public void SetDucksToLaunchInt(int ducks) {
        ducksToLaunch = ducks;
    }
}
