using UnityEngine;

namespace Scripts.MainMenu
{
    public class RandomSpawn : MonoBehaviour
    {
        [SerializeField] private Bounds _bounds;
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private Vector2 _cooldownRange;

        private float _cooldown;

        public void Spawn()
        {
            var prefab = _prefabs[UnityEngine.Random.Range(0, _prefabs.Length)];
            var xPos = UnityEngine.Random.Range(_bounds.min.x, _bounds.max.x);
            var yPos = UnityEngine.Random.Range(_bounds.min.y, _bounds.max.y);
            var pos = new Vector3(xPos, yPos);
            var obj = Instantiate(prefab, pos, Quaternion.identity);
            if (obj.TryGetComponent<ISpawnCallback>(out var callback))
                callback.OnSpawn(this);
        }
        
        private void Awake()
        {
            _cooldown = UnityEngine.Random.Range(_cooldownRange.x, _cooldownRange.y);
        }

        private void Update()
        {
            _cooldown -= Time.deltaTime;
            if (_cooldown <= 0)
            {
                Spawn();
                _cooldown = UnityEngine.Random.Range(_cooldownRange.x, _cooldownRange.y);
            }
        }

        private void OnDrawGizmos() => Gizmos.DrawWireCube(_bounds.center, _bounds.size);
    }
}