using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTeleporter : MonoBehaviour
{
    private GameObject currentTeleporter;

    public float teleportCooldown = 1.5f;
    [SerializeField, ReadOnly]
    private float teleportCooldownCounter;

    void Awake() 
    {
        ResetCooldownTimer();
    }

    private void Update() 
    {
        TeleportCoolTimer();

        if(teleportCooldownCounter == 0)
        {
            Teleport();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Teleporter"))
        {
            currentTeleporter = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(!other.CompareTag("Teleporter")) return;
        if(other.gameObject == currentTeleporter)
        {
            currentTeleporter = null;
        }
    }

    private void Teleport()
    {
        if(currentTeleporter != null)
        {
            Teleporter teleporter = currentTeleporter.GetComponent<Teleporter>();

            transform.position = teleporter.GetDestination().position;
            transform.rotation = teleporter.GetDestination().rotation;
            ResetCooldownTimer();
        }
    }

    private void TeleportCoolTimer()
    {
        if(teleportCooldownCounter > 0)
        {
            teleportCooldownCounter -= Time.deltaTime;
        }

        if(teleportCooldownCounter < 0)
        {
            teleportCooldownCounter = 0;
        }
    }

    private void ResetCooldownTimer()
    {
        teleportCooldownCounter = teleportCooldown;
    }
}
