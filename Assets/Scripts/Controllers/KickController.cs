using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickController : MonoBehaviour
{
    private const float MAX_COOLDOWN_TIME = 0.2f;
    private float kickRechargeTime = 0f;
    private bool isKickCoolDown = true;
    public int playerId;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsKickCoolDown)
        {
            KickRechargeTime += Time.deltaTime;
        }

        if (playerId == 1 && KickRechargeTime >= MAX_COOLDOWN_TIME)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bool isObstacleKicked = this.KickObstacle();

                if(isObstacleKicked)
                {
                    KickRechargeTime = 0f;
                    IsKickCoolDown = true;
                }
                
            }

            IsKickCoolDown = false;
        }
        else if(playerId == 2 && KickRechargeTime >= MAX_COOLDOWN_TIME)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                bool isObstacleKicked = this.KickObstacle();

                if (isObstacleKicked)
                {
                    KickRechargeTime = 0f;
                    IsKickCoolDown = true;
                }
            }

            IsKickCoolDown = false;
        }
    }

    public bool KickObstacle()
    {
        Vector3 targetPosition = this.transform.position + this.transform.right;
        Vector2 squareSize = new Vector2(0.90f, 0.90f);
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, squareSize, 0f, this.transform.right, Vector3.Distance(this.transform.position, targetPosition));

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Obstacle"))
        {
            hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(this.transform.right.x, this.transform.right.y) * 100);
        }

        return hit.collider != null && hit.collider.gameObject.CompareTag("Obstacle");
    }

    public bool IsKickCoolDown { get => isKickCoolDown; set => isKickCoolDown = value; }
    public float KickRechargeTime { get => kickRechargeTime; set => kickRechargeTime = value; }
}
