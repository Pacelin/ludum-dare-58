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

        private static bool _isProcessing = false;
        
        public static async UniTask PostButterfly(int id, float size)
        {
            var data = new ButterflyData 
            {
                id = id,
                size = size, 
                username = UserDataManager.GetString("username"), 
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
                    _lastUpdate = DateTime.MinValue;
                    Debug.Log("Запрос успешен!");
                    Debug.Log("Ответ: " + request.downloadHandler.text);
                }
            }
        }

        public static async UniTask<ButterflyData[]> GetButterflies()
        {
            if (DateTime.Now - _lastUpdate < TimeSpan.FromSeconds(10))
                return _cached;

            if (_isProcessing)
            {
                await UniTask.WaitWhile(() => _isProcessing,
                    cancellationToken: ApplicationState.ExitCancellationToken);
                if (_cached == null)
                    throw new Exception("Ошибка получения данных с сервера");
                return _cached;
            }
            
            _isProcessing = true;
            using (UnityWebRequest request = UnityWebRequest.Get(GET_URL))
            {
                try
                {
                    await request.SendWebRequest()
                        .ToUniTask(cancellationToken: ApplicationState.ExitCancellationToken);
                }
                catch
                {
                    _isProcessing = false;
                    throw;
                }
            
                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    var result = JsonUtility.FromJson<TableData>(jsonResponse);
                    foreach (var row in result.data)
                        Debug.Log($"ID: {row.id}, Size: {row.size}, User: {row.username}");
                    _cached = result.data;
                    _lastUpdate = DateTime.Now;
                    _isProcessing = false;
                    return result.data;
                }
                
                Debug.LogError($"Ошибка: {request.error}");
                _isProcessing = false;
                return null;
            }
        }
    }
}