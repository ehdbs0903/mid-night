using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    
    public TMP_InputField NicknameInputField;
    public TMP_InputField PasswordInputField;
    
    public TMP_InputField RegisterNicknameInputField;
    public TMP_InputField RegisterPasswordInputField;
    public TMP_InputField RegisterEmailInputField;

    
    public void OnStartButtonClick()
    {
        LoginPanel.SetActive(true);
        
        NicknameInputField.text = "";
        PasswordInputField.text = "";
    }
    
    public void OnSignInButtonClick()
    {
        SceneManager.LoadScene(1);
        // string nickname = NicknameInputField.text;
        // string password = PasswordInputField.text;
        //
        // StartCoroutine(
        //     Api_PostLogin.Send(
        //         nickname,
        //         password,
        //         (code, message) =>
        //         {
        //             Debug.Log(code);
        //             if (code == "200")
        //             {
        //                 Debug.Log("Login successful: " + message);
        //                 SceneManager.LoadScene(1);
        //             }
        //             else
        //             {
        //                 Debug.LogWarning($"Login failed (code={code}): {message}");
        //             }
        //
        //             NicknameInputField.text = "";
        //             PasswordInputField.text = "";
        //         }
        //     )
        // );
    }
    
    public void OnSignUpButtonClick()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
        
        RegisterNicknameInputField.text = "";
        RegisterPasswordInputField.text = "";
        RegisterEmailInputField.text = "";
    }
    
    public void OnConfirmButtonClick()
    {
        string nickname = RegisterNicknameInputField.text;
        string password = RegisterPasswordInputField.text;
        string email    = RegisterEmailInputField.text;
        
        StartCoroutine(
            Api_PostSignUp.Send(
                nickname, password, password, email, nickname,
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
        
                    RegisterNicknameInputField.text = "";
                    RegisterPasswordInputField.text = "";
                    RegisterEmailInputField.text = "";
                }
            )
        );
    }

    
    public void OnXButtonClickLogin()
    {
        LoginPanel.SetActive(false);
    }
    
    public void OnXButtonClickRegister()
    {
        RegisterPanel.SetActive(false);
    }
    
    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
