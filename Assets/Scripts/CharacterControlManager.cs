using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlManager : MonoBehaviour
{
    public float moveSpeed = 1f;

    public Rigidbody2D rigidBody2d;

    Vector2 movement;

    public GameObject aim;

    private float time = 0f;

    private float remainingTimeBeforeMove = 0f;


    public Animator animator;


    public GameManager gameManager;

    public Transform aimTransform;

    public int playerId;

    private void Start()
    {
        this.time = moveSpeed / 1f;
    }


    private void Update()
    {
        if(!gameManager.isEndGame)
        {


            if(playerId == 1)
            {
                //Input
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");
            } else
            {
                if(Input.GetKey(KeyCode.Keypad4)){
                    movement.x = -1;
                } else if (Input.GetKey(KeyCode.Keypad6))
                {
                    movement.x = 1;
                }
                else
                {
                    movement.x = 0;
                }

                if (Input.GetKey(KeyCode.Keypad8))
                {
                    movement.y = 1;
                }
                else if (Input.GetKey(KeyCode.Keypad5))
                {
                    movement.y = -1;
                }
                else
                {
                    movement.y = 0;
                }
            }
            
            if(movement.x != 0f || movement.y != 0f)
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
            }
           
            //animator.SetFloat("Speed", movement.sqrMagnitude);

            if (movement.x > 0)
            {
                aim.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            else if (movement.x < 0)
            {
                aim.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
            }

            if (movement.y > 0)
            {
                aim.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
            }
            else if (movement.y < 0)
            {
                aim.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));
            }
        }
        
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.isEndGame)
        {
            remainingTimeBeforeMove += Time.fixedDeltaTime;

            //Movement
            if (remainingTimeBeforeMove >= this.time)
            {
                if (IsMovePossible())
                {
                    rigidBody2d.MovePosition(rigidBody2d.position + movement);
                    remainingTimeBeforeMove = 0f;
                }
                
            }
        }
    }

    public bool IsMovePossible()
    {
        Vector3 targetPosition = rigidBody2d.position + movement;
        Vector2 squareSize = new Vector2(0.90f, 0.90f);
        //LayerMask.NameToLayer("")
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, squareSize, 0f, aimTransform.right, Vector3.Distance(this.transform.position, targetPosition));
        return hit.collider == null;
    }
}
