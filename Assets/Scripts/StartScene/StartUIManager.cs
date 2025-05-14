using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    
    public InputField NicknameInputField;
    public InputField PasswordInputField;
    
    public InputField RegisterNicknameInputField;
    public InputField RegisterPasswordInputField;
    public InputField RegisterPasswordEmailInputField;
    public InputField RegisterNameInputField;
    
    public void OnStartButtonClick()
    {
        startPanel.SetActive(false);
        LoginPanel.SetActive(true);
        
        NicknameInputField.text = "";
        PasswordInputField.text = "";
    }
    
    public void OnSignInButtonClick()
    {
        string nickname = NicknameInputField.text;
        string password = PasswordInputField.text;
        
        StartCoroutine(
            Api_PostLogin.Send(
                nickname,
                password,
                (code, message) => {
                    if (code == "200")
                    {
                        Debug.Log("Login successful: " + message);
                        SceneManager.LoadScene(1);
                    }
                    else
                    {
                        Debug.LogWarning($"Login failed (code={code}): {message}");
                    }

                    NicknameInputField.text = "";
                    PasswordInputField.text = "";
                }
            )
        );
    }
    
    public void OnSignUpButtonClick()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
        
        RegisterNicknameInputField.text = "";
        RegisterPasswordInputField.text = "";
        RegisterNicknameInputField.text = "";
        RegisterPasswordInputField.text = "";
    }
    
    public void OnConfirmButtonClick()
    {
        string nickname = RegisterNicknameInputField.text;
        string password = RegisterPasswordInputField.text;
        string email    = RegisterPasswordEmailInputField.text;
        string name     = RegisterNameInputField.text;

        StartCoroutine(
            Api_PostSignUp.Send(
                nickname, password, email, name,
                code => {
                    if (code == "200")
                    {
                        SceneManager.LoadScene(1);
                    }
                    else
                    {
                        Debug.LogWarning("Registration failed: code=" + code);
                    }

                    RegisterPanel.SetActive(false);
                    LoginPanel.SetActive(true);

                    RegisterNicknameInputField.text      = "";
                    RegisterPasswordInputField.text      = "";
                    RegisterPasswordEmailInputField.text = "";
                    RegisterNameInputField.text          = "";
                }
            )
        );
    }

    
    public void OnCancelButtonClick()
    {
        RegisterPanel.SetActive(false);
        startPanel.SetActive(true);
    }
    
    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
