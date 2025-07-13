using YG;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

public class LevelMenuManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _levels = new List<Transform>();
    [SerializeField] Transform _levelsContent;

    private void Start()
    {
        for (int i = 1; i != 999; i++)
        {
            string levelName = $"Level{i}";
            GameObject level = Resources.FindObjectsOfTypeAll<GameObject>()
                .FirstOrDefault(go => go.name == levelName);

            if (level != null)
            {
                Transform container = level.transform
                    .GetComponentsInChildren<Transform>(true)
                    .FirstOrDefault(t => t.name == "StarsContainer");

                _levels.Add(container);
            }

            else 
                break;
        }
    }

    public void ActivateMenu() => PaintStars();

    private void PaintStars()
    {
        int i = 0;
        foreach (Transform level in _levels)
        {
            Transform star1 = level.Find("Star1");
            Transform star2 = level.Find("Star2");
            Transform star3 = level.Find("Star3");

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
