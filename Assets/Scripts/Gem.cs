using UnityEngine;

public class Gem : MonoBehaviour
{
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        collider.size = new Vector2(20f, 20f);
    }

    void OnMouseDown()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = Color.blue; // Change color to blue when clicked
        }

        bool isGameOver = Random.value > 0.5f; // 50/50 chance
        gameController.HandleGemInteraction(isGameOver);

        StartCoroutine(DestroyGemAfterDelay(1f)); // Destroy gem after 1 second
    }

    private System.Collections.IEnumerator DestroyGemAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
