using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogManager : MonoBehaviour {
    public static DogManager instance;
    [Header("References")]
    public Transform dogParent;

    [SerializeField]private Dog[] dogs;

    internal bool gotDogs;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        GetDogs();
    }

    private void GetDogs() {
        if (dogParent != null) {
            dogs = dogParent.GetComponentsInChildren<Dog>();
            gotDogs = true;
        }
    }

    private void Update() {
    }

    public void LaunchDog() {
        int dogIndex = Random.Range(0, dogs.Length);
        StartCoroutine(dogs[dogIndex].ShowDog());
    }
}
