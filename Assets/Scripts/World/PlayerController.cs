using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float tileSize = 1f;

    private Vector2Int gridPosition;
    private bool isMoving;

    private void Update()
    {
        if (isMoving)
        {
            return;
        }

        Vector2Int input = ReadDirectionalInput();
        if (input != Vector2Int.zero)
        {
            // TODO: collision/walkability check against the tilemap before committing the move.
            gridPosition += input;
        }
    }

    private Vector2Int ReadDirectionalInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) return Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) return Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) return Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) return Vector2Int.right;
        return Vector2Int.zero;
    }
}
