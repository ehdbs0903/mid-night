using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private CardData[] cards;

    [SerializeField]
    private Color lockColor = new Color(1, 1, 1, 0);
    [SerializeField]
    private Color unlockedColor = new Color(1, 1, 1, 1);

    void Start()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].isUnlocked)
                continue;


            cards[i].image.color = lockColor;
            // Debug.Log($"{cards[i].name} 알파값 조정");


        }
    }


    public void UnlockCard(CardType type, CardGrade grade)
    {
        int index = (int)type + (int)grade;

        if (cards[index].isUnlocked)
            return;

        Debug.Log($"Unlocked card{index}");
        
        if (grade == CardGrade.Common)
        {
            CardSpriteOn(index);
        }
        else if (grade == CardGrade.Rare)
        { 
            CardSpriteOn(index);
            CardSpriteOn(index - 1);
        }
        else if (grade == CardGrade.Epic)
        {
            CardSpriteOn(index);
            CardSpriteOn(index - 1);
            CardSpriteOn(index - 2);
        }
    }

    private void CardSpriteOn(int index)
    {
        cards[index].isUnlocked = true;
        cards[index].image.color = unlockedColor;
    }
}
