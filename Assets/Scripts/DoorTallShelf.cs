using UnityEngine;

public class DoorClickController : MonoBehaviour
{
    public Transform pivot;
    public float openAngle = 90f;
    public float speed = 2f;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private Collider doorCollider;
    void Start()
    {
        doorCollider = GetComponent<Collider>();
        closedRotation = pivot.localRotation;
        openRotation = Quaternion.Euler(pivot.localEulerAngles + new Vector3(0, openAngle, 0));
    }

    void OnMouseDown()
    {
        isOpen = !isOpen;
        if (doorCollider != null) doorCollider.enabled = false;
        Invoke("EnableCollider", 1.5f);
    }
    void EnableCollider()
    {
        if (doorCollider != null) doorCollider.enabled = true;
    }

    void Update()
    {
        if (isOpen)
            pivot.localRotation = Quaternion.Slerp(pivot.localRotation, openRotation, Time.deltaTime * speed);
        else
            pivot.localRotation = Quaternion.Slerp(pivot.localRotation, closedRotation, Time.deltaTime * speed);
    }
}






