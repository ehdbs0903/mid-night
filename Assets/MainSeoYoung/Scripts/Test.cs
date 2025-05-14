using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private CardManager cardManager;
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha1))
    //     { 
    //         cardManager.UnlockCard((int)CardType.Carrot, CardGrade.Common);
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.Alpha2))
    //     {
    //         cardManager.UnlockCard((int)CardType.Carrot, CardGrade.Epic);
    //     }
    // }

    void Start()
    {
        cardManager.UnlockCard((int)CardType.Carrot, CardGrade.Epic);
    }
}
