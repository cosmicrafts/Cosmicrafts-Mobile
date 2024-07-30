using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class InternalTestScript : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TextMeshProUGUI debugText;
    private const string serverUrl = "https://dashboard.cosmicrafts.com/api";

    public void RegisterPlayer()
    {
        string username = usernameInputField.text;
        if (!string.IsNullOrEmpty(username))
        {
            StartCoroutine(RegisterPlayerCoroutine(username, 1)); // Hardcoded avatarId to 1
        }
        else
        {
            Debug.Log("Username cannot be empty.");
            debugText.text = "Username cannot be empty.";
        }
    }

    IEnumerator RegisterPlayerCoroutine(string username, int avatarId)
    {
        string userId = System.Guid.NewGuid().ToString(); // Generate a unique userId for testing
        Debug.Log("Generated userId: " + userId);
        debugText.text = "Generated userId: " + userId + "\n";

        // Correctly create the JSON object for the request body
        var registerJson = new RegisterPlayerRequest { userId = userId, username = username, avatarId = avatarId };
        string json = JsonUtility.ToJson(registerJson);
        Debug.Log("Register JSON: " + json);
        debugText.text += "Register JSON: " + json + "\n";

        var registerRequest = new UnityWebRequest($"{serverUrl}/registerPlayer", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        registerRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
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

    public void GetAllPlayers()
    {
        StartCoroutine(GetAllPlayersCoroutine());
    }

    IEnumerator GetAllPlayersCoroutine()
    {
        var getRequest = new UnityWebRequest($"{serverUrl}/getAllPlayers", "GET");
        getRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return getRequest.SendWebRequest();

        if (getRequest.result == UnityWebRequest.Result.Success)
        {
            var response = getRequest.downloadHandler.text;
            Debug.Log("Get All Players Response: " + response);
            debugText.text = "Get All Players Response: " + response + "\n";
        }
        else
        {
            Debug.Log("Error fetching players: " + getRequest.error);
            debugText.text = "Error fetching players: " + getRequest.error + "\n";
        }
    }

    [System.Serializable]
    public class RegisterPlayerRequest
    {
        public string userId;
        public string username;
        public int avatarId;
    }
}
