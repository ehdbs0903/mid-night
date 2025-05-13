using UnityEngine;

public class StartUiOnOff : MonoBehaviour
{
    [SerializeField]
    private GameObject stageCan;
    [SerializeField]
    private GameObject CollectionCan;
    
    // void Start()
    // {
    //     
    // }
    //
    // void Update()
    // {
    //     
    // }


    public void StageOn()
    {
        stageCan.SetActive(true);
    }

    public void StageOff()
    {
        stageCan.SetActive(false);
    }

    public void CollectionOn()
    {
        CollectionCan.SetActive(true);
    }
    
    public void CollectionOff()
    {
        CollectionCan.SetActive(false);
    }
    
    
    
    
}
