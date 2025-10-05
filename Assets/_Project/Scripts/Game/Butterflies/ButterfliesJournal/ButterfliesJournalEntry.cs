using R3;

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
    }
}