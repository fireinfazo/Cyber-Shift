using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private SpawnManager spawnManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var spawnPoint in spawnPoints)
            {
                if (spawnPoint != null)
                {
                    spawnManager.SpawnMob(spawnPoint);
                }
            }

            gameObject.SetActive(false);
        }
    }
}

