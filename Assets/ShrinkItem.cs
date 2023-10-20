using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkItem : Item
{

    public delegate void ItemAppliedEvent(Transform t);
    public event ItemAppliedEvent itemUsedEvent;

    public override void ApplyEffect(Snake snake){
        snake.Shrink();
        this.itemUsedEvent?.Invoke(this.transform);
        base.ApplyEffect(snake);
    }
}
