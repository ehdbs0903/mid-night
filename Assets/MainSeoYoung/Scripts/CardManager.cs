using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private CardData[] cards;

    public static List<Api_GetStagesInfo.CropRankInfo> cropRankInfoList = new();

    [SerializeField] private int lockAlpha = 0;
    [SerializeField] private int unlockedAlpha = 1;

    void Start()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].isUnlocked)
                continue;


            cards[i].canvasGroup.alpha = lockAlpha;
            // Debug.Log($"{cards[i].name} 알파값 조정");


        }
    }


    public void UnlockCard(int type, CardGrade grade)
    {
        int index = (int)type + (int)grade;
        
        if (cards[index].isUnlocked)
            return;
        
        // Debug.Log($"Unlocked card{index}");
        
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
        if (cards[index].isUnlocked)
            return;
        
        cards[index].isUnlocked = true;
        cards[index].canvasGroup.alpha = unlockedAlpha;
    }

    public void CollectionOpen()
    {
        for (int i = 0; i < cropRankInfoList.Count; i++)
        {
            int type = (int)cropRankInfoList[i].type;
            int rank = (int)cropRankInfoList[i].rank;
            
            UnlockCard(type,(CardGrade) rank);
        }
    }
}
