using UnityEngine;

public class DamageObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IPlayer player = other.GetComponent<IPlayer>();

        if(player != null)
        {
            player.TakeDamage();
        }
    }
}
