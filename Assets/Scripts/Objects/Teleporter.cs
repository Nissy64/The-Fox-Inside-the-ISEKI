using UnityEngine;

namespace Objects 
{
    public class Teleporter : MonoBehaviour
    {
        public Transform destination;

        public Transform GetDestination()
        {
            return destination;
        }
    }
}