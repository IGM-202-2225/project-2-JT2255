using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goldfish : Agent
{
    public enum State
    {
        Healthy,
        Hurt
    }

    private State currentState = State.Healthy;
    public State CurrentState => currentState;
    public SpriteRenderer spriteRenderer;
    public Sprite healthySprite;
    public Sprite hurtSprite;

    protected override void CalculateSteeringForces()
    {
        BettaFish targetFish = AgentManager.Instance.GetClosestBettaFish(this);
        GameObject targetFood = AgentManager.Instance.GetClosestFood(this);

        switch (currentState)
        {
            case State.Healthy:
                {
                    float distToFish = Vector3.SqrMagnitude(physicsObject.Position - targetFish.physicsObject.Position);

                    if (IsTouchingBettaFish(targetFish) && targetFish.CurrentState == BettaFish.State.Healthy)
                    {
                        StateTransition(State.Hurt);
                    }
                    else
                    {
                        if (distToFish < Mathf.Pow(3f, 2))
                        {
                            Flee(targetFish.physicsObject.Position, 3);
                        }
                        else
                        {
                            Wander();
                        }

                        Seperate(AgentManager.Instance.goldfishes);
                    }

                    break;
                }
            case State.Hurt:
                {
                    if (AgentManager.Instance.fishFoodList.Count > 0)
                    {
                        float distToFood = Vector3.SqrMagnitude(physicsObject.Position - targetFood.transform.position);

                        if (IsTouchingFood(targetFood))
                        {
                            AgentManager.Instance.fishFoodList.Remove(targetFood);
                            Destroy(targetFood);
                            StateTransition(State.Healthy);
                        }
                        else
                        {
                            if (distToFood < Mathf.Pow(4f, 2))
                            {
                                Seek(targetFood.transform.position, 3);
                            }
                            else
                            {
                                Wander();
                            }

                            Seperate(AgentManager.Instance.goldfishes);
                        }
                    }
                    else
                    {
                        Wander();
                    }

                    break;
                }
        }

        AvoidAllObstacles();
        StayInBounds(4);
    }

    public void StateTransition(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Healthy:
                {
                    spriteRenderer.sprite = healthySprite;
                    break;
                }
            case State.Hurt:
                {
                    spriteRenderer.sprite = hurtSprite;
                    break;
                }
        }
    }

    private bool IsTouchingFood(GameObject food)
    {
        float sqrDistance = Vector3.SqrMagnitude(physicsObject.Position - food.transform.position);
        float sqrRadii = Mathf.Pow(physicsObject.radius, 2) + Mathf.Pow(.5f, 2);

        return sqrDistance < sqrRadii;
    }

    private bool IsTouchingBettaFish(BettaFish fish)
    {
        float sqrDistance = Vector3.SqrMagnitude(physicsObject.Position - fish.physicsObject.Position);
        float sqrRadii = Mathf.Pow(physicsObject.radius, 2) + Mathf.Pow(fish.physicsObject.radius, 2);

        return sqrDistance < sqrRadii;
    }
}
