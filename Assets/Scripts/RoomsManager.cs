using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    public GameObject room1;
    public GameObject room2;
    public DoorController doorToRoom2;

    void Start()
    {
        room1.SetActive(true);
        room2.SetActive(false);
    }
    public void OnDoorOpened()
    {
        room2.SetActive(true);
    }

    public void OnPlayerEnteredRoom2()
    {
        room1.SetActive(false);
    }
}