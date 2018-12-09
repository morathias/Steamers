using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusDamageSkill : Skill {
    protected override void addSkillToPlayer()
    {
        _playerStats.damage += _playerStats.damageBase * 0.1f;
    }
}
