using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData
{
    public float playerX, playerY, playerZ;

    public float rotationX, rotationY;

    public bool isDoorOpen = false, isChestOpen, isHeld;

    public bool isInRoom2;
}

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public static GameManager instance = null;
    string saveFilePath;
    GameObject lockPad;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player == null)
        {
            Debug.LogError("Player nu a fost găsit!");
            return;
        }

        SaveData data = new ();

        Vector3 pos = player.transform.position;
        data.playerX = pos.x;
        data.playerY = pos.y;
        data.playerZ = pos.z;

        data.rotationY = player.transform.rotation.eulerAngles.y;
        Camera cam = player.GetComponentInChildren<Camera>();
        if (cam != null)
        {
            data.rotationX = cam.transform.localRotation.eulerAngles.x;
        }
        GameObject room1 = GameObject.Find("Room1");
        bool isRoom1Active = (room1 != null && room1.activeInHierarchy);
        data.isInRoom2 = !isRoom1Active;

        if (!data.isInRoom2)
        {
            DoorController door = FindAnyObjectByType<DoorController>();
            data.isDoorOpen = door.isOpen;

            ChestController chest = FindAnyObjectByType<ChestController>();
            data.isChestOpen = chest.isOpen;


            NoteController note = FindAnyObjectByType<NoteController>();
            data.isHeld = note.isHeld;
        }


        try
        {
            string json = JsonUtility.ToJson(data, true);
            string directory = Path.GetDirectoryName(saveFilePath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(saveFilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Eroare la scrierea fisierului: " + e.Message);
        }
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            StartCoroutine(LoadAndPosition(data));

        }
        else
        {
            Debug.LogError("Nu exista nicio salvare la: " + saveFilePath);
            SceneManager.LoadScene("Room1");
            return;
        }
    }

    IEnumerator LoadAndPosition(SaveData data)
    {
        if(SceneFader.instance != null)
        {
            SceneFader.instance.FadeToBlackOnly();
            yield return new WaitForSeconds(2f / SceneFader.instance.fadeSpeed);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Room1");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        player = GameObject.FindGameObjectWithTag("Player");


        if (player != null)
        {
            if (player.TryGetComponent<CharacterController>(out var controller)) controller.enabled = false;

            player.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);

            player.transform.rotation = Quaternion.Euler(0, data.rotationY, 0);

            CameraMovement cam = player.GetComponentInChildren<CameraMovement>();
            if (cam != null)
            {
                float rotX = data.rotationX;
                if (rotX > 180) rotX -= 360;
                cam.xRotation = rotX;
                cam.transform.localRotation = Quaternion.Euler(rotX, 0, 0);
            }

            Physics.SyncTransforms();

            if (controller != null) controller.enabled = true;
        }

        DoorController door = FindAnyObjectByType<DoorController>();
        door.LoadStatus(data.isDoorOpen);


        RoomsManager roomsManager = FindAnyObjectByType<RoomsManager>(FindObjectsInactive.Include);

        if (roomsManager && data.isInRoom2)
        {
            roomsManager.OnPlayerEnteredRoom2();
            roomsManager.OnDoorOpened();

            if (roomsManager.caracterAnimator)
            {
                roomsManager.caracterAnimator.SetTrigger("StartRising");
            }
            else
            {
                Debug.LogWarning("Obiectul CaracterAI nu este activ sau nu exista in scena!");
            }
        }

        if (!data.isInRoom2)
        {
            ChestController chest = FindAnyObjectByType<ChestController>();
            chest.LoadStatus(data.isChestOpen);

            if (data.isChestOpen)
            {
                lockPad = GameObject.FindGameObjectWithTag("Lock");
                if (lockPad)
                {
                    lockPad.SetActive(false);
                }
            }

            NoteController note = FindAnyObjectByType<NoteController>();
            note.LoadStatus(data.isHeld);
        }
        
        if (SceneFader.instance != null)
            SceneFader.instance.FadeIn();
    }
}
