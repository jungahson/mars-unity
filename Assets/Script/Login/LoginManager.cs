using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text resultText;

    private const string loginUrl = "http://localhost:8000/login"; // 여기에 백엔드 URL 입력

    public void OnLoginButtonPressed()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        StartCoroutine(SendLoginRequest(username, password));
    }

    IEnumerator SendLoginRequest(string username, string password)
    {
        // JSON 형태의 로그인 데이터 구성
        string jsonData = JsonUtility.ToJson(new LoginData(username, password));
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // UnityWebRequest 생성
        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 보내기
        yield return request.SendWebRequest();

        // 응답 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("로그인 성공: " + request.downloadHandler.text);
            resultText.text = "로그인 성공!";
        }
        else
        {
            Debug.Log("로그인 실패: " + request.error);
            resultText.text = "로그인 실패: " + request.error;
        }
    }

    // 로그인 정보 구조체
    [System.Serializable]
    public class LoginData
    {
        public string username;
        public string password;

        public LoginData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}