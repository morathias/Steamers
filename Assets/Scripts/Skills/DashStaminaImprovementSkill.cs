using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStaminaImprovementSkill : Skill {

    protected override void addSkillToPlayer()
    {
        _player.consumoDash = 20f;
    }
}
