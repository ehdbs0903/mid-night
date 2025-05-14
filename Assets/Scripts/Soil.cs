using UnityEngine;

public class Soil : MonoBehaviour
{
    [Header("Crop Levels")]
    public GameObject level1Crop;
    public GameObject level2Crop;

    public void HarvestCrop()
    {
        if (level1Crop != null) level1Crop.SetActive(false);
        if (level2Crop != null) level2Crop.SetActive(false);
    }
    
    public void GrowCrop()
    {
        if (level1Crop != null && level1Crop.activeSelf)
        {
            level1Crop.SetActive(false);
            if (level2Crop != null)
                level2Crop.SetActive(true);
        }
    }
}