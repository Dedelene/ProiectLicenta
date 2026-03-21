using System.Collections;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public RoomsManager roomManager;
    public DoorController door;

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (door.isOpen) door.CloseDoor();

            yield return new WaitForSeconds(2f);

            roomManager.OnPlayerEnteredRoom2();

            gameObject.SetActive(false);
        }
    }
}