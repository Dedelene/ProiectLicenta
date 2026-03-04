using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public Camera mainCam;
    public Camera chestCam;
    public ChestController chest;
    public GameObject crosshair;

    public bool isUsingLock = false;
    LockWheelManager lockWheelManager;

    private void Start()
    {
        lockWheelManager = GetComponent<LockWheelManager>();
    }

    void Update()
    {
        if (!isUsingLock) return;

        if (Input.GetMouseButtonDown(1))
            ExitLockPad();
    }

    public void EnterLockPad()
    {
        isUsingLock = true;

        chestCam.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false);

        if (crosshair != null) crosshair.SetActive(false);

        if (lockWheelManager != null)
            lockWheelManager.UpdateVisual();
    }

    public void ExitLockPad()
    {
        isUsingLock = false;
        chestCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);

        if(crosshair != null ) crosshair.SetActive(true);
    }
}
