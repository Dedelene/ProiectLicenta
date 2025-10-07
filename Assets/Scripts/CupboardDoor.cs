using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupboardDoor : MonoBehaviour
{
    bool isOpen = false;
    Quaternion closedRotation;
    Quaternion openRotation;

    Collider doorCollider;

    // Start is called before the first frame update
    void Start()
    {
        doorCollider = GetComponent<Collider>();
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -90, 0));
    }

    private void OnMouseDown()
    {
        isOpen = !isOpen;
        if (doorCollider != null) doorCollider.enabled = false;
        Invoke("EnableCollider", 1.5f);
    }

    void EnableCollider()
    {
        if (doorCollider != null) doorCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * 2);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRotation, Time.deltaTime * 2);
    }
}
