using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour
{
    enum AnimationState
    {
        None,
        Close,
        Open,
    }
    
    [SerializeField] private ClearWindowController clearWindowController;
    
    [SerializeField] private float delayBeforeClear = 0.5f;

    [SerializeField] private AnimationState animationState;
    private Animator animator;

    private bool isCleared = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animationState = AnimationState.Close;
        UpdateAnimation(animationState);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCleared) return;

        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (player.hasKey)
            {
                isCleared = true;
                player.StopMovement();
                animationState = AnimationState.Open;
                UpdateAnimation(animationState);
                StartCoroutine(ShowClearWindowWithDelay());
            }
        }
    }

    private IEnumerator ShowClearWindowWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeClear);

        if (clearWindowController != null)
        {
            clearWindowController.ShowClearWindow();
        }
    }

    void UpdateAnimation(AnimationState state)
    {
        if (animator == null) return;

        switch (state)
        {
            case AnimationState.Close:
                animator.SetBool("isOpen", false);
                break;
            case AnimationState.Open:
                animator.SetBool("isOpen", true);
                break;
            case AnimationState.None:
                break;
        }
    }
}