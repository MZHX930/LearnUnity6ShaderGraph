using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class LiuGuangScroll : MonoBehaviour
{
    [SerializeField] private Vector2 speed = new Vector2(0.5f, 0f);
    [SerializeField] private string textureName = "_MainTex";

    private Graphic graphic;
    private Material runtimeMaterial;

    private void Awake()
    {
        graphic = GetComponent<Graphic>();
    }

    private void OnEnable()
    {
        EnsureMaterialInstance();
    }

    private void OnDisable()
    {
        if (runtimeMaterial == null)
        {
            return;
        }

        Destroy(runtimeMaterial);
        runtimeMaterial = null;
    }

    private void Update()
    {
        if (runtimeMaterial == null)
        {
            return;
        }

        var offset = runtimeMaterial.GetTextureOffset(textureName);
        offset += speed * Time.unscaledDeltaTime;
        runtimeMaterial.SetTextureOffset(textureName, offset);
    }

    private void EnsureMaterialInstance()
    {
        if (graphic == null)
        {
            graphic = GetComponent<Graphic>();
        }

        var baseMaterial = graphic.material;
        if (baseMaterial == null)
        {
            return;
        }

        runtimeMaterial = new Material(baseMaterial)
        {
            name = baseMaterial.name + " (Runtime)"
        };
        graphic.material = runtimeMaterial;
    }
}

