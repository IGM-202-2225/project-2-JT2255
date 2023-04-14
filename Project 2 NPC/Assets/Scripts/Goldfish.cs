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

        switch (currentState)
        {
            case State.Healthy:
            {
                float distToFish = Vector3.SqrMagnitude(physicsObject.Position - targetFish.physicsObject.Position);
                
                if (IsTouchingBettaFish(targetFish))
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
                Wander();
                Seperate(AgentManager.Instance.goldfishes);
                //seek fish food here
                //switch to healthy after eating food
                break;
            }
        }
        
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
                break;
            }
            case State.Hurt:
            {
                spriteRenderer.sprite = hurtSprite;
                break;
            }
        }
    }

    private bool IsTouchingFood()
    {
        //check for touching food here
        return true;
    }

    private bool IsTouchingBettaFish(BettaFish fish)
    {
        float sqrDistance = Vector3.SqrMagnitude(physicsObject.Position - fish.physicsObject.Position);
        float sqrRadii = Mathf.Pow(physicsObject.radius, 2) + Mathf.Pow(fish.physicsObject.radius, 2);

        return sqrDistance < sqrRadii;
    }
}
