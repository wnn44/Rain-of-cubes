using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private bool _hasCollided = false;
    private Renderer _renderer;
    private Color _initialColor = Color.blue;
    private float _minTime = 2.0f;
    private float _maxTime = 5.0f;
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
    
    public void Init(ObjectPool<GameObject> pool)
    {
        _pool = pool;
        _hasCollided = false;
        _renderer.material.color = _initialColor;
    }

    private Color GenerateRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
    }

    private void OnTriggerEnter()
    {
        if (!_hasCollided)
        {
            _hasCollided = true;
            _renderer.material.color = GenerateRandomColor();

            float lifetime = Random.Range(_minTime, _maxTime);
            StartCoroutine(ReturnToPoolAfterDelay(lifetime));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _pool.Release(gameObject);
    }
}

