using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    enum AnimationState
    {
        None,
        On,
        Off,
    }

    [Header("Animation State")]
    [SerializeField] private AnimationState animationState;

    [Header("Turn On Target")]
    public GameObject turnOnTarget;

    [Header("Turn Off Target")]
    public GameObject turnOffTarget;

    private bool isPressed = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animationState = AnimationState.Off;
        UpdateAnimation(animationState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed && other.TryGetComponent<IPlayer>(out IPlayer player))
        {
            isPressed = true;

            animationState = AnimationState.On;
            UpdateAnimation(animationState);

            ActivateSwitch();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IPlayer>(out IPlayer player))
        {
            isPressed = false;

            animationState = AnimationState.Off;
            UpdateAnimation(animationState);
        }
    }

    private void ActivateSwitch()
    {
        if (turnOnTarget != null && turnOnTarget == turnOffTarget)
        {
            if (turnOnTarget.TryGetComponent<IOperatable>(out IOperatable operatable))
            {
                operatable.ToggleOperating();
            }
            return;
        }

        if (turnOnTarget != null)
        {
            if (turnOnTarget.TryGetComponent<IOperatable>(out IOperatable operatable))
            {
                operatable.SetOperating(true);
            }
        }

        if (turnOffTarget != null)
        {
            if (turnOffTarget.TryGetComponent<IOperatable>(out IOperatable operatable))
            {
                operatable.SetOperating(false);
            }
        }
    }

    void UpdateAnimation(AnimationState state)
    {
        if (animator == null) return;

        switch (state)
        {
            case AnimationState.On:
                animator.SetBool("isOn", true);
                break;

            case AnimationState.Off:
                animator.SetBool("isOn", false);
                break;

            case AnimationState.None:
                break;
        }
    }
}
