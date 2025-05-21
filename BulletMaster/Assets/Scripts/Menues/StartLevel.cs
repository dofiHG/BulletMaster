using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    public void LoadLevel(int index)
    {
        LevelSettings.instance.UnsubscribeEvents();

        if (index == 0) 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
