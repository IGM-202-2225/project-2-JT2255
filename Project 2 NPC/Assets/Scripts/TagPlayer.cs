using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagPlayer : Agent
{
    public enum TagState
    {
        It,
        NotIt,
        Counting
    }

    private TagState currentState = TagState.NotIt;

    public TagState CurrentState => currentState;
    private float countdownTimer = 0;
    public float visionDistance = 4f;
    public SpriteRenderer spriteRenderer;
    public Sprite itSprite;
    public Sprite notItSprite;
    
    protected override void CalculateSteeringForces()
    {
        switch (currentState)
        {
            case TagState.It:
            {
                TagPlayer targetPlayer = AgentManager.Instance.GetClosestTagPlayer(this);

                if (IsTouching(targetPlayer))
                {
                    targetPlayer.Tag();
                    StateTransition(TagState.NotIt);
                }
                else
                {
                    Seek(targetPlayer.physicsObject.Position);
                }
                break;
            }
            case TagState.Counting:
            {
                countdownTimer -= Time.deltaTime;

                if (countdownTimer <= 0f)
                {
                    StateTransition(TagState.It);
                }
                break;
            }
            case TagState.NotIt:
            {
                TagPlayer currentIt = AgentManager.Instance.currentItPlayer;

                float distToIt = Vector3.SqrMagnitude(physicsObject.Position - currentIt.physicsObject.Position);

                if (distToIt < Mathf.Pow(visionDistance, 2))
                {
                    Flee(currentIt.physicsObject.Position);
                }
                else
                {
                    Wander();
                }
                
                Seperate(AgentManager.Instance.tagPlayers);
                
                break;
            }
        }
        
        StayInBounds(4);
    }

    private void StateTransition(TagState newTagState)
    {
        currentState = newTagState;
        
        switch (newTagState)
        {
            case TagState.It:
            {
                spriteRenderer.sprite = itSprite;
                physicsObject.useFriction = false;
                
                break;
            }
            case TagState.Counting:
            {
                countdownTimer = AgentManager.Instance.countdownTime;
                AgentManager.Instance.currentItPlayer = this;
                spriteRenderer.sprite = itSprite;
                physicsObject.useFriction = true;
                
                break;
            }
            case TagState.NotIt:
            {
                spriteRenderer.sprite = notItSprite;
                physicsObject.useFriction = false;
                
                break;
            }
        }
    }

    public void Tag()
    {
        StateTransition(TagState.Counting);
    }
    
    private bool IsTouching(TagPlayer otherPlayer)
    {
        float sqrDistance = Vector3.SqrMagnitude(physicsObject.Position - otherPlayer.physicsObject.Position);
        float sqrRadii = Mathf.Pow(physicsObject.radius, 2) + Mathf.Pow(otherPlayer.physicsObject.radius, 2);

        return sqrDistance < sqrRadii;
    }
}
