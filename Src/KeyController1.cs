using UnityEngine;

public class KeyController1 : MonoBehaviour
{
    private bool _isCollected;

    private Collider _keyCollider;
    private Renderer _keyRenderer;

    private void Start()
    {
        _keyCollider = GetComponent<Collider>();
        _keyRenderer = GetComponentInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_isCollected)
        {
            return;
        }

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if(player == null)
        {
            return;
        }

        _isCollected = true;
        player.SetHasKey(true);

        gameObject.SetActive(false);

        Debug.Log("Œ®‚ªŽæ“¾‚³‚ê‚Ü‚µ‚½");
    }
}
