using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineral : Item {

	void Start () {
        base.Start();
	}

    protected override void itemAgarrado()
    {
        Destroy(gameObject);
    }
}
