using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Scope : MonoBehaviour
{
    [SerializeField] private float farDistance = 12f;
    [SerializeField] private float normalDistance = 14f;
    [SerializeField] private float transitionTime = 0.5f;
    private CameraController cameraController;

    public void AimToTarget(bool aim)
    {
        cameraController = CameraController.Instance;
        float targetDistance = aim ? farDistance : normalDistance;
        LeanTween.value(gameObject, UpdateCameraSize,
        cameraController.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize,
        targetDistance, transitionTime);
    }

    private void UpdateCameraSize(float value)
    {
        cameraController.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = value;
    }
}
