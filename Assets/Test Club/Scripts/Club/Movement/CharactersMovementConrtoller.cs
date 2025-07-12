using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    public static CharacterMovementController Instance;

    [SerializeField] private TilemapGridBuilder tilemapGridBuilder;

    private void Awake()
    {
        Instance = this;
    }

    private void OnArrivalSubscribe(AStarMovement movement, IOnArrivalHandler arrivalHandler)
    {
        if (movement != null && arrivalHandler != null)
        {
            movement.OnArrivedAtDestination = arrivalHandler.Arrived;
            Debug.Log("Character subscribed on destination point arrival");
        }
        else
        {
            Debug.LogWarning("Cannot subscribe character on arrival: movement script does not exist");
        }
    }

    public void AStarMoveTo(GameObject character, Vector3 destination)
    {
        var pathfinder = new AStarPathfinder(tilemapGridBuilder.GetPathfindingParams());
        var path = pathfinder.FindPath(character.transform.position, destination);
        character.GetComponent<AStarMovement>().StartMoving(path);
    }

    public void AStarMoveTo(GameObject character, Transform destination, bool subscribeOnArrival)
    {
        var pathfinder = new AStarPathfinder(tilemapGridBuilder.GetPathfindingParams());
        var path = pathfinder.FindPath(character.transform.position, destination.position);
        Debug.Log("Generated path from " + character.name + " current point to " + destination.name + " transform");

        if (subscribeOnArrival)
        {
            var arrival = destination.GetComponent<IOnArrivalHandler>();
            if (arrival != null)
            {
                OnArrivalSubscribe(character.GetComponent<AStarMovement>(), arrival);
            } else
            {
                Debug.Log("Destination point does not implement IOnArrivalHandler so character cannot subscribe to it");
            }
        }
        character.GetComponent<AStarMovement>().StartMoving(path);
    }
}