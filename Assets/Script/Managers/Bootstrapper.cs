using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        Debug.Log("[Bootstrap] Initializing core systems...");

        // Spawn persistent managers from Resources folder
        // Assets/Resources/Managers.prefab must exist
        GameObject managers = Object.Instantiate(
            Resources.Load<GameObject>("Managers")
        );
        Object.DontDestroyOnLoad(managers);
    }
}