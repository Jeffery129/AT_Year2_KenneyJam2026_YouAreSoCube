using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour, IPlayer
{
    enum AnimationState
    {
        None,
        Idle,
        Walk,
        Die
    }

    [SerializeField] private PlayerController.AnimationState animationState;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Hold Threshold")]
    public float holdThreshold = 0.2f;

    [Header("Game Over Settings")]
    [SerializeField] private GameOverWindowManager gameOverWindowManager;
    [SerializeField] private float delayBeforeGameOver = 1.5f;

    [Header("Death SE")]
    [SerializeField] private AudioClip deathSE;

    public bool hasKey = false;

    private Vector3 _targetPosition;
    private bool _isMoving = false;
    private Camera _mainCamera;

    private Rigidbody _rb;
    private Animator _animator;

    private float _mouseDownTime;
    private bool _isWaitingForRelease;

    private bool _isDead = false;

    private void Start()
    {
        _mainCamera = Camera.main;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (_isDead) return;

        if (Mouse.current == null) return;

        if (!IsGrounded())
        {
            _isWaitingForRelease = false;
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _mouseDownTime = Time.time;
            _isWaitingForRelease = true;
        }

        if (_isWaitingForRelease && Mouse.current.leftButton.isPressed)
        {
            if (Time.time - _mouseDownTime > holdThreshold)
            {
                _isWaitingForRelease = false;
            }
        }

        if (_isWaitingForRelease && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            SetTargetPosition();
            _isWaitingForRelease = false;
        }

        if (_isMoving)
        {
            animationState = AnimationState.Walk;
        }
        else
        {
            animationState = AnimationState.Idle;
        }

        UpdateAnimation(animationState);
    }

    private void FixedUpdate()
    {
        if (!_isDead && _isMoving)
        {
            MoveToTarget();
        }
    }

    private bool IsGrounded()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;

        return Physics.Raycast(rayStart, Vector3.down, 0.3f);
    }

    private void SetTargetPosition()
    {
        var mousePos = Mouse.current.position.ReadValue();
        var ray = _mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out var hit))
        {
            _targetPosition = new Vector3(hit.point.x, _rb.position.y, hit.point.z);
            _isMoving = true;
        }
    }

    private void MoveToTarget()
    {
        _targetPosition.y = _rb.position.y;

        var distance = Vector3.Distance(_rb.position, _targetPosition);

        if (distance < 0.1f)
        {
            _isMoving = false;
            return;
        }

        Vector3 moveDirection = (_targetPosition - _rb.position).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);
            _rb.MoveRotation(lookRotation);
        }

        Vector3 nextPosition = Vector3.MoveTowards(_rb.position, _targetPosition, moveSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(nextPosition);
    }

    private void UpdateAnimation(AnimationState state)
    {
        if (_animator == null) return;

        switch (state)
        {
            case AnimationState.Idle:
                _animator.SetBool("isWalking", false);
                break;

            case AnimationState.Walk:
                _animator.SetBool("isWalking", true);
                break;

            case AnimationState.Die:
                _animator.SetBool("isWalking", false);
                _animator.SetTrigger("isDead");
                break;

            case AnimationState.None:
                break;
        }
    }

    public void TakeDamage()
    {
        if (_isDead)
        {
            return;
        }



        _isDead = true;
        _isMoving = false;
        _isWaitingForRelease = false;

        animationState = AnimationState.Die;
        UpdateAnimation(animationState);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySE(deathSE);
        }


        Debug.Log("dead");

        StartCoroutine(ShowGameOverWithDelay());
    }

    private IEnumerator ShowGameOverWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeGameOver);

        if (gameOverWindowManager != null)
        {
            gameOverWindowManager.ShowGameOverWindow();
        }
    }

    public void SetHasKey(bool set)
    {
        hasKey = set;
    }

    public void StopMovement()
    {
        _isMoving = false;
        _isWaitingForRelease = false;

        animationState = AnimationState.Idle;
        UpdateAnimation(animationState);
    }
}
