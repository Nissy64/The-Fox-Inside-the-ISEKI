using UnityEngine;
using Player;

namespace Objects 
{
    public class Needle : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

                StartCoroutine(playerMovement.PlayerGameOver());
            }
        }
    }
}