using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSocket : MonoBehaviour
{
    public NoteController heldNote;
    public Transform snapPoint;

    public void TryAttach()
    {
        if (heldNote != null && heldNote.isHeld) AttachNote();
    }

    public void AttachNote()
    {
        heldNote.transform.SetParent(snapPoint);
        heldNote.transform.localPosition = Vector3.zero;
        heldNote.transform.localRotation = Quaternion.identity;
        heldNote.transform.localScale = new Vector3(0.137f, 0.1582279f, 0.1666666f);

        heldNote.isHeld = false;
        if(heldNote.GetComponent<Collider>()) heldNote.GetComponent<Collider>().enabled = true;
    }
}
