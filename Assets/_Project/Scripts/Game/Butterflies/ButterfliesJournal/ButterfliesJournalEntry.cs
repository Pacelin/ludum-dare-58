using R3;
using UnityEngine;

namespace Scripts.Game.Butterflies.ButterfliesJournal
{
    [System.Serializable] 
    public struct ButterfliesJournalEntry
    {
        public int Id;
        public float Size;
        [System.NonSerialized] public int Cost;
        [System.NonSerialized] public ReadOnlyReactiveProperty<ERecordType> RecordType;
        [System.NonSerialized] public bool IsNewEntry;
        [System.NonSerialized] public Sprite Icon;
        [System.NonSerialized] public string Name;
    }
}