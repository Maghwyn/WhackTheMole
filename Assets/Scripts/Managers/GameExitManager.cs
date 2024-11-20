using UnityEngine;

public class GameExitManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private TeleportEvent _teleportEvent;
    
    private void Awake()
    {
        _teleportEvent.OnAnchorEnter += GameExit;
    }

    private void GameExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
