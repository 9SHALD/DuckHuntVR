using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Launcher launcher;
    [SerializeField] private Light_Gun gun;
    // Start is called before the first frame update
    void Start()
    {
        if (launcher == null) {
            Debug.LogError("Launcher is not added to gamemanager");
        }
        if (gun == null) {
            Debug.LogError("Gun is not added to gamemanager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
