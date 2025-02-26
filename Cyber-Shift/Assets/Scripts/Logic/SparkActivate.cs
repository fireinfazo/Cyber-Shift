using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkActivate : MonoBehaviour
{
    [SerializeField] private List<GameObject> sparks;
    void Start()
    {
        ActivateSparks();
    }

    private void ActivateSparks()
    {
        int num = Random.Range(0, sparks.Count + 1);

        for (int i = 0; i < num; i++)
        {
            sparks[i].SetActive(true);
        }
    }
}
