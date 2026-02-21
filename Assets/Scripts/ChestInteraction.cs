using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public Camera mainCam;
    public Camera chestCam;
    public ChestController chest;
    public GameObject crosshair;

    bool isUsingLock = false;

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

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void ExitLockPad()
    {
        isUsingLock = false;
        chestCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);

        if(crosshair != null ) crosshair.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
