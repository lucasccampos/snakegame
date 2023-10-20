using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{

    public delegate void ItemAppliedEvent(Transform t);
    public event ItemAppliedEvent itemUsedEvent;

    public override void ApplyEffect(Snake snake){
        snake.Grow();
        GameManager.instance?.AddScore(1);
        this.itemUsedEvent?.Invoke(this.transform);
        base.ApplyEffect(snake);
    }
}
