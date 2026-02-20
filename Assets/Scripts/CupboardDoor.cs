using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupboardDoor : MonoBehaviour
{
    bool isOpen = false;
    bool isMoving = false;
    readonly float speed = 2f;

    Quaternion closedRotation;
    Quaternion openRotation;

    Collider doorCollider;

    // Start is called before the first frame update
    void Start()
    {
        doorCollider = GetComponent<Collider>();
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, -90f);
    }

    public void ToggleDoor()
    {
        if (!isMoving)
        {
            StartCoroutine(AnimateDoor());
        }
    }
    IEnumerator AnimateDoor()
    {
        isMoving = true;

        if (doorCollider != null) doorCollider.enabled = false;

        Quaternion target = isOpen ? closedRotation : openRotation;
        Quaternion start = transform.localRotation;

        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.localRotation = Quaternion.Slerp(start, target, t);
            yield return null;
        }

        transform.localRotation = target;

        isOpen = !isOpen;
        if (doorCollider != null) doorCollider.enabled = true;
        isMoving = false;
    }
}
