using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUiOnOff : MonoBehaviour
{
    [SerializeField] private GameObject stageCan;
    [SerializeField] private GameObject CollectionCan;
    [SerializeField] private GameObject mainCan;
    [SerializeField] private CardManager cardManager;

    void Start()
    {
        mainCan.SetActive(true);
        stageCan.SetActive(false);
        CollectionCan.SetActive(false);
    }


    public void StageOn()
    {
        mainCan.SetActive(false);
        CollectionCan.SetActive(false);
        stageCan.SetActive(true);
    }

    public void StageOff()
    {
        stageCan.SetActive(false);
        mainCan.SetActive(true);
    }

    public void CollectionOn()
    {
        // Api_GetStagesInfo.Send(12914298);
        //
        // cardManager.CollectionOpen();
        
        mainCan.SetActive(false);
        stageCan.SetActive(false);
        CollectionCan.SetActive(true);
    }
    
    public void CollectionOff()
    {
        CollectionCan.SetActive(false);
        mainCan.SetActive(true);
    }

    public void OnStageClick()
    {
        SceneManager.LoadScene(2);
    }
}
