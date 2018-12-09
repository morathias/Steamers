using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusStaminaSkill : Skill {

    protected override void addSkillToPlayer()
    {
        _player.staminaBase += 100 * 0.05f;
        _player.Stamina += 100 * 0.05f;
        _playerStats._porcentajeActualBarraStamina += 0.127f;
    }
}
