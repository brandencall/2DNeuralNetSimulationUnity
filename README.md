## **Description**:
 This 2D simulation project was made in unity and demonstrates how a neural net will learn over time using a genetic algorithm. 

## **Breakdown**: 
 There are 4 main components of the project:
* Simulation Manager
  * Main loop for the simulation. Manages simulation parameters as well as population and repopulating the simulation.
  * Starts with 1000 entities and repopulates the next generation with the entities that end in the safe zone.
* Entity
  * Each entity is placed in a random position in the world. They all start off with a random neural net brain.
  * The color of the entity depends on their neural net brain. The more similar the entities neural nets are then the more similar the brains will be. Over generations the entities become a similar color.
  * Contains the neural net configuration options
    * Number of inputs.
    * Number of outputs.
    * Number of layers (hidden layer number).
    * Number of hidden neurons per layer.
  * The inputs for the neural net are:
    * northBoarderDistance
      * Distance from the north border.
    * southBoarderDistance
      * Distance from the south border.
    * westBoarderDistance
      * Distance from the west border.
    * eastBoarderDistance
      * Distance from the east border.
    * distanceFromCenter
      * Distance from the center. Calculated using the distance formula between 2 points
				![main-450-250 dark](https://github.com/brandencall/2DNeuralNetSimulationUnity/assets/54908229/23a2b480-5de9-449a-9b14-6475ca365b04)

        <https://wumbo.net/formulas/distance-between-two-points-2d/>
    * entitiesForward
      * Raycast in the entity's forward direction. If the Raycast hits an entity then it returns a 1 else it returns a 0.
    * entitiesLeft
      * Raycast in the entity's left direction. If the Raycast hits an entity then it returns a 1 else it returns a 0.
    * entitiesRight
      * Raycast in the entity's right direction. If the Raycast hits an entity then it returns a 1 else it returns a 0.
  * The outputs for the neural net are:
    * moveSpeed
      * The speed the entity will move
    * northInput
      * Determines if the entity moves north.
    * southInput
      * Determines if the entity moves south.
    * eastinput
      * Determines if the entity moves east.
    * westInput
      * Determines if the entity moves west.
* Neural Net
  * This class uses the MathNet.Numerics.LinearAlgebra library to perform matrix multiplication
  * If a new neural net is created then the Initialize method is called. This method creates the different weight and bias matrices based on the number of inputs, number of hidden neurons, number of hidden layers and number of outputs.
    * The weight matrices are initialized with random weights between -4 and 4.
  * The RunNetork takes in a list of inputs from the entity and outputs a list to the entity.
    * The inputs are run through a sigmoid function to set them between 0 and 1.
    * The layers are all activated with a Tanh function (sets a value between -1 and 1)
* Genetic Manager
  * After the current generation is complete (parameter set in the simulation manager), the genetic manager is called and is given all of the entities that made it to the safe zone.
  * The CrossOver function takes all of the safe entities and randomly picks 2 safe entities. Two offspring are created with these 2 entities neural nets. For all of the parents' weight matrices, each one has a 50% chance of going to either child 1 or child 2. The same thing happens with the parents' bias matrices.
  * After the new generation is made from the previous generation, we perform a mutation on the weight matrices. A mutation occurs .5% (which may be a little high and could be lowered to .1%) for every weight in the new generation.

## **Examples of the simulation running**:
* Simulation parameters:
  * Generation life span: 10 seconds
  * Number of Entities: 500
* Entity parameters:
  * Hidden layers: 1
  * Hidden Neurons: 12

 The green area is the safe zone for all of the entities.

**Generation 0:**

https://github.com/brandencall/2DNeuralNetSimulationUnity/assets/54908229/f9646ba7-1682-4416-af63-148d4a0f9a25

Safe entities: 251

**Generation 10:**

https://github.com/brandencall/2DNeuralNetSimulationUnity/assets/54908229/254350a4-aaa6-404a-940f-582e44c1eeeb

Safe entities: 294

**Generation 100:**

https://github.com/brandencall/2DNeuralNetSimulationUnity/assets/54908229/0594ca35-1c04-4074-b4b3-c960a4089e1f

Safe entities: 488


Over time the entities learn that they need to head to the east border in order to survive. Eventually all entities in this simulation will survive.

References:
* Inspiration: <https://www.youtube.com/watch?v=N3tRFayqVtk&t=1593s>
* <https://www.youtube.com/watch?v=C6SZUU8XQQ0>

