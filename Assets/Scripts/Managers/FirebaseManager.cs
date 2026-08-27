using System;
using UnityEngine;

// TODO: this is a placeholder ahead of adding the actual Firebase Unity SDK
// (com.google.firebase.auth / .database packages) to Packages/manifest.json.
public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    public bool IsSignedIn { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SignIn(string email, string password, Action<bool> onComplete)
    {
        // TODO: Firebase Auth sign-in.
        onComplete?.Invoke(false);
    }

    public void SyncToCloud(PlayerData data, Action<bool> onComplete)
    {
        // TODO: push PlayerData to Firebase Realtime Database, cloud version authoritative.
        onComplete?.Invoke(false);
    }

    public void SyncFromCloud(Action<PlayerData> onComplete)
    {
        // TODO: pull PlayerData from Firebase Realtime Database.
        onComplete?.Invoke(null);
    }
}
