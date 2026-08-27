using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SaveLocal(PlayerData data)
    {
        // NOTE: JsonUtility can't serialize Dictionary<,> - PlayerData.inventory needs a
        // serializable key/value list wrapper before this actually works.
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public PlayerData LoadLocal()
    {
        if (!File.Exists(SavePath))
        {
            return null;
        }

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<PlayerData>(json);
    }
}
