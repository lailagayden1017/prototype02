using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float baseScale = 0.5f;
    [SerializeField] private float scaleIncreaseRate = 0.05f;
    [SerializeField] private float baseStickRange = 1f;
    [SerializeField] private float rotationSpeed = 100f;  // Degrees per second

    private Rigidbody2D rb;
    private List<StuckObjectData> stuckObjects = new List<StuckObjectData>();

    private float CurrentStickRange => baseStickRange * (transform.localScale.x / baseScale);

    private class StuckObjectData
    {
        public GameObject obj;
        public Vector2 relativePosition;

        public StuckObjectData(GameObject obj, Vector2 relativePos)
        {
            this.obj = obj;
            this.relativePosition = relativePos;
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(baseScale, baseScale, 1);
        GetComponent<CircleCollider2D>().radius *= (1 / baseScale);
    }

    private void CheckForStickableObjects()
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, CurrentStickRange);
        foreach (Collider2D collider in nearbyObjects)
        {
            if (collider.CompareTag("Stickable") && !stuckObjects.Exists(x => x.obj == collider.gameObject))
            {
                Vector2 relativePos = collider.transform.position - transform.position;
                StickObject(collider.gameObject, relativePos);
            }
        }
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        CheckForStickableObjects();
        UpdateStuckObjects();
    }

    private void HandleRotation()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(moveX, moveY) * moveSpeed;
    }


    private void UpdateStuckObjects()
    {
        foreach (var stuckData in stuckObjects)
        {
            if (stuckData.obj != null)
            {
                stuckData.obj.transform.position = (Vector2)transform.position + stuckData.relativePosition;
            }
        }
    }

    private void StickObject(GameObject obj, Vector2 relativePos)
    {
        stuckObjects.Add(new StuckObjectData(obj, relativePos));
        obj.GetComponent<Rigidbody2D>().simulated = false;

        float newScale = baseScale + (stuckObjects.Count * scaleIncreaseRate);
        transform.localScale = new Vector3(newScale, newScale, 1);
    }
}