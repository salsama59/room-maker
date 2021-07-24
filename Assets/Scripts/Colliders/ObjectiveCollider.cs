using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveCollider : MonoBehaviour
{

    public bool isOccupiedByObstacle = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            this.isOccupiedByObstacle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            this.isOccupiedByObstacle = false;
        }
    }
}
