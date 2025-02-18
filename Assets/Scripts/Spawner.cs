using UnityEngine.Pool;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;
    private ObjectPool<GameObject> _pool;

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

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = StartPoint();
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetSphere), 0.0f, _repeatRate);
    }

    private void GetSphere()
    {
        _pool.Get();
    }

    private void OnTriggerEnter(Collider other)
    {
        _pool.Release(other.gameObject);
    }

    private Vector3 StartPoint()
    {
        float y= _startPoint.transform.position.y;
        float x = Random.Range(-10, 10);
        float z = Random.Range(-10, 10);

        return new Vector3(x, y, z);
    }
}

