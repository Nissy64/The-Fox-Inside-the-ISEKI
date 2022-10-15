using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTeleporter : MonoBehaviour
{
    private GameObject currentTeleporter;

    public float teleportCoolDown = 1.5f;
    [SerializeField, ReadOnly]
    private float teleportCoolDownCounter;

    void Awake() 
    {
        ResetCoolDownTimer();
    }

    private void Update() 
    {
        TeleportCoolTime();

        if(teleportCoolDownCounter == 0)
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
            ResetCoolDownTimer();
        }
    }

    private void TeleportCoolTime()
    {
        if(teleportCoolDownCounter > 0)
        {
            teleportCoolDownCounter -= Time.deltaTime;
        }

        if(teleportCoolDownCounter < 0)
        {
            teleportCoolDownCounter = 0;
        }
    }

    private void ResetCoolDownTimer()
    {
        teleportCoolDownCounter = teleportCoolDown;
    }
}
