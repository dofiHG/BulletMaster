using UnityEngine;
using YG;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Start()
    {
        for (int i = 0; i < YG2.saves.stars.Length - 1; i++)
        {
            YG2.saves.stars[i] = 0;
            YG2.saves.openedLevels[i] = 0;
        }
        YG2.saves.openedLevels[0] = 1;
        YG2.SaveProgress();

        if (FindObjectsOfType<DontDestroyOnLoad>().Length > 1)
            Destroy(gameObject);

        else
            DontDestroyOnLoad(gameObject);
    }
}
