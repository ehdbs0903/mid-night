using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject StagePanel;
    public GameObject RewardPanel;

    // test
    
    // Edit → Project Settings → Player → Other Settings 로 이동
    //
    // Active Input Handling
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StageEnd();
        }
    }

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
