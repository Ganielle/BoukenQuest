using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private UserData userData;

    [Header("References")]
    [SerializeField] private GamePlayerController playerControls;
    [SerializeField] private GameObject target;

    [Header("Parameters")]
    [SerializeField] private float maxCameraSensitivity;
    [SerializeField] private float topClamp;
    [SerializeField] private float bottomClamp;
    [SerializeField] private float cameraAngleOverride;

    [Header("DEBUGGER")]
    [SerializeField] private float _threshold;
    [SerializeField] private float _cinemachineTargetYaw;
    [SerializeField] private float _cinemachineTargetPitch;
    [SerializeField] private float currentCameraSensitivity;

    private void Start()
    {
        _threshold = 0.10f;
        currentCameraSensitivity = maxCameraSensitivity * userData.CameraSensitivity;
    }

    private void LateUpdate()
    {
        HandleCameraInput();
    }

    private void HandleCameraInput()
    {
        if (playerControls.LookDirection.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += playerControls.LookDirection.x * currentCameraSensitivity;
            _cinemachineTargetPitch += -playerControls.LookDirection.y * currentCameraSensitivity;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

        Vector3 Direction = new Vector3(
            Mathf.Cos(_cinemachineTargetPitch * Mathf.Deg2Rad) * Mathf.Sin(_cinemachineTargetYaw * Mathf.Deg2Rad),
            Mathf.Sin(-_cinemachineTargetPitch * Mathf.Deg2Rad),
            Mathf.Cos(_cinemachineTargetPitch * Mathf.Deg2Rad) * Mathf.Cos(_cinemachineTargetYaw * Mathf.Deg2Rad)
        );

        target.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
