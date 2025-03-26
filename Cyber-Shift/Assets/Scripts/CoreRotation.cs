using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotateAmount;
    void Update()
    {
        transform.Rotate(rotateAmount * Time.deltaTime);
    }
}
