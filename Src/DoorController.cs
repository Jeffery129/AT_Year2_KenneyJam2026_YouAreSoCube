using UnityEngine;

public class DoorController : MonoBehaviour, IOperatable
{
    enum AnimationState
    {
        None,
        Open,
        Close,
    }

    [Header("Animation State")]
    [SerializeField] private AnimationState animationState;

    [Header("Default State")]
    public bool isDefaultOpen = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (isDefaultOpen)
        {
            animationState = AnimationState.Open;
            UpdateAnimation(animationState);
            animator.Play("open", 0, 1.0f);
        }
        else
        {
            animationState = AnimationState.Close;
            UpdateAnimation(animationState);
            animator.Play("close", 0, 1.0f);
        }
    }

    public void SetOperating(bool state)
    {
        if (state == true)
        {
            animationState = AnimationState.Open;
        }
        else
        {
            animationState = AnimationState.Close;
        }

        UpdateAnimation(animationState);
    }

    public void ToggleOperating()
    {
        if (animationState == AnimationState.Open)
        {
            animationState = AnimationState.Close;
        }
        else
        {
            animationState = AnimationState.Open;
        }

        UpdateAnimation(animationState);
    }

    void UpdateAnimation(AnimationState state)
    {
        if (animator == null) return;

        switch (state)
        {
            case AnimationState.Open:
                animator.SetBool("isOpen", true);
                break;

            case AnimationState.Close:
                animator.SetBool("isOpen", false);
                break;

            case AnimationState.None:
                break;
        }
    }
}
