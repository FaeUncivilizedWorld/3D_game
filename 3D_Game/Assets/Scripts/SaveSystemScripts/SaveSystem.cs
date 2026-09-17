using System.Text;
using UnityEngine;

public static class SaveSystem
{
    private const string SAVE_KEY = "Player Save Data";

    public static void SavePlayer(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Game Saved!");
    }

    public static PlayerData LoadPlayer()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            return JsonUtility.FromJson <PlayerData>(json);
        } 
        else
        {
            Debug.LogWarning("No Saved Data Found. Returning Default Values!");
            return null;
        }
    }

    public static bool HasSavedData()
    {
        return PlayerPrefs.HasKey(SAVE_KEY);
    }
}
