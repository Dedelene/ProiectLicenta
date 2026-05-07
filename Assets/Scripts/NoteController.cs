using UnityEngine;

public class NoteController : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new (0.5f, -0.2f, 0.1f);
    public bool isHeld = false;
    Vector3 originalScale;
    private NoteSocket noteSocket;
    public bool isAttached = false;

    void Start()
    {
        originalScale = transform.localScale;
        noteSocket = FindAnyObjectByType<NoteSocket>();
    }

    public void LoadStatus(bool status, bool attached)
    {
        isHeld = status;
        isAttached = attached;

        if (isHeld)
        {
            Take();
        }
        else if(isAttached)
        {
            noteSocket.AttachNote();
        }
    }
    public void Take()
    {
        isHeld = true;
        transform.SetParent(player.transform);
        transform.localPosition = offset;
        transform.localRotation = Quaternion.Euler(0, 100f, 0);

        transform.localScale = originalScale;

        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;
    }

    public Vector3 GetOriginalScale() => originalScale;
}
