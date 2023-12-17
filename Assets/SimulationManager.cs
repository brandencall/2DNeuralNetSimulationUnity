using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;


public class SimulationManager : MonoBehaviour
{
    [Header("References")]
    public GameObject entityPrefab;
    public GameObject world;
    public GameObject safeZone;

    private GeneticManager geneticManager;
    private MatrixToColorMapper colorMapper;
    private SafeArea safeArea;

    [Header("Simulation Parameters")]
    public int generationLifeSpan = 10;
    public int numberOfEntities = 1000;
    public int currentGeneration = 0;

    private Entity[] population;
    private GameObject[] entityObjects;
    private NeuralNet[] newPopulationBrains;
    private List<Entity> safeEntities;

    private Vector3 entityTransform;
    private float entityRadius;
    private float timeSinceGenerationStart = 0;
    private float worldWidth;
    private float worldHeight;

    void Start()
    {
        geneticManager = new GeneticManager();
        colorMapper = new MatrixToColorMapper();
        safeArea = safeZone.GetComponent<SafeArea>();

        population = new Entity[numberOfEntities];
        entityObjects = new GameObject[numberOfEntities];
        safeEntities = new List<Entity>();

        entityTransform = entityPrefab.transform.localScale;
        entityRadius = entityTransform.x / 2;
        worldWidth = world.transform.localScale.x;
        worldHeight = world.transform.localScale.y;

        SpawnEntities();
    }

    private void SpawnEntities()
    {
        if (currentGeneration != 0)
        {
            RemoveEntities();
        }

        for (int i = 0; i < numberOfEntities; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(0 + entityRadius, worldWidth - entityRadius),
                Random.Range(0 + entityRadius, worldHeight - entityRadius));

            float spawnRotation = Random.Range(0, 360);
            Quaternion rotation = Quaternion.Euler(0f, 0f, spawnRotation);

            GameObject entityObject = Instantiate(entityPrefab, spawnPosition, rotation);
            entityObjects[i] = entityObject;
            population[i] = entityObject.GetComponent<Entity>();

            if (newPopulationBrains != null)
            {
                population[i].Initialize(newPopulationBrains[i]);
            }
            else
            {
                population[i].Initialize();
            }

            SetEntityColor(population[i].neuralNet, entityObjects[i]);
        }
    }

    private void SetEntityColor(NeuralNet brain, GameObject entityObject)
    {
        int red, green, blue;
        int weightRed, weightGreen, weightBlue;
        int biasRed, biasGreen, biasBlue;

        (weightRed, weightGreen, weightBlue) = colorMapper.ComputeWeightColor(brain.weights);
        (biasRed, biasGreen, biasBlue) = colorMapper.ComputeBiasColor(brain.biases);

        red = (weightRed + biasRed) % 255;
        green = (weightGreen + biasGreen) % 255;
        blue = (weightBlue + biasBlue) % 255;

        Color32 newColor = new Color32((byte)red, (byte)green, (byte)blue, 255);

        entityObject.GetComponent<SpriteRenderer>().color = newColor;

        
    }

    private void RemoveEntities()
    {
        for (int i = 0; i < numberOfEntities; i++)
        {
            Destroy(entityObjects[i]);
            entityObjects[i] = null;
            population[i] = null;
        }
        safeEntities.Clear();
    }

    private void FixedUpdate()
    {
        timeSinceGenerationStart += Time.deltaTime;

        if (timeSinceGenerationStart > generationLifeSpan)
        {
            // Check entities in safe zone and kill the ones not in safe zone.
            GetEntitiesInSafeZone();
            // Call the GeneticManager with the entities that are alive.
            if (safeEntities.Count >= 2)
            {
                newPopulationBrains = geneticManager.CreateOffSpring(safeEntities, numberOfEntities);
                currentGeneration++;
                SpawnEntities();
            }
            else
            {
                currentGeneration = 0;
                newPopulationBrains = null;
                RemoveEntities();
                SpawnEntities();
            }
            

            timeSinceGenerationStart = 0;           
        }
    }

    private void GetEntitiesInSafeZone()
    {
        for (int i = 0; i < numberOfEntities; i++)
        {
            if (safeArea.IsEntityInSafeZone(population[i]))
            {
                safeEntities.Add(population[i]);
            }
        }
        print(currentGeneration + ": " + safeEntities.Count);
    }
}
