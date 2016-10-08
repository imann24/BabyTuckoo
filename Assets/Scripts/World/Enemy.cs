/*
 * Author(s): Isaiah Mann
 * Description: Represents an enemy AI agent in the world
 */

using UnityEngine;
using System.Collections;

public class Enemy : AIAgent {
    public static Enemy Instance;

    protected override void SetReferences()
    {
        if (SingletonUtil.TryInit(ref Instance, this, gameObject))
        {
            base.SetReferences();
        }
    }

    protected override void CleanupReferences()
    {
        base.CleanupReferences();
        SingletonUtil.TryCleanupSingleton(ref Instance, this);
    }
}
