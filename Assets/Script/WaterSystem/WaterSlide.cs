using UnityEngine;
using DrawMesh;

[RequireComponent(typeof(PolygonCollider2D))]
public class WaterSlide : MonoBehaviour
{
    [Header("Slide Physics")]
    [Tooltip("Lower friction makes water slide faster")]
    public float slideFriction = 0.01f;
    [Tooltip("Bounciness of the slide")]
    public float slideBounciness = 0.1f;

    private void Start()
    {
        ApplySlidePhysics();
    }

    public void ApplySlidePhysics()
    {
        // 1. Create a custom Physics Material for the "Pipe" effect
        PhysicsMaterial2D slideMat = new PhysicsMaterial2D("WaterSlideMaterial");
        slideMat.friction = slideFriction;
        slideMat.bounciness = slideBounciness;

        // 2. Apply it to the Collider and Rigidbody
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (poly != null) poly.sharedMaterial = slideMat;
        if (rb != null) rb.sharedMaterial = slideMat;
        
        // 3. Ensure the layer is correct for water interaction
        // Based on your DrawSettings, this is usually handled in EndDraw()
    }
}