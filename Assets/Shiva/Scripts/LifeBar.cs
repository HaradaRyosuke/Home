using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public RectTransform bar;
    public float max = 100;

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = bar.GetComponent<Image>();

    }

    public void SetLife(float life)
    {
        Vector2 size = bar.sizeDelta;
        size.x = Mathf.Min(1, life / max) * 5;
        bar.sizeDelta = size;

        if (!image)
            return;

        if (life < max / 5)
        {
            image.color = Color.red;
        }
        else if (life < max / 2)
        {
            image.color = Color.yellow;
        }
        else
        {
            image.color = Color.green;
        }
    }
}
