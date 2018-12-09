using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeMenu : MonoBehaviour {
    private Button _unlockButton;
    private Text _selectedSkillTitleTxt;
    private Text _selectedSkillDescriptionTxt;

	void Start () {
        _unlockButton = transform.Find("infoGral").Find("Viewport").GetChild(0).Find("UnlockBtn").GetComponent<Button>();
        _unlockButton.onClick.AddListener(onUnlockPressed);
        _unlockButton.gameObject.SetActive(false);

        _selectedSkillTitleTxt = transform.Find("infoGral").Find("Viewport").GetChild(0).Find("SkillTitleTxt").GetComponent<Text>();
        _selectedSkillTitleTxt.text = "";

        _selectedSkillDescriptionTxt = transform.Find("infoGral").Find("Viewport").GetChild(0).Find("SkillDescriptionTxt").GetComponent<Text>();
        _selectedSkillDescriptionTxt.text = "";
    }
	
	void Update() {
        Skill selectedSkill = SkillTreeManager.getInstance().getSelectedSkill();
        if (selectedSkill == null)
            return;

        _selectedSkillDescriptionTxt.text = selectedSkill.description;
        _selectedSkillTitleTxt.text = selectedSkill.title;

        if (selectedSkill.isUnlocked() || !selectedSkill.canUnlock())
            _unlockButton.gameObject.SetActive(false);
        else
            _unlockButton.gameObject.SetActive(true);
    }

    public void onUnlockPressed() {
        Skill selectedSkill = SkillTreeManager.getInstance().getSelectedSkill();

        selectedSkill.onUnlockPressed();
    }
}
