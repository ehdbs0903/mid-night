using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private CardManager cardManager;
    void Update()
    {
        Debug.Log(Input.GetKeyDown(KeyCode.A));
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("보통의 감자");
            cardManager.UnlockCard(CardType.Potato, CardGrade.Common);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("최고의 감자");
            cardManager.UnlockCard(CardType.Potato, CardGrade.Epic);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("보통의 옥수수");
            cardManager.UnlockCard(CardType.Corn, CardGrade.Common);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("좋은의 감자");
            cardManager.UnlockCard(CardType.Potato, CardGrade.Rare);
        }
    }
}
