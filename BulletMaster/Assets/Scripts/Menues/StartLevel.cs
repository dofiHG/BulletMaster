using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class StartLevel : MonoBehaviour
{
    public void LoadLevel(int index)
    {
        YG2.InterstitialAdvShow();
        LevelSettings.instance.UnsubscribeEvents();

        GameObject dontDestroy = GameObject.Find("DontDestroyOnLoad");

        if (index == 0)
        {
            dontDestroy.transform.Find("ConfettiParticleSystem").gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
            
        else
        {
            dontDestroy.transform.Find("ConfettiParticleSystem").gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }   
    }
}
