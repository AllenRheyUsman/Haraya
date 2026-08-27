using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private List<GameObject> screens = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowScreen(string screenName)
    {
        // TODO: swap active screen with a transition (Spline-exported animation).
        foreach (var screen in screens)
        {
            screen.SetActive(screen.name == screenName);
        }
    }

    public void HideAll()
    {
        foreach (var screen in screens)
        {
            screen.SetActive(false);
        }
    }
}
