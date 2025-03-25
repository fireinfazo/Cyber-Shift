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

            //Destroy(gameObject); //крч мы не спавнили кучу мобов с 1 тригера они одноразовые
            gameObject.SetActive(false);
        }
    }
}

