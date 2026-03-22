using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[System.Serializable]
public class RankData
{
    public int rank;
    public string name;
    public int score;

    public void Deconstruct(out int rank, out string name, out int score) =>
        (rank, name, score) = (this.rank, this.name, this.score);
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
        var request = new UnityWebRequest($"{url}/save-score", "POST");
        var data = $"{{\"name\":\"{name}\",\"score\":{score}}}";

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        await request.SendWebRequest();
    }

    public async Awaitable<List<RankData>> RequestLeaderboard()
    {
        var request = new UnityWebRequest($"{url}/leaderboard", "GET");
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
