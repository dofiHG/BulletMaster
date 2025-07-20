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

        if (index == -1)
        {
            dontDestroy.transform.Find("ConfettiParticleSystem").gameObject.SetActive(false);
            dontDestroy.transform.Find("Victory").gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
            
        else if(index == 99)
        {
            dontDestroy.transform.Find("ConfettiParticleSystem").gameObject.SetActive(false);
            dontDestroy.transform.Find("Victory").gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }   

        else
        {
            dontDestroy.transform.Find("ConfettiParticleSystem").gameObject.SetActive(false);
            dontDestroy.transform.Find("Victory").gameObject.SetActive(false);
            SceneManager.LoadScene(index);
        }
    }
}
