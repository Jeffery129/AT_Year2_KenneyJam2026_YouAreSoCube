using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeClick : MonoBehaviour
{
    private enum Directions
    {
        RedX,
        GreenY,
        BlueZ,
        RedXMinus,
        GreenYMinus,
        BlueZMinus,
    }

    [SerializeField] private List<Directions> changeDirections = new List<Directions>();

    [Header("Scale Change")]
    [SerializeField] private float scaleSensitivity = 0.01f;
    [SerializeField] private float scaleRatio = 0.5f;
    [SerializeField] private LayerMask rayLayerMask = ~0;

    [Header("Player Push")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float playerCheckThickness = 0.2f;
    [SerializeField] private float playerCheckMargin = 0.05f;

    [Header("Change Count")]
    [SerializeField] private float changeCountThreshold = 0.01f;

    private Camera _mainCamera;
    private BoxCollider _boxCollider;

    private Vector3 _originScale;
    private Vector3 _targetPosition;
    private Vector3 _targetScale;
    private Vector3 _scaleChangeDirection;

    private Vector2 _startMousePosVector2;
    private Vector2 _nowMousePosVector2;

    private float _scaleChangeValue;
    private float _lastFaceMoveValue;

    private bool _isScaleChanging;

    private void Start()
    {
        _mainCamera = Camera.main;
        _boxCollider = GetComponent<BoxCollider>();

        _targetPosition = transform.position;
        _targetScale = transform.localScale;
        _scaleChangeDirection = Vector3.right;

        // オリジンのスケールを記録する
        _originScale = transform.localScale;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartScaleChange();
        }

        if (Mouse.current.leftButton.isPressed && _isScaleChanging)
        {
            UpdateScaleChange();
        }

        if (_isScaleChanging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isScaleChanging = false;

            if (HasScaleChanged())
            {
                InGameUIManager.Instance?.UseScaleChangeChance();
            }
        }
    }

    private void StartScaleChange()
    {
        var mousePos = Mouse.current.position.ReadValue();
        var ray = _mainCamera.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, rayLayerMask))
        {
            _isScaleChanging = false;
            return;
        }

        if (hit.collider.gameObject != gameObject)
        {
            _isScaleChanging = false;
            return;
        }

        // Hitの法線のdirection
        _scaleChangeDirection = hit.normal.normalized;

        // この方向にスケールを変えるのが許されるのかをチェックする、じゃないとreturnする
        if (!CanChangeDirection(_scaleChangeDirection))
        {
            _isScaleChanging = false;
            return;
        }

        if (InGameUIManager.Instance != null && !InGameUIManager.Instance.CanScaleChange())
        {
            _isScaleChanging = false;
            return;
        }

        // データのリセット
        _startMousePosVector2 = mousePos;
        _targetPosition = transform.position;
        _targetScale = transform.localScale;
        _lastFaceMoveValue = 0f;

        _isScaleChanging = true;
    }

    private void UpdateScaleChange() // 長押し中スケール変化の更新
    {
        _nowMousePosVector2 = Mouse.current.position.ReadValue();

        var changeMousePosVector2 = _nowMousePosVector2 - _startMousePosVector2;
        var screenScaleChangeDirection = GetScreenScaleChangeDirection();

        var localScaleChangeDirection = transform.InverseTransformDirection(_scaleChangeDirection);
        var localAxisDirection = GetLocalAxisDirection(localScaleChangeDirection);

        localScaleChangeDirection = new Vector3(
            Mathf.Abs(localAxisDirection.x),
            Mathf.Abs(localAxisDirection.y),
            Mathf.Abs(localAxisDirection.z)
        );

        _scaleChangeValue = Vector2.Dot(changeMousePosVector2, screenScaleChangeDirection) * scaleSensitivity;

        var minScaleChangeValue = GetMinScaleChangeValue(localAxisDirection);
        _scaleChangeValue = Mathf.Max(minScaleChangeValue, _scaleChangeValue);

        var currentFaceMoveValue = _scaleChangeValue * (scaleRatio + 0.5f);
        var pushValue = Mathf.Max(0f, currentFaceMoveValue - _lastFaceMoveValue);

        transform.localScale = _targetScale + localScaleChangeDirection * _scaleChangeValue;
        transform.position = _targetPosition + _scaleChangeDirection * (_scaleChangeValue * scaleRatio);

        PushPlayer(pushValue, localAxisDirection);

        _lastFaceMoveValue = currentFaceMoveValue;
    }

    private bool HasScaleChanged()
    {
        return Vector3.Distance(transform.localScale, _targetScale) > changeCountThreshold;
    }

    private Vector2 GetScreenScaleChangeDirection() // 画面内にマウスの移動に応じて、スケール変化の向きを決めていく
    {
        var startScreenPosition = _mainCamera.WorldToScreenPoint(_targetPosition);
        var endScreenPosition = _mainCamera.WorldToScreenPoint(_targetPosition + _scaleChangeDirection);

        var screenDirection = (Vector2)(endScreenPosition - startScreenPosition);

        if (screenDirection.sqrMagnitude <= 0.0001f)
        {
            return Vector2.right;
        }

        return screenDirection.normalized;
    }

    private static Vector3 GetLocalAxisDirection(Vector3 direction)
    {
        direction.Normalize();

        var absX = Mathf.Abs(direction.x);
        var absY = Mathf.Abs(direction.y);
        var absZ = Mathf.Abs(direction.z);

        if (absX >= absY && absX >= absZ)
        {
            return new Vector3(Mathf.Sign(direction.x), 0f, 0f);
        }

        if (absY >= absX && absY >= absZ)
        {
            return new Vector3(0f, Mathf.Sign(direction.y), 0f);
        }

        return new Vector3(0f, 0f, Mathf.Sign(direction.z));
    }

    private float GetMinScaleChangeValue(Vector3 localAxisDirection) // オリジンのスケールより小さくしてはだめのようにする
    {
        if (Mathf.Abs(localAxisDirection.x) > 0.5f)
        {
            return _originScale.x - _targetScale.x;
        }

        if (Mathf.Abs(localAxisDirection.y) > 0.5f)
        {
            return _originScale.y - _targetScale.y;
        }

        return _originScale.z - _targetScale.z;
    }

    private void PushPlayer(float pushValue, Vector3 localAxisDirection)
    {
        if (pushValue <= 0f) return;
        if (_boxCollider == null) return;

        var checkCenter = GetPlayerCheckCenter(localAxisDirection);
        var checkHalfExtents = GetPlayerCheckHalfExtents(localAxisDirection, pushValue);

        var colliders = Physics.OverlapBox(
            checkCenter,
            checkHalfExtents,
            transform.rotation,
            playerLayerMask,
            QueryTriggerInteraction.Ignore
        );

        foreach (var hitCollider in colliders)
        {
            var hit = hitCollider.attachedRigidbody;

            if (hit == null) continue;

            hit.MovePosition(hit.position + _scaleChangeDirection * pushValue);
        }
    }

    private Vector3 GetPlayerCheckCenter(Vector3 localAxisDirection)
    {
        var boxHalfSize = _boxCollider.size * 0.5f;

        var faceLocalPosition = _boxCollider.center + new Vector3(
            boxHalfSize.x * localAxisDirection.x,
            boxHalfSize.y * localAxisDirection.y,
            boxHalfSize.z * localAxisDirection.z
        );

        return transform.TransformPoint(faceLocalPosition);
    }

    private Vector3 GetPlayerCheckHalfExtents(Vector3 localAxisDirection, float pushValue)
    {
        var halfExtents = Vector3.Scale(_boxCollider.size, transform.lossyScale) * 0.5f;

        halfExtents.x += playerCheckMargin;
        halfExtents.y += playerCheckMargin;
        halfExtents.z += playerCheckMargin;

        var checkThickness = playerCheckThickness + pushValue;

        if (Mathf.Abs(localAxisDirection.x) > 0.5f)
        {
            halfExtents.x = checkThickness;
        }
        else if (Mathf.Abs(localAxisDirection.y) > 0.5f)
        {
            halfExtents.y = checkThickness;
        }
        else if (Mathf.Abs(localAxisDirection.z) > 0.5f)
        {
            halfExtents.z = checkThickness;
        }

        return halfExtents;
    }

    // ---------------------------------------------------
    // クリックしたら、この方向にスケールを変えるのが許されるのかをチェックする
    private bool CanChangeDirection(Vector3 scaleChangeDirection)
    {
        var localScaleChangeDirection = transform.InverseTransformDirection(scaleChangeDirection);
        var localAxisDirection = GetLocalAxisDirection(localScaleChangeDirection);
        var direction = GetDirectionEnumDataFromDirection(localAxisDirection);

        return changeDirections.Contains(direction); // リストに含まれてるのかチェックする
    }

    // Directionのenum listから参照する
    private static Directions GetDirectionEnumDataFromDirection(Vector3 localAxisDirection)
    {
        return localAxisDirection.x switch
        {
            > 0.5f => Directions.RedX,
            < -0.5f => Directions.RedXMinus,
            _ => localAxisDirection.y switch
            {
                > 0.5f => Directions.GreenY,
                < -0.5f => Directions.GreenYMinus,
                _ => localAxisDirection.z > 0.5f ? Directions.BlueZ : Directions.BlueZMinus
            }
        };
    }
}