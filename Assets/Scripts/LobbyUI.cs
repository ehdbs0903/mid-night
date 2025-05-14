using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    public void SelectStage(int index)
    {
        PlayerPrefs.SetInt("SelectedStage", index);
        SceneManager.LoadScene("Stage");
    }
}
