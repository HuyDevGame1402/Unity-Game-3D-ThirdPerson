using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    [Header("ZombieSpawn Var")]
    public GameObject zombiePrefab;
    public Transform zombieSpawnPosition;
    public GameObject dangerZone1;
    private float repeatCycle = 1f;

    [Header("Sounds")]
    public AudioClip dangerZoneSound;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            InvokeRepeating("EnemySpawner", 1f, repeatCycle);
            audioSource.PlayOneShot(dangerZoneSound);
            StartCoroutine(DangerZoneTimer());
            Destroy(gameObject, 10f);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void EnemySpawner()
    {
        Instantiate(zombiePrefab, zombieSpawnPosition.position, zombieSpawnPosition.rotation);
    }

    private IEnumerator DangerZoneTimer()
    {
        dangerZone1.SetActive(true);
        yield return new WaitForSeconds(5f);
        dangerZone1.SetActive(false);
    }
}
