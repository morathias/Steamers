using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBulletsSkill : Skill {
    protected override void addSkillToPlayer()
    {
        _player.transform.Find("RevolverDuo").GetComponent<Arma>().balas += 16;
    }
}
