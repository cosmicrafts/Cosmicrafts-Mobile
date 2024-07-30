using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

public class GooglePlayGamesLogin : MonoBehaviour
{
    private const string serverUrl = "https://dashboard.cosmicrafts.com/api";
    public TextMeshProUGUI debugText;

    void Start()
    {
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string imgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

            Debug.Log("Logged in successfully");
            debugText.text += "Logged in successfully\n";
            Debug.Log("User Name: " + name);
            debugText.text += "User Name: " + name + "\n";
            Debug.Log("User ID: " + id);
            debugText.text += "User ID: " + id + "\n";
            Debug.Log("User Image URL: " + imgUrl);
            debugText.text += "User Image URL: " + imgUrl + "\n";

            StartCoroutine(SendUserIdToServer(id, name, 1)); // Example avatarId
        }
        else
        {
            Debug.Log("Failed to log in: " + status);
            debugText.text += "Failed to log in: " + status + "\n";

            switch (status)
            {
                case SignInStatus.Canceled:
                    debugText.text += "Sign-in canceled by user.\n";
                    break;
                case SignInStatus.InternalError:
                    debugText.text += "Sign-in failed due to internal error.\n";
                    break;
                default:
                    debugText.text += "Unknown sign-in status: " + status + "\n";
                    break;
            }

            // Optionally, show a login button to manually authenticate if needed
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(SignInCallback);
        }
    }

    IEnumerator SendUserIdToServer(string userId, string username, int avatarId)
    {
        var json = JsonUtility.ToJson(new { userId });
        var loginRequest = new UnityWebRequest($"{serverUrl}/login", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        loginRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        loginRequest.downloadHandler = new DownloadHandlerBuffer();
        loginRequest.SetRequestHeader("Content-Type", "application/json");

        yield return loginRequest.SendWebRequest();

        if (loginRequest.result == UnityWebRequest.Result.Success)
        {
            var response = loginRequest.downloadHandler.text;
            Debug.Log("Login Response: " + response);
            debugText.text += "Login Response: " + response + "\n";

            var keys = JsonUtility.FromJson<Keys>(response);

            // Now call the registerPlayer API
            var registerJson = JsonUtility.ToJson(new { username, avatarId, keys.privateKeyBase64 });
            var registerRequest = new UnityWebRequest($"{serverUrl}/registerPlayer", "POST");
            byte[] registerBody = System.Text.Encoding.UTF8.GetBytes(registerJson);
            registerRequest.uploadHandler = new UploadHandlerRaw(registerBody);
            registerRequest.downloadHandler = new DownloadHandlerBuffer();
            registerRequest.SetRequestHeader("Content-Type", "application/json");

            yield return registerRequest.SendWebRequest();

            if (registerRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Player registered successfully: " + registerRequest.downloadHandler.text);
                debugText.text += "Player registered successfully: " + registerRequest.downloadHandler.text + "\n";
            }
            else
            {
                Debug.Log("Error registering player: " + registerRequest.error);
                debugText.text += "Error registering player: " + registerRequest.error + "\n";
            }
        }
        else
        {
            Debug.Log("Error logging in: " + loginRequest.error);
            debugText.text += "Error logging in: " + loginRequest.error + "\n";
        }
    }

    [System.Serializable]
    public class Keys
    {
        public string publicKeyBase64;
        public string privateKeyBase64;
    }
}
