using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class StartLevel : MonoBehaviour
{
    public void LoadLevel(int index)
    {
        YG2.InterstitialAdvShow();
        LevelSettings.instance.UnsubscribeEvents();

        if (index == 0) 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
