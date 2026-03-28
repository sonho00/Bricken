using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using Cysharp.Threading.Tasks;

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

    public async UniTask PostScore(string name, int score)
    {
        var request = new UnityWebRequest($"{url}/post-score", "POST");
        RankData obj = new RankData { name = name, score = score };
        string data = JsonUtility.ToJson(obj);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        await request.SendWebRequest();
    }

    public async UniTask<List<RankData>> RequestLeaderboard()
    {
        var request = new UnityWebRequest($"{url}/get-scores", "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            RankDataList rankDataList = JsonUtility.FromJson<RankDataList>(json);
            return rankDataList.ranks;
        }
        else return new List<RankData>();
    }
}
