/*
 * Author(s): Isaiah Mann
 * Description: Human player in the world
 */

using UnityEngine;
using System.Collections;

public class Player : Agent {
	public static Player Instance;

	protected override void SetReferences () {
		if (SingletonUtil.TryInit(ref Instance, this, gameObject)) {
			base.SetReferences ();	
		}
	}

	protected override void CleanupReferences () {
		base.CleanupReferences ();
		SingletonUtil.TryCleanupSingleton(ref Instance, this);
	}
}
