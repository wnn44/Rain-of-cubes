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

    private void OnEnable()
    {
        Cube.ReturnToPool += Release;
    }

    private void OnDisabe()
    {
        Cube.ReturnToPool -= Release;
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private IEnumerator SpawnCubes()
    {
        while (true)
        {        
            GetCube();
            yield return new WaitForSeconds(_repeatRate);
        }
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = StartPoint();
        cube.Init();
        cube.gameObject.SetActive(true);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void Release(Cube cube)
    {
        _pool.Release(cube);
    }

    private Vector3 StartPoint()
    {
        float y = _startPoint.transform.position.y;
        float x = Random.Range(_startPointMin, _startPointMax);
        float z = Random.Range(_startPointMin, _startPointMax);

        return new Vector3(x, y, z);
    }
}

