using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    public bool Color—hanged = false;
    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private Color GenerateRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
    }

    private void OnTriggerEnter()
    {
        if (!Color—hanged)
        {
            Color—hanged = true;
            renderer.material.color = GenerateRandomColor();
        }
    }
}

