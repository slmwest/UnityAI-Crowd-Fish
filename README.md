# Flock and FlockManager

## Flock.cs
This script represents an individual fish in a flock. It contains the logic for the fish's movement and behavior within the flock.

### Variables
- `speed`: The current speed of the fish.
- `turning`: A boolean flag indicating whether the fish is turning or not.

### Methods
- `Start()`: Initializes the fish's speed randomly within the range specified by the FlockManager.
- `Update()`: Handles the fish's movement and behavior. It checks if the fish is within the bounds of the flock, adjusts its speed randomly, and applies flocking rules to align, separate, and cohere with neighboring fish.
- `ReturnToFlock()`: Sets the destination of the fish back to the center of the flock by adjusting its rotation.
- `ApplyFlockRules()`: Applies the three rules of flocking (cohesion, separation, and alignment) to the fish.

## FlockManager.cs
This script manages the flock of fish. It creates and initializes the fish, updates the goal position of the flock, and provides settings for the fish's behavior.

### Variables
- `FM`: A static reference to the singleton instance of the FlockManager.
- `fishPrefab`: The prefab for the first type of fish.
- `fishPrefab2`: The prefab for the second type of fish.
- `numFish`: The number of fish in the flock.
- `allFish`: An array containing references to all the fish in the flock.
- `swimLimits`: The limits within which the fish can swim.
- `goalPos`: The current goal position of the flock.

### Settings
- `minSpeed`: The minimum speed of the fish.
- `maxSpeed`: The maximum speed of the fish.
- `neighbourDistance`: The distance within which the fish consider each other as neighbors.
- `rotationSpeed`: The speed at which the fish rotate.
- `avoidDistance`: The distance at which the fish start avoiding each other.
- `changeGoalPosRate`: The rate at which the goal position of the flock changes.
- `changeSpeedRate`: The rate at which the speed of the fish changes.

### Methods
- `Start()`: Creates and initializes the fish within the specified swim limits.
- `Update()`: Updates the goal position of the flock with a certain probability.
