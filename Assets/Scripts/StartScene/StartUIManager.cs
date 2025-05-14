using UnityEngine;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    
    public InputField IDInputField;
    public InputField PasswordInputField;
    
    public InputField RegisterIDInputField;
    public InputField RegisterPasswordInputField;
    public InputField RegisterPasswordEmailInputField;
    public InputField RegisterNameInputField;
    
    public void OnStartButtonClick()
    {
        startPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }
    
    public void OnSignInButtonClick()
    {
        // 로그인 api 호출
        Debug.Log("11");
        
        // 성공하면 씬 넘기기
        
        // 실패하면 에러 메시지 띄우기
        
        // 인풋 필드 초기화
        IDInputField.text = "";
        PasswordInputField.text = "";
    }
    
    public void OnSignUpButtonClick()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }
    
    public void OnConfirmButtonClick()
    {
        // 회원가입 api 호출
        Debug.Log("22");
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(true);
        
        // 인풋 필드 초기화
        RegisterIDInputField.text = "";
        RegisterPasswordInputField.text = "";
        RegisterPasswordEmailInputField.text = "";
        RegisterNameInputField.text = "";
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
