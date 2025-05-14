using UnityEngine;

public class StartUiOnOff : MonoBehaviour
{
    [SerializeField] private GameObject stageCan;
    [SerializeField] private GameObject CollectionCan;
    [SerializeField] private GameObject mainCan;

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
        mainCan.SetActive(false);
        stageCan.SetActive(false);
        CollectionCan.SetActive(true);
    }
    
    public void CollectionOff()
    {
        CollectionCan.SetActive(false);
        mainCan.SetActive(true);
    }
    
    
    
    
}
