using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private bool _hasCollided = false;
    private Renderer _renderer;
    private Color _initialColor = Color.blue;
    private float _minTime = 2.0f;
    private float _maxTime = 5.0f;

    public static event Action<Cube> ReturnToPool;

    public void Init()
    {
        _hasCollided = false;
        _renderer.material.color = _initialColor;
        transform.rotation = Quaternion.identity;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private Color GenerateRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
    }

    private void OnTriggerEnter()
    {
        if (_hasCollided == false)
        {
            _hasCollided = true;
            _renderer.material.color = GenerateRandomColor();

            StartCoroutine(ReturnToPoolAfterDelay(UnityEngine.Random.Range(_minTime, _maxTime)));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        ReturnToPool?.Invoke(this);
    }
}

