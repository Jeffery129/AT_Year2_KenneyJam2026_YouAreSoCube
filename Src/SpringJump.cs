using UnityEngine;

public class SpringJump : MonoBehaviour
{
    [Header("Ç∂Ç·ÇÒÇ’ÇœÇÌ")]
    public float upwardPower;  // Å™
    public float forwardPower;  // â°

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IPlayer>(out IPlayer player))
        {
            player.StopMovement();

            if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;

                Vector3 springForece =
                    Vector3.up * upwardPower +
                    transform.forward * forwardPower;

                rb.AddForce(springForece,
                    ForceMode.VelocityChange);
            }
        }
    }
}
