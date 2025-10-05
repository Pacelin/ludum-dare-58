using System;
using Cysharp.Threading.Tasks;
using Scripts.Core.Lifetime;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.Game.Server
{
    public static class ButterfliesServer
    {
        [Serializable]
        private struct TableData
        {
            public ButterflyData[] data;
        }
        
        private const string URL = "https://api.thespinningsofa.ru/";
        private const string POST_URL = URL + "api/post-butterfly";
        private const string GET_URL = URL + "api/get-butterflies";
        private static ButterflyData[] _cached;
        private static DateTime _lastUpdate;
        
        public static async UniTask PostButterfly(int id, float size, int sizeindex)
        {
            var data = new ButterflyData 
            {
                id = id,
                size = size, 
                username = UserDataManager.GetString("username"), 
                sizeindex = sizeindex
            };
            var json = JsonUtility.ToJson(data);
            Debug.Log("Отправляемый JSON: " + json);
            
            using (UnityWebRequest request = UnityWebRequest.PostWwwForm(POST_URL, ""))
            {
                request.method = "POST";
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                await request.SendWebRequest().ToUniTask(cancellationToken: ApplicationState.ExitCancellationToken);

                if (request.result == UnityWebRequest.Result.ConnectionError || 
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Ошибка: " + request.error);
                    Debug.LogError("Ответ сервера: " + request.downloadHandler?.text);
                }
                else
                {
                    Debug.Log("Запрос успешен!");
                    Debug.Log("Ответ: " + request.downloadHandler.text);
                }
            }
        }

        public static async UniTask<ButterflyData[]> GetButterflies()
        {
            if (DateTime.Now - _lastUpdate < TimeSpan.FromSeconds(10))
                return _cached;
            
            using (UnityWebRequest request = UnityWebRequest.Get(GET_URL))
            {
                await request.SendWebRequest()
                    .ToUniTask(cancellationToken: ApplicationState.ExitCancellationToken);
            
                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    var result = JsonUtility.FromJson<TableData>(jsonResponse);
                    foreach (var row in result.data)
                        Debug.Log($"ID: {row.id}, Size: {row.size}, User: {row.username}, SizeIndex: {row.sizeindex}");
                    _cached = result.data;
                    _lastUpdate = DateTime.Now;
                    return result.data;
                }
                else
                {
                    Debug.LogError($"Ошибка: {request.error}");
                    return null;
                }
            }
        }
    }
}