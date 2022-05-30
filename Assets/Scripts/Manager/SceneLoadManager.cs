using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void Reload() => SceneManager.LoadScene(this.gameObject.scene.name);

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
}
