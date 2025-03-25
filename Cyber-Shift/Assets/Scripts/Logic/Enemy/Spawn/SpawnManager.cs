using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject pistolBotPrefab;
    [SerializeField] private GameObject assaultBotPrefab;
    [SerializeField] private GameObject shotgunBotPrefab; 

    public GameObject GetMobPrefab(SpawnPoint.MobType mobType)
    {
        switch (mobType)
        {
            case SpawnPoint.MobType.PistolBot:
                return pistolBotPrefab;
            case SpawnPoint.MobType.AssaultBot:
                return assaultBotPrefab;
            case SpawnPoint.MobType.ShotgunBot:
                return shotgunBotPrefab;
            default:
                return null;
        }
    }

    public void SpawnMob(SpawnPoint spawnPoint)
    {
        GameObject mobPrefab = GetMobPrefab(spawnPoint.mobType);
        if (mobPrefab != null)
        {
            Vector3 spawnPosition = spawnPoint.GetSpawnPosition();
            Instantiate(mobPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
