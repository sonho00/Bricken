using UnityEngine;
using UnityEngine.EventSystems;

public class LineInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private LineRenderer line;
    private const float minY = 300f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        line.enabled = true;
        eventData.pressPosition = new Vector2(eventData.position.x, minY);
        line.SetPosition(0, eventData.pressPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        line.SetPosition(1, eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        line.enabled = false;
        if (eventData.position.y < minY)
        {
            gameManager.gameState = GameState.Ready;
        }
        else
        {
            gameManager.Fire(eventData.pressPosition, eventData.position);
        }
    }
}
