using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class StartLevel : MonoBehaviour
{
    public void LoadLevel(int index)
    {
        YG2.InterstitialAdvShow();
        LevelSettings.instance.UnsubscribeEvents();

        GameObject dontDestroy = GameObject.Find("DontDestroy");

        dontDestroy.transform.Find("ConfettiParticleSystem").gameObject.SetActive(false);
        dontDestroy.transform.Find("Victory").gameObject.SetActive(false);
        dontDestroy.transform.Find("LoseSound").gameObject.SetActive(false);

        if (index == -1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        else if(index == 99)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        else
            SceneManager.LoadScene(index);
    }
}
