using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("中心（プレイヤー？）")]
    public Transform target;

    [Header("カメラとプレイヤーの距離")]
    public float distance = 10.0f;

    [Header("回転速度")]
    public float rotationSpeed = 0.2f;

    [Header("カメラの高さオフセット")]
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0);

    [Header("回転角度の制限")]
    public float minYAngle = -80f;
    public float maxYAngle = 80f;

    private float rotX = 0f;
    private float rotY = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotX = angles.y;
        rotY = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;
        if (Mouse.current == null) return;

        HandleRotation();
        UpdateCameraPosition();
    }

    void HandleRotation()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            rotX += mouseDelta.x * rotationSpeed;
            rotY -= mouseDelta.y * rotationSpeed;

            rotY = Mathf.Clamp(rotY, minYAngle, maxYAngle);
        }
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0f);
        Vector3 lookAtPosition = target.position + targetOffset;
        Vector3 position = lookAtPosition - (rotation * Vector3.forward * distance);

        transform.rotation = rotation;
        transform.position = position;
    }
}