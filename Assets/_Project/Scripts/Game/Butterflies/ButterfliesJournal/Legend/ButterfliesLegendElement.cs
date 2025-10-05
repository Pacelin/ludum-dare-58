using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;

namespace Scripts.Game.Butterflies.ButterfliesJournal.Legend
{
    public class ButterfliesLegendElement : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space]
        [SerializeField] private Button _button;
        [SerializeField] private Image _butterflyIcon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _sizeText;
        [SerializeField] private GameObject _newRecordObject;
        [SerializeField] private GameObject _newWorldRecordObject;
        [SerializeField] private GameObject _newSpeciesObject;

        private CompositeDisposable _disposables;
        
        public void Initialize(ButterfliesLegend legend, ButterfliesJournalView journalView, ButterfliesJournalEntry entry)
        {
            _butterflyIcon.sprite = entry.Icon;
            _sizeText.text = $"{entry.Size:F} mm";
            _nameText.text = entry.Name;
            _costText.text = $"{entry.Cost}";
            _newSpeciesObject.SetActive(entry.IsNewEntry);

            _disposables = new CompositeDisposable();
            entry.RecordType.Subscribe(type =>
            {
                _newRecordObject.SetActive(type == ERecordType.LocalRecord);
                _newWorldRecordObject.SetActive(type == ERecordType.WorldRecord);
            }).AddTo(_disposables);
            _button.OnClickAsObservable()
                .Subscribe(_ => journalView.Open(entry.Id))
                .AddTo(_disposables);
            
            DOTween.Sequence(gameObject)
                .Append(transform.DOScale(1, 0.4f)
                    .From(0)
                    .OnUpdate(() => legend.UpdateLayout()))
                .AppendInterval(4)
                .Append(_canvasGroup.DOFade(0, 1f))
                .AppendCallback(() => Destroy(gameObject));
        }

        private void OnDisable()
        {
            _disposables?.Dispose();
            DOTween.Kill(gameObject);
        }
    }
}