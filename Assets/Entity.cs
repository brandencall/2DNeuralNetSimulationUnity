using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public NeuralNet neuralNet;

    [Header("References")]
    public GameObject world;

    [Header("NeuralNet options")]
    internal int numberOfInputs = 8;
    internal int numberOfOutputs = 5;
    public int numberOfLayers = 1;
    public int numberOfHiddenNeurons = 3;

    //NeuralNet Inputs
    private float northBoarderDistance;
    private float southBoarderDistance;
    private float westBoarderDistance;
    private float eastBoarderDistance;
    private float distanceFromCenter;
    private float entitiesForward;
    private float entitiesLeft;
    private float entitiesRight;
    private List<float> inputs;

    //NeuralNet Outputs
    public float moveSpeed;
    public float northInput;
    public float southInput;
    public float eastInput;
    public float westInput;

    //private Rigidbody2D rigidbody2;

    [Header("Entity Controlls")]
    public float raycastDistance = 5;

    private float worldWidth;
    private float worldHeight;
    private float worldCenterX;
    private float worldCenterY;


    void Start()
    {
        worldWidth = world.transform.localScale.x;
        worldHeight = world.transform.localScale.y;
        worldCenterX = world.transform.localPosition.x;
        worldCenterY = world.transform.localPosition.y;

        inputs = new List<float>();

        //rigidbody2 = GetComponent<Rigidbody2D>();

    }

    public void Initialize()
    {
        neuralNet = new NeuralNet(numberOfInputs, numberOfOutputs);
        neuralNet.Initialize(numberOfLayers, numberOfHiddenNeurons);
    }

    public void Initialize(NeuralNet neuralNet)
    {
        this.neuralNet = neuralNet;
    }

    private void FixedUpdate()
    {
        inputs.Clear();

        northBoarderDistance = GetNorthBoarderDistance();
        inputs.Add(northBoarderDistance);
        southBoarderDistance = GetSouthBoarderDistance();
        inputs.Add(southBoarderDistance);
        westBoarderDistance = GetWestBoarderDistance();
        inputs.Add(westBoarderDistance);
        eastBoarderDistance = GetEastBoarderDistance();
        inputs.Add(eastBoarderDistance);
        distanceFromCenter = GetDistanceFromWorldCenter();
        inputs.Add(distanceFromCenter);

        entitiesForward = CheckEntitiesForward();
        inputs.Add(entitiesForward);
        entitiesLeft = CheckEntitiesLeft();
        inputs.Add(entitiesLeft);
        entitiesRight = CheckEntitiesRight();
        inputs.Add(entitiesRight);

        List<float> outputs = neuralNet.RunNetwork(inputs);

        /*moveSpeed = outputs[0];
        rotationInput = outputs[1];

        MoveEntity(moveSpeed, rotationInput);*/

        moveSpeed = outputs[0];
        northInput = outputs[1];
        southInput = outputs[2];
        eastInput = outputs[3];
        westInput = outputs[4];
        MoveEntity(moveSpeed, northInput, southInput, eastInput, westInput);
    }


    private float GetNorthBoarderDistance()
    {
        return worldHeight - transform.localPosition.y;
    }

    private float GetSouthBoarderDistance()
    {
        return transform.localPosition.y;
    }

    private float GetWestBoarderDistance()
    {
        return transform.localPosition.x;
    }

    private float GetEastBoarderDistance()
    {
        return worldWidth - transform.localPosition.x;
    }

    private float GetDistanceFromWorldCenter()
    {
        return (float)Math.Sqrt(Math.Pow(worldCenterY - transform.localPosition.y, 2)
            + Math.Pow(worldCenterX - transform.localPosition.x, 2));
    }

    private int CheckEntitiesForward()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), raycastDistance);

        if (hit)
        {
            if (hit.collider.name.Equals("Entity(Clone)"))
            {
                return 1;
            }            
        }

        return 0;
    }

    private int CheckEntitiesLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), raycastDistance);

        if (hit)
        {
            if (hit.collider.name.Equals("Entity(Clone)"))
            {
                return 1;
            }
        }

        return 0;
    }

    private int CheckEntitiesRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), raycastDistance);

        if (hit)
        {
            if (hit.collider.name.Equals("Entity(Clone)"))
            {
                return 1;
            }
        }

        return 0;
    }

    public void MoveEntity(float moveSpeed, float northInput, float southInput, float eastInput, float westInput)
    {
        // Calculate the movement direction based on inputs
        Vector2 movementDirection = new Vector2(eastInput - westInput, northInput - southInput).normalized;

        // Calculate the rotation angle based on movement direction
        float targetRotation = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);

        // Apply force in the movement direction
        /* Vector2 movementForce = movementDirection * moveSpeed * 4;
         rigidbody2.AddForce(movementForce);*/

        // Apply movement directly using Transform translation
        Vector3 movement = movementDirection * moveSpeed * 10 * Time.deltaTime;
        transform.Translate(movement);
    }
}
