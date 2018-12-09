using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeOnKillSkill : Skill {

    protected override void addSkillToPlayer(){
        _playerStats.onKilling = true;
    }
}
