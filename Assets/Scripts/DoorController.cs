using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public float openAngle = -50f;
    public float duration = 2f;
    public bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openedRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        openedRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    public void LoadStatus(bool openStatus)
    {
        isOpen = openStatus;
        transform.localRotation = isOpen ? openedRotation : closedRotation;
    }

    public void OpenDoor()
    {
        if (!isOpen)
            StartCoroutine(OpenSmoothly());
    }

    public void CloseDoor()
    {
        if (isOpen)
            StartCoroutine(CloseSmoothly());
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

    IEnumerator CloseSmoothly()
    {
        isOpen = false;
        Quaternion startRot = transform.localRotation;
        Quaternion endRot = closedRotation;

        float t = 0;
        while(t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
        transform.localRotation = endRot;   
    }
}

