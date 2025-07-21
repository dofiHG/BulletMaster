using YG;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelMenuManager : MonoBehaviour
{
    [SerializeField] private Transform _levelsContent;
    [SerializeField] private Sprite _lockedLevel;
    [SerializeField] private Sprite _unlockedLevel;

    private List<Transform> _levels = new List<Transform>();

    private void Start()
    {
        foreach(Transform level in _levelsContent)
            _levels.Add(level);
    }

    public void ActivateMenu() => PaintStars();

    private void PaintStars()
    {
        int i = 0;
        foreach (Transform level in _levels)
        {
            Transform star1 = level.Find("StarsContainer").Find("Star1");
            Transform star2 = level.Find("StarsContainer").Find("Star2");
            Transform star3 = level.Find("StarsContainer").Find("Star3");

            Color32 goldColor = new Color32(255, 251, 0, 255);

            switch (YG2.saves.stars[i])
            {
                case 3:
                    SetStarColor(star3, goldColor);
                    goto case 2;
                case 2:
                    SetStarColor(star2, goldColor);
                    goto case 1;
                case 1:
                    SetStarColor(star1, goldColor);
                    break;
                case 0:
                    if (YG2.saves.openedLevels[i] == 0)
                    {
                        level.GetComponent<Image>().sprite = _lockedLevel;
                        level.GetComponent<Button>().enabled = false;
                    }
                    else
                    {
                        level.GetComponent<Image>().sprite = _unlockedLevel;
                        level.GetComponent<Button>().enabled = true;
                    }

                    break;
            }
            i++;
        }
    }

    private void SetStarColor(Transform star, Color32 color)
    {
        Image image = star.GetComponent<Image>();

        if (image != null)
            image.color = color;
    }
}
