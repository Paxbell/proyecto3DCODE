using UnityEngine;

public class zombieSpawn : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnDelay = 3f; // Tiempo que toma la animación antes de que se mueva
    public Animator spawnAnimator;

    private bool spawning = false;

    void Start()
    {
        if (CompareTag("spawn") && !spawning)
        {
            StartCoroutine(SpawnZombie());
        }
    }

    System.Collections.IEnumerator SpawnZombie()
    {
        spawning = true;

        // Reproduce animación de escarvar
        if (spawnAnimator != null)
        {
            spawnAnimator.SetTrigger("spawn");
        }

        yield return new WaitForSeconds(spawnDelay);

        GameObject zombie = Instantiate(zombiePrefab, transform.position, transform.rotation);
        zombie.SetActive(true);
    }
}
