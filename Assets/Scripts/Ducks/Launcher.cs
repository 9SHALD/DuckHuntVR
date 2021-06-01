using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {

    [Header("References")]
    public GameObject targetPrefab;

    public Transform launcherParent;

    private Transform[] launchers;

    [Header("Settings")]
    public Vector2 targetSpeedRange;
    public Vector2 duckTimerMinMax;
    public int maxTargets;

    public List<Duck> launchedTargets = new List<Duck>();

    private bool launch;
    private bool launchingDuck;

    internal bool gotLaunchers;

    private void Awake() {
        GetLaunchers();
        ToggleLaunching(true);
    }

    private void GetLaunchers() {
        if (launcherParent != null) {
            launchers = launcherParent.GetComponentsInChildren<Transform>();
            gotLaunchers = true;
        }
    }

    private void Update() {
        if (launch) {
            if (!launchingDuck) {
                if (launchedTargets.Count < maxTargets) {
                    StartCoroutine(LaunchTimer());
                }
            }
        }
    }

    public void ToggleLaunching(bool toggle) {
        launch = toggle;

        if (!toggle) {

            StopAllCoroutines();

            launchingDuck = false;
        }
    }

    private void LaunchDuck() {
        int laucherIndex = Random.Range(1, launchers.Length);
        GameObject newTarget = Instantiate(targetPrefab, launchers[laucherIndex].position, launchers[laucherIndex].rotation);

        Rigidbody rb = newTarget.GetComponent<Rigidbody>();
        rb.velocity = rb.transform.forward * Random.Range(targetSpeedRange.x, targetSpeedRange.y);

        Duck target = newTarget.GetComponent<Duck>();
        target.launchedBy = this;

        launchedTargets.Add(newTarget.GetComponent<Duck>());
    }

    private IEnumerator LaunchTimer() {
        launchingDuck = true;
        float timer = Random.Range(duckTimerMinMax.x, duckTimerMinMax.y);
        yield return new WaitForSeconds(timer);

        LaunchDuck();

        launchingDuck = false;
    }
}
