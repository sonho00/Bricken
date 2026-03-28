using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

[System.Serializable]
public class RankData
{
    public string name;
    public int score;

    internal void Deconstruct(out string name, out int score)
    {
        name = this.name;
        score = this.score;
    }
}

[System.Serializable]
public class RankDataList
{
    public List<RankData> ranks;
}

public class NetworkManager : MonoBehaviour
{
    private const string url = "http://127.0.0.1:8000";

    public async void PostScore(string name, int score)
    {
        var request = new UnityWebRequest($"{url}/post-score", "POST");
        var obj = new { name, score };
        string data = JsonUtility.ToJson(obj);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        await request.SendWebRequest();
    }

    public async Awaitable<List<RankData>> RequestLeaderboard()
    {
        var request = new UnityWebRequest($"{url}/get-scores", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string rawData = request.downloadHandler.text;
            string json = $"{{\"ranks\":{rawData}}}";
            RankDataList list = JsonUtility.FromJson<RankDataList>(json);
            return list.ranks;
        }
        else return new List<RankData>();
    }
}
