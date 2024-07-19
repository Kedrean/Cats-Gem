using UnityEngine;

public class Cat : MonoBehaviour
{
    private GameController gameController;
    private bool hasBeenClicked = false;

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
        if (!hasBeenClicked)
        {
            hasBeenClicked = true;
            gameController.IncreaseScoreAndReplaceCat(gameObject);
        }
    }
}
