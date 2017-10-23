using UnityEngine;
using System.Collections;

public class Pota : Item
{
    void Start()
    {
        base.Start();
    }

    protected override void itemAgarrado()
    {
        Stats levelComponent = Target.gameObject.GetComponent<Stats>();

        levelComponent.VidaActual += levelComponent.health * levelComponent.regen;
        if (levelComponent.VidaActual > levelComponent.health)
            levelComponent.VidaActual = levelComponent.health;

        Destroy(gameObject);
    }
}