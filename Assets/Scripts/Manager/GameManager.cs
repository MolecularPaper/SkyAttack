using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
