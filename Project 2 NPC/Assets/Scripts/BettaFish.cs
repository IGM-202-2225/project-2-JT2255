using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BettaFish : Agent
{
    public enum State
    {
        Healthy,
        Hungry
    }

    private State currentState = State.Healthy;
    public State CurrentState => currentState;
    public SpriteRenderer spriteRenderer;
    public float healthTimer = 10f;
    public Sprite healthySprite;
    public Sprite hungrySprite;

    protected override void CalculateSteeringForces()
    {
        Goldfish targetFish = AgentManager.Instance.GetClosestGoldfish(this);
        GameObject targetFood = AgentManager.Instance.GetClosestFood(this);

        switch (currentState)
        {
            case State.Healthy:
                {
                    float distToFish = Vector3.SqrMagnitude(physicsObject.Position - targetFish.physicsObject.Position);

                    if (IsTouchingGoldfish(targetFish) && targetFish.CurrentState == Goldfish.State.Healthy)
                    {
                        targetFish.StateTransition(Goldfish.State.Hurt);
                        //StateTransition(State.Hungry);
                    }
                    else
                    {
                        if (distToFish < Mathf.Pow(3f, 2) && targetFish.CurrentState == Goldfish.State.Healthy)
                        {
                            Seek(targetFish.physicsObject.Position);
                        }
                        else
                        {
                            Wander();
                        }
                    }

                    healthTimer -= Time.deltaTime;

                    if (healthTimer <= 0f)
                    {
                        StateTransition(State.Hungry);
                    }

                    Seperate(AgentManager.Instance.bettaFishes);
                    break;
                }
            case State.Hungry:
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

                            Seperate(AgentManager.Instance.bettaFishes);
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

    private void StateTransition(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Healthy:
                {
                    spriteRenderer.sprite = healthySprite;
                    healthTimer = 10f;
                    break;
                }
            case State.Hungry:
                {
                    spriteRenderer.sprite = hungrySprite;
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

    private bool IsTouchingGoldfish(Goldfish fish)
    {
        float sqrDistance = Vector3.SqrMagnitude(physicsObject.Position - fish.physicsObject.Position);
        float sqrRadii = Mathf.Pow(physicsObject.radius, 2) + Mathf.Pow(fish.physicsObject.radius, 2);

        return sqrDistance < sqrRadii;
    }
}
