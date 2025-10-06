using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using Scripts.Game.Currency;
using Scripts.Game.Server;
using UnityEngine;

namespace Scripts.Game.Butterflies.ButterfliesJournal
{
    public class ButterfliesJournal
    {
        private readonly ButterfliesConfig _generalConfig;
        private readonly Wallet _wallet;
        private readonly Dictionary<int, float> _butterfliesSizesMap;
        private readonly Subject<ButterfliesJournalEntry> _entryAddSubject;
        
        public ButterfliesJournal(ButterfliesConfig generalConfig, Wallet wallet)
        {
            _generalConfig = generalConfig;
            _wallet = wallet;
            _butterfliesSizesMap = new Dictionary<int, float>();
            _entryAddSubject = new Subject<ButterfliesJournalEntry>();
            Load();
        }
        
        public bool HasButterfly(int id) => _butterfliesSizesMap.ContainsKey(id);

        public void GetWorldRecord(int id, Action<float, int, string> callback)
        {
            UniTask.Void(async () =>
            {
                try
                {
                    var worldRecords = await ButterfliesServer.GetButterflies();
                    if (worldRecords.Any(w => w.id == id))
                    {
                        var worldRecord = worldRecords.First(w => w.id == id);
                        var cost = ButterfliesUtils.CalculateCost(_generalConfig,
                            _generalConfig.GetButterflyById(id).Config, worldRecord.size);
                        callback.Invoke(worldRecord.size, cost, worldRecord.username);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            });
        }

        public (float Size, int Cost) GetLocalRecord(int butterflyId)
        {
            var size = _butterfliesSizesMap[butterflyId];
            var cost = ButterfliesUtils.CalculateCost(_generalConfig,
                _generalConfig.GetButterflyById(butterflyId).Config, size);
            return (size, cost);
        }

        public void PostButterfly(ButterflyView butterfly)
        {
            var recordInfoReactive = new ReactiveProperty<ERecordType>();
            var entry = butterfly.CreateJournalEntry();
            entry.Cost = ButterfliesUtils.CalculateCost(_generalConfig, butterfly.Config, entry.Size);
            entry.RecordType = recordInfoReactive;
            entry.Icon = butterfly.SpriteRenderer.sprite;
            entry.Name = ButterfliesUtils.GetButterflyName(_generalConfig, butterfly.Config, entry.Size);
            entry.IsNewEntry = !_butterfliesSizesMap.ContainsKey(entry.Id);
            bool isLocalRecord = false;
            _wallet.Earn(entry.Cost);

            if (entry.IsNewEntry)
            {
                _butterfliesSizesMap.Add(entry.Id, entry.Size);
            }
            else
            {
                isLocalRecord = _butterfliesSizesMap[entry.Id] < entry.Size;
                if (isLocalRecord)
                    _butterfliesSizesMap[entry.Id] = entry.Size;
            }
            
            UniTask.Void(async () =>
            {
                bool isWorldRecord = false;
                try
                {
                    var worldRecords = await ButterfliesServer.GetButterflies();
                    if (worldRecords.Any(w => w.id == entry.Id))
                        isWorldRecord = worldRecords.First(w => w.id == entry.Id).size < entry.Size;
                    else
                        isWorldRecord = true;
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }

                recordInfoReactive.Value = isWorldRecord ? ERecordType.WorldRecord : 
                    isLocalRecord ? ERecordType.LocalRecord : ERecordType.None;
                ButterfliesServer.PostButterfly(entry.Id, entry.Size)
                    .Forget(Debug.LogException);
            });
            
            _entryAddSubject.OnNext(entry);
            if (entry.IsNewEntry || isLocalRecord)
                Save();
        }

        public Observable<ButterfliesJournalEntry> ObserveEntries() => _entryAddSubject;

        private void Load()
        {
            _butterfliesSizesMap.Clear();
            var loadData = UserDataManager.GetString("journal_data", null);
            if (string.IsNullOrEmpty(loadData)) 
                return;
            var fromJson = JsonUtility.FromJson<ButterfliesJournalData>(loadData);
            foreach (var entry in fromJson.Entries)
                _butterfliesSizesMap.Add(entry.Id, entry.Size);
        }

        private void Save()
        {
            var data = new ButterfliesJournalData()
            {
                Entries = _butterfliesSizesMap.Select(pair => new ButterfliesJournalEntry()
                    {
                        Id = pair.Key,
                        Size = pair.Value
                    })
                    .ToArray()
            };
            UserDataManager.SetString("journal_data", JsonUtility.ToJson(data));
        }
    }
}