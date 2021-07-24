using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public GameObject obstacleModel;

    private int score;

    public GameManager gameManager;

    public int playerId;

    private KickController kickControllerScript;


    // Start is called before the first frame update
    void Start()
    {
        KickControllerScript = this.gameObject.GetComponent<KickController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerId == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (this.IsSpawnPossible())
                {
                    KickControllerScript.IsKickCoolDown = true;
                    KickControllerScript.KickRechargeTime = 0f;
                    this.SpawnObstacle();
                }
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (this.IsSpawnPossible())
                {
                    KickControllerScript.IsKickCoolDown = true;
                    KickControllerScript.KickRechargeTime = 0f;
                    this.SpawnObstacle();
                }
            }
        }
        
    }


    public void SpawnObstacle()
    {
        Vector3 spawnPosition = this.PositionSnap(this.transform.position + this.transform.right);
        MapCoordinates coordinates = gameManager.ConvertWorldPositionToMapCoprdinates(spawnPosition);
        GameMapPosition gameMapPosition =  gameManager.GameMap[coordinates.lineCoordinate, coordinates.columnCoordinate];
        Instantiate(obstacleModel, spawnPosition, Quaternion.identity);

        if (gameMapPosition.hasObjective)
        {
            this.score += 5;
        } else
        {
            this.score += 1;
        }
        
        gameManager.UpdateScore(this.score.ToString(), playerId);
    }

    public bool IsSpawnPossible()
    {
        Vector3 targetPosition = this.transform.position + this.transform.right;
        Vector2 squareSize = new Vector2(0.90f, 0.90f);
        //LayerMask.NameToLayer("")
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, squareSize, 0f, this.transform.right, Vector3.Distance(this.transform.position, targetPosition));
        return hit.collider == null;
    }

    public Vector3 PositionSnap(Vector3 positionToSnap)
    {
        
        bool isXnegative = false;
        bool isYnegative = false;


        if(positionToSnap.x < 0)
        {
            isXnegative = true;
        }

        if(positionToSnap.y < 0)
        {
            isYnegative = true;
        }


        float integerPartForX = Mathf.Floor(Mathf.Abs(positionToSnap.x));
        float integerPartForY = Mathf.Floor(Mathf.Abs(positionToSnap.y));
       
        positionToSnap.x = integerPartForX + 0.5f;

        if (isXnegative)
        {
            positionToSnap.x *= -1;
        }
       
       positionToSnap.y = integerPartForY + 0.5f;
        

        if(isYnegative)
        {
            positionToSnap.y *= -1;
        }
        

        return positionToSnap;

    }

    public KickController KickControllerScript { get => kickControllerScript; set => kickControllerScript = value; }
}
