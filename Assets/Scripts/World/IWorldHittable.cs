using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldHittable
{
    /// <summary>
    /// Calls when a hitbox hits objects with IWorldHittable
    /// </summary>
    /// <param name="allegiance">The allegiance of the object that hit the target</param>
    /// <param name="damage">The damage the target will receive in world or battle</param>
    /// <param name="hitter">The object that hit the target</param>
    public void OnWorldHit(Allegiance allegiance, int damage, GameObject hitter);
}
