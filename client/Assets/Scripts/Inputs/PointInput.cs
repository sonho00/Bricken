using UnityEngine;
using UnityEngine.EventSystems;

public class PointInput : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BrickManager brickManager;
    public GameObject cursor;

    private void OnDisable()
    {
        cursor.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        cursor.SetActive(true);
        // Debug.Log(eventData.position);
        Vector2 offset = new Vector2(Screen.width / 2f - 540f, Screen.height / 2f - 1080f);
        // Debug.Log(offset);
        cursor.transform.position = brickManager.GetClosestBrick(eventData.position - offset).transform.position;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursor.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameManager.Fire(eventData.pressPosition, cursor.transform.position);
    }
}
