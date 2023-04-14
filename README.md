# Project Fish Sim

### Student Info

-   Name: Joe Trovato
-   Section: 02

## Simulation Design

Simulation of a fish tank with 2 different types of fish, both seek food, but one fish also flees from the other. Player able to spawn in fish food in order to feed the fish or else they will die.

### Controls

-   Click to drop fish food

## Betta Fish

This fish primarily seeks food, while also making other fish types flee away from it.

### Healthy

**Objective:** Seeks other fish

#### Steering Behaviors

- Seek
   - Seeks nearest fish food
   - Seeks other fish
- Obstacles - Avoids coral in the fish tank
- Seperation -
   
#### State Transistions

- After eating food, fish will become Healthy
   
### Hungry

**Objective:** Seek fish food

#### Steering Behaviors

- Seek
    - Seeks nearest fish food
- Obstacles - Avoids coral and other fish
- Seperation - 
   
#### State Transistions

- When this fish gets in range of another fish, it will attack it and become Hungry

## Goldfish

This fish tries to stay safe and eat food

### Healthy

**Objective:** In this state, this fish will flee from all other Betta Fish

#### Steering Behaviors

- Flee
   - Flees from betta fish
- Obstacles - Coral and Betta Fish
- Seperation - 
   
#### State Transistions

- After eating food, goldfish will become healthy
   
### Hurt

**Objective:** Try to find fish food in order to survive

#### Steering Behaviors

- Seek
   - Seek fish food
- Obstacles - Coral
- Seperation - 
   
#### State Transistions

- When attacked by another Betta Fish, the goldfish will become hurt

## Sources

-   _List all project sources here –models, textures, sound clips, assets, etc._
-   _If an asset is from the Unity store, include a link to the page and the author’s name_

## Make it Your Own

- Will be making all assets for game myself including food and fish sprites

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

