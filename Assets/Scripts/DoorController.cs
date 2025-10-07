using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public float openAngle = -55f;
    public float duration = 2f;
    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openedRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openedRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    public void OpenDoor()
    {
        if (!isOpen)
            StartCoroutine(OpenSmoothly());
    }

    IEnumerator OpenSmoothly()
    {
        isOpen = true;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = openedRotation;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        transform.rotation = endRot;
    }
}

