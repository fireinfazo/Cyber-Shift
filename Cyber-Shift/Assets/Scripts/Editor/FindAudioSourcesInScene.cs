#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class DisablePlayOnAwakeForAllAudioSources
{
    [MenuItem("Tools/Disable All PlayOnAwake")]
    public static void DisablePlayOnAwake()
    {
        // Находим все AudioSource на сцене (включая неактивные)
        AudioSource[] allAudioSources = GameObject.FindObjectsOfType<AudioSource>(true);

        int disabledCount = 0;

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource.playOnAwake)
            {
                audioSource.playOnAwake = false;
                disabledCount++;
                Debug.Log($"Disabled PlayOnAwake on: {audioSource.gameObject.name}", audioSource.gameObject);
            }
        }

        Debug.Log($"Disabled PlayOnAwake on {disabledCount} AudioSources");
    }
}
#endif