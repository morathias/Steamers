using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageSkill : Skill {

    protected override void addSkillToPlayer()
    {
        _playerStats.RageOn = true;
        _playerStats.rageText.enabled = true;
    }
}
