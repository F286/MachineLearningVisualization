using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
    public string sceneName;

    public void OnTrigger()
    {
        SceneManager.LoadScene(sceneName);
    }
}
