using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusHealthSkill : Skill {

    protected override void addSkillToPlayer()
    {
        _playerStats.health += _playerStats.healthPool * 0.1f;
        _playerStats.VidaActual += _playerStats.healthPool * 0.1f;
        _playerStats._porcentajeActualBarraVida += 0.127f;
    }
}
