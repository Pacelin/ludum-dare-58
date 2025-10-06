using System;
using System.Threading;
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
        
        private const string BASE_URL = "https://api.thespinningsofa.ru/";
        private const string POST_URL = BASE_URL + "api/post-butterfly";
        private const string GET_URL = BASE_URL + "api/get-butterflies";
        private const int CACHE_DURATION_SECONDS = 10;
        
        private static ButterflyData[] _cachedData;
        private static DateTime _lastUpdate;
        private static readonly AsyncLazy<ButterflyData[]> _currentRequest;
        private static CancellationTokenSource _requestCancellation;

        static ButterfliesServer()
        {
            _requestCancellation = new CancellationTokenSource();
            _currentRequest = new AsyncLazy<ButterflyData[]>(FetchButterfliesData);
        }
        
        public static async UniTask PostButterfly(int id, float size)
        {
            var data = new ButterflyData 
            {
                id = id,
                size = size, 
                username = PlayerPrefs.GetString("username"), 
            };
            
            var json = JsonUtility.ToJson(data);
            Debug.Log($"Отправляемый JSON: {json}");
            
            using var request = CreatePostRequest(json);
            
            try
            {
                await request.SendWebRequest()
                    .ToUniTask(cancellationToken: GetCancellationToken());
                
                if (request.IsError())
                {
                    Debug.LogError($"Ошибка: {request.error}\nОтвет сервера: {request.downloadHandler?.text}");
                    return;
                }
                
                InvalidateCache();
                Debug.Log($"Запрос успешен! Ответ: {request.downloadHandler.text}");
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Запрос отменен");
            }
        }

        public static async UniTask<ButterflyData[]> GetButterflies()
        {
            if (IsCacheValid())
                return _cachedData;

            try
            {
                return await _currentRequest.Task;
            }
            catch (OperationCanceledException)
            {
                return Array.Empty<ButterflyData>();
            }
        }

        private static UnityWebRequest CreatePostRequest(string json)
        {
            var request = new UnityWebRequest(POST_URL, "POST")
            {
                uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        private static async UniTask<ButterflyData[]> FetchButterfliesData()
        {
            using var request = UnityWebRequest.Get(GET_URL);
            
            await request.SendWebRequest().ToUniTask(cancellationToken: GetCancellationToken());
            
            if (!request.IsSuccess())
                return Array.Empty<ButterflyData>();

            var jsonResponse = request.downloadHandler.text;
            var result = JsonUtility.FromJson<TableData>(jsonResponse);
            
            UpdateCache(result.data);
            return result.data;
        }

        private static bool IsCacheValid()
        {
            return _cachedData != null && 
                   (DateTime.Now - _lastUpdate).TotalSeconds < CACHE_DURATION_SECONDS;
        }

        private static void UpdateCache(ButterflyData[] data)
        {
            _cachedData = data;
            _lastUpdate = DateTime.Now;
        }

        private static void InvalidateCache()
        {
            _lastUpdate = DateTime.MinValue;
            _cachedData = null;
        }

        private static bool IsError(this UnityWebRequest request)
        {
            return request.result == UnityWebRequest.Result.ConnectionError || 
                   request.result == UnityWebRequest.Result.ProtocolError;
        }

        private static bool IsSuccess(this UnityWebRequest request)
        {
            return request.result == UnityWebRequest.Result.Success;
        }

        private static CancellationToken GetCancellationToken()
        {
            return ApplicationState.ExitCancellationToken;
        }

        public static void Dispose()
        {
            _requestCancellation?.Cancel();
            _requestCancellation?.Dispose();
            _requestCancellation = null;
        }
    }
}