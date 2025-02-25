using UnityEngine;

public class ColorModifier : MonoBehaviour
{
    public void RColor(Cube cube)
    {
        Renderer renderer;

        cube.TryGetComponent<Renderer>(out renderer);

        renderer.material.color =  UnityEngine.Random.ColorHSV();
    }
}
