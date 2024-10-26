using UnityEngine;

public class Parallax : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;  // The layer transform
        public float parallaxSpeed;       // How fast the layer moves
    }

    public ParallaxLayer[] layers;  // Array to hold all parallax layers
    public Camera mainCamera;       // Reference to the main camera
    private Vector3 lastCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Record the initial camera position
        lastCameraPosition = mainCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate camera movement delta
        Vector3 cameraDelta = mainCamera.transform.position - lastCameraPosition;

        // Loop through each layer and adjust its position based on parallax speed
        foreach (ParallaxLayer layer in layers)
        {
            // Calculate new position for each layer
            float parallaxAmount = cameraDelta.x * layer.parallaxSpeed;
            Vector3 newLayerPosition = new Vector3(layer.layerTransform.position.x + parallaxAmount, layer.layerTransform.position.y, layer.layerTransform.position.z);

            // Apply new position to the layer
            layer.layerTransform.position = newLayerPosition;
        }

        // Update the last camera position for the next frame
        lastCameraPosition = mainCamera.transform.position;
    }
}
