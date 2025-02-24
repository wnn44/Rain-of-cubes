using UnityEngine.Pool;
using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private SpawnPoint _startPoint;
    [SerializeField] private float _repeatRate = 0.3f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Cube> _pool;
    private float _startPointMin = -10.0f;
    private float _startPointMax = 10.0f;
    private INotifier _notifier;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private IEnumerator SpawnCubes()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            TakeFromPool();

            yield return waitForSeconds;
        }
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = StartPoint();
        cube.Init();
        cube.gameObject.SetActive(true);
    }

    private void TakeFromPool()
    {
        Cube cube = _pool.Get();

        cube.CubeEndedLife += OnRelease;
    }

    private void OnRelease(INotifier cube)
    {
        _pool.Release((Cube)cube);

        cube.CubeEndedLife -= OnRelease;
    }

    private Vector3 StartPoint()
    {
        float y = _startPoint.transform.position.y;
        float x = UnityEngine.Random.Range(_startPointMin, _startPointMax);
        float z = UnityEngine.Random.Range(_startPointMin, _startPointMax);

        return new Vector3(x, y, z);
    }
}

