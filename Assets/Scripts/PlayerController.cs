using UnityEngine;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{

    private Cube _prefab;
    private int _poolSize = 10;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            maxSize: _poolSize);
    }
}
