using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam; // Reference to the camera
    public Transform followTarget; // The object the parallax effect will follow

    // Initial positions and distances
    private Vector2 startingPosition; // The starting position of the object
    private float startingZ; // The initial Z position of the object

    // Properties to calculate parallax effect
    private Vector2 canMoveSinceStart => (Vector2)cam.transform.position - startingPosition; // Calculates movement since the start
    private float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z; // Calculates the Z distance between the object and the target
    private float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane)); // Calculates the clipping plane based on the distance
    private float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane; // Calculates the parallax factor

    void Start()
    {
        startingPosition = transform.position; // Set the initial position of the object
        startingZ = transform.position.z; // Set the initial Z position of the object
    }

    void Update()
    {
        // Calculate the new position with the parallax effect
        Vector2 newPosition = startingPosition + canMoveSinceStart * parallaxFactor;

        // Update the object's position with the parallax effect
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
