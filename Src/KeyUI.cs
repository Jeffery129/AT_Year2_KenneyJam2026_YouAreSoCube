using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    [SerializeField]private PlayerController _player;
    [SerializeField] private RawImage _keyImage;

    private Color noKeyColor =
        new Color(0.15f, 0.15f, 0.15f, 1f); // çïÇ≠Ç∑ÇÈ

    private void Start()
    {
        if (_player == null)
        {
            _player = FindFirstObjectByType<PlayerController>();
        }

        if(_keyImage == null)
        {
            _keyImage = GetComponent<RawImage>();
        }
    }

    private void Update()
    {


        if(_player == null || _keyImage == null)
        {
            return;
        }

        if(_player.hasKey)
        {
            _keyImage.color = Color.white;
        }
        else
        {
            _keyImage.color = noKeyColor;
        }
    }
}
