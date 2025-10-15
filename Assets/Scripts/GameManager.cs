using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public string sceneName;
    public float playerX;
    public float playerY;
    public float playerZ;
}

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
            Debug.LogWarning("No player assigned to GameManager!");
            return;
        }

        SaveData data = new SaveData();
        data.sceneName = SceneManager.GetActiveScene().name;
        Vector3 pos = player.transform.position;
        data.playerX = pos.x;
        data.playerY = pos.y;
        data.playerZ = pos.z;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();

        Debug.Log($"Game saved in scene: {data.sceneName}, position: {pos}");
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            string json = PlayerPrefs.GetString("SaveData");
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            SceneManager.LoadScene(data.sceneName);
            StartCoroutine(RestorePosition(data));
        }
        else
        {
            Debug.Log("No save found.");
        }
    }

    private System.Collections.IEnumerator RestorePosition(SaveData data)
    {
        yield return null;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            player.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
    }
}
