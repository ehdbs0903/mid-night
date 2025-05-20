using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject StagePanel;
    public GameObject RewardPanel;

    public void StageEnd()
    {
        StagePanel.SetActive(false);
        RewardPanel.SetActive(true);
    }
    
    public void OnRestartButtonClick()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }
    
    public void OnHomeButtonClick()
    {
        SceneManager.LoadScene(1);
    }
}
