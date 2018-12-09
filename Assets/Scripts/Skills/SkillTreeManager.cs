using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager {
    private static SkillTreeManager _instance;

    private Skill _selectedSkill;

    public static SkillTreeManager getInstance() {
        if (_instance == null)
            _instance = new SkillTreeManager();

        return _instance;
    }


    public void setSelectedSkill(Skill skill) {
        if(_selectedSkill != null && _selectedSkill != skill)
            _selectedSkill.onDeselect();

        _selectedSkill = skill;
    }

    public Skill getSelectedSkill() {
        return _selectedSkill;
    }
}
