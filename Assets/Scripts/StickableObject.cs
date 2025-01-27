using UnityEngine;

public class StickableObject : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab; // Define particle prefab

    void Start()
    {
        gameObject.tag = "Stickable";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collision confirmed");
            if (particlePrefab != null)
            {
                Instantiate(particlePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("Particle prefab is null");
            }
        }
    }
}