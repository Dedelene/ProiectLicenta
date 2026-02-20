using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public float openAngle = 90f;
    public float duration = 2f;
    public bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openedRotation;
    void Start()
    {
        closedRotation = transform.localRotation;
        openedRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }
    public void OpenChest()
    {
        if (!isOpen)
            StartCoroutine(OpenSmoothly());
    }

    IEnumerator OpenSmoothly()
    {
        isOpen = true;
        Quaternion startRot = transform.localRotation;
        Quaternion endRot = openedRotation;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        transform.localRotation = endRot;
    }
}
