using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance = null;
    [SerializeField] private ParticleSystem DuckDeath;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    public void PlayDuckDeath(Vector3 position, Quaternion rotation) {
        ParticleSystem playTo = CreateNewParticleSystem(DuckDeath);
        playTo.transform.position = position;
        playTo.transform.rotation = rotation;

        playTo.Play();

        StartCoroutine(ParticleDeathTimer(playTo));
    }

     private ParticleSystem CreateNewParticleSystem(ParticleSystem system)
    {
        GameObject newObject = Instantiate(system.gameObject, transform.position, Quaternion.identity);
        newObject.name = "Particle System";

        return newObject.GetComponent<ParticleSystem>();
    }

    private IEnumerator ParticleDeathTimer(ParticleSystem system) {
        yield return new WaitForSeconds(system.main.duration);
        system.Stop();
        Destroy(system.gameObject);
    }
}
