using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum MobType { PistolBot, AssaultBot, ShotgunBot }

    public MobType mobType;
    private float heightOffset = 1f;

    public Vector3 GetSpawnPosition()
    {
        return transform.position + Vector3.up * heightOffset;
    }
}
