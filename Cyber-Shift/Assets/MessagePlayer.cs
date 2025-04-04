using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Transform player;
    [SerializeField] private AudioClip message;
    private float interactionDistance = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(player.transform.position, player.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    PlayMessage();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void PlayMessage()
    {
        if (_audioSource != null)
        {
            if (message != null) 
            {
                _audioSource.PlayOneShot(message);
            }
        }
    }

    
}
