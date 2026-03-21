using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using UnityEngine.InputSystem;

[System.Serializable]
public class ScorePacket
{
    public string name;
    public int score;
}

[System.Serializable]
public class SaveResponse
{
    public string status;
    public string message;
}

public class NetworkManager : MonoBehaviour
{
    private string url = "http://127.0.0.1:8000/save-score";

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(PostScore("Laborat", Random.Range(100, 1000)));
        }
    }

    IEnumerator PostScore(string playerName, int score)
    {
        ScorePacket packet = new ScorePacket { name = playerName, score = score };
        string json = JsonUtility.ToJson(packet);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                SaveResponse res = JsonUtility.FromJson<SaveResponse>(request.downloadHandler.text);

                Debug.Log("<color=green>서버 응답 성공:</color> " + res.message);
            }
            else
            {
                Debug.Log("<color=red>서버 응답 실패:</color> " + request.error);
            }
        }
    }

    IEnumerator GetScores()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8000/scores"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("<color=green>서버 응답 성공:</color> " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("<color=red>서버 응답 실패:</color> " + request.error);
            }
        }
    }
}
