using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraViewChanger : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cm1stPerson;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cm3rdPerson;

    private TPSCameraController TPSCameraController;
    private FPSCameraController FPSCameraController;

    private float viewCount;

    private void Awake()
    {
        TPSCameraController = GetComponent<TPSCameraController>();
        FPSCameraController = GetComponent<FPSCameraController>();
        viewCount = 0;
    }

    private void LastUpdate()
    {
        ViewChange();
    }

    private void ViewChange()
    {
        if (viewCount % 2 == 0)
        {
            cm1stPerson.Priority -= 10;
            TPSCameraController.enabled = true;
            FPSCameraController.enabled = false;

        }
        else
        {
            cm1stPerson.Priority += 10;
            TPSCameraController.enabled = false;
            FPSCameraController.enabled = true;
        }
    }

    private void OnViewChange(InputValue value)
    {
        ++viewCount;
        ViewChange();
    }
}
