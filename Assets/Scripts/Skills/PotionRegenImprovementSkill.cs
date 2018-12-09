using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionRegenImprovementSkill : Skill {

    protected override void addSkillToPlayer()
    {
        _playerStats.regen = 0.5f;
    }
}
