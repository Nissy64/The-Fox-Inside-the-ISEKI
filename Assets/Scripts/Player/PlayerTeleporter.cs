using UnityEngine;
using Objects;

namespace Player
{
    public class PlayerTeleporter : MonoBehaviour
    {
        private GameObject currentTeleporter;

        public TrailRenderer playerTrail;
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

        private void OnTriggerEnter2D(Collider2D collision) 
        {
            if(collision.CompareTag("Teleporter"))
            {
                currentTeleporter = collision.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if(!collision.CompareTag("Teleporter")) return;
            if(collision.gameObject == currentTeleporter)
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
                playerTrail.emitting = false;
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
}