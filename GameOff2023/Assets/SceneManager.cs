using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public string SceneName;

    public void changeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }
}
