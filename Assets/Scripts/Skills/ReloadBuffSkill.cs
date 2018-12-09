using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBuffSkill : Skill {

    protected override void addSkillToPlayer()
    {
        _playerStats.buffReload = 2;
    }
}
