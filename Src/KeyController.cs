using UnityEngine;

public class KeyController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IPlayer>(out IPlayer player))
        {
            player.SetHasKey(true);
            Destroy(gameObject);
        }
    }
}