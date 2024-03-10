using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeartSprite;
    public Sprite takesHitHeartSprite;
    public Sprite emptyHeartSprite;

    public void UpdateHearts(int currentHealth)
    {
        if (currentHealth >= 70)
        {
            SetHeartSprite(0, fullHeartSprite);
            SetHeartSprite(1, fullHeartSprite);
            SetHeartSprite(2, fullHeartSprite);
        }
        else if (currentHealth >= 40)
        {
            SetHeartSprite(0, takesHitHeartSprite);
            SetHeartSprite(1, fullHeartSprite);
            SetHeartSprite(2, fullHeartSprite);
        }
        else if (currentHealth >= 0.1f)
        {
            SetHeartSprite(0, takesHitHeartSprite);
            SetHeartSprite(1, takesHitHeartSprite);
            SetHeartSprite(2, fullHeartSprite);
        }
        else
        {
            SetHeartSprite(0, emptyHeartSprite);
            SetHeartSprite(1, emptyHeartSprite);
            SetHeartSprite(2, emptyHeartSprite);
        }
    }

    void SetHeartSprite(int index, Sprite sprite)
    {
        if (index >= 0 && index < hearts.Length && hearts[index] != null)
        {
            hearts[index].sprite = sprite;
        }
    }
}