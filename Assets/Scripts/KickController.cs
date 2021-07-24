using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickController : MonoBehaviour
{


    public int playerId;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (playerId == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.KickObstacle();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                this.KickObstacle();
            }
        }
    }

    public void KickObstacle()
    {
        Vector3 targetPosition = this.transform.position + this.transform.right;
        Vector2 squareSize = new Vector2(0.90f, 0.90f);
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, squareSize, 0f, this.transform.right, Vector3.Distance(this.transform.position, targetPosition));

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Obstacle"))
        {
            hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(this.transform.right.x, this.transform.right.y) * 100);
        }
    }
}
