using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    public void LoadLevel(int index)
    {
        LevelSettings.instance.UnsubscribeEvents();

        if (index == 0) 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene(index);
    }
}
