using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : Item
{
    [SerializeField] private float speedIncreased = 0.01f;

    public delegate void ItemAppliedEvent(Transform t);
    public event ItemAppliedEvent itemUsedEvent;

    public override void ApplyEffect(Snake snake){
        snake.SetSpeed(snake.moveTimerMax - this.speedIncreased);
        this.itemUsedEvent?.Invoke(this.transform);
        base.ApplyEffect(snake);
    }
}
