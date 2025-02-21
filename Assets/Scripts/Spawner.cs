using UnityEngine.Pool;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private SpawnPoint _startPoint;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<GameObject> _pool;
    private float _startPointMin = -10;
    private float _startPointMax = 10;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => obj.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetSphere), 0.0f, _repeatRate);
    }

    private void ActionOnGet(GameObject obj)
    {

        obj.transform.position = StartPoint();
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Cube controller = obj.GetComponent<Cube>();
        controller.Init(_pool);

        obj.SetActive(true);
    }

    private void GetSphere()
    {
        _pool.Get();        
    }

    private Vector3 StartPoint()
    {
        float y = _startPoint.transform.position.y;
        float x = Random.Range(_startPointMin, _startPointMax);
        float z = Random.Range(_startPointMin, _startPointMax);

        return new Vector3(x, y, z);
    }
}

