using System.Collections;
using UnityEngine;

public class DoorClickController : MonoBehaviour
{
    public Transform pivot;
    public float openAngle = 90f;
    public float speed = 2f;
    private bool isOpen = false;
    private bool isMoving = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private Collider doorCollider;
    void Start()
    {
        doorCollider = GetComponent<Collider>();
        closedRotation = pivot.localRotation;
        openRotation = Quaternion.Euler(pivot.localEulerAngles + new Vector3(0, openAngle, 0));
    }

    public void ToggleDoor()
    {
        if (!isMoving)
            StartCoroutine(AnimateDoor());
    }

    private IEnumerator AnimateDoor()
    {
        isMoving = true;

        if (doorCollider != null) doorCollider.enabled = false;

        Quaternion target = isOpen ? closedRotation : openRotation;
        Quaternion start = pivot.localRotation;

        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime * speed;
            pivot.localRotation = Quaternion.Slerp(start, target, t);
            yield return null;
        }

        pivot.localRotation = target;
        isOpen = !isOpen;

        if (doorCollider != null) doorCollider.enabled = true;

        isMoving = false;
    }
}