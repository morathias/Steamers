using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    enum Tutorials {
        MoveTutorial,
        ShootTutorial,
        ReloadTutorial,
        LevelUpTutorial,
        RotateCameraTutorial
    }

    public static bool hasSeenTutorial = false;

    private delegate bool showTutorial();
    private List<showTutorial> _tutorialSteps = new List<showTutorial>();

    private bool _shouldShowShootTutorial;

    public List<GameObject> _tutorialsObjects;

    private Transform _playerTransform;
    private Stats _playerStats;
    private Arma _gun;

	void Start () {
        _tutorialSteps.Add(showMoveTutorial);
        _tutorialSteps.Add(showShootTutorial);
        _tutorialSteps.Add(showReloadTutorial);
        _tutorialSteps.Add(showLevelUpTutorial);
        _tutorialSteps.Add(showRotateCameraTutorial);

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerStats = _playerTransform.GetComponent<Stats>();
        _gun = _playerTransform.GetChild(0).GetComponent<Arma>();
	}

	void Update () {
        for (int i = 0; i < _tutorialSteps.Count; i++){
            if (_tutorialSteps[i]()) {
                _tutorialSteps.Remove(_tutorialSteps[i]);
            }
        }

        if (_tutorialSteps.Count == 0) {
            hasSeenTutorial = true;
            Destroy(gameObject);
        }
	}

    private bool showMoveTutorial() {
        Time.timeScale = 0f;

        _tutorialsObjects[(int)Tutorials.MoveTutorial].SetActive(true);

        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            Time.timeScale = 1f;
            _tutorialsObjects[(int)Tutorials.MoveTutorial].SetActive(false);
            return true;
        }

        return false;
    }

    private bool showShootTutorial() {
        if (_playerTransform.position.z > 75f){
            Time.timeScale = 0f;
            _tutorialsObjects[(int)Tutorials.ShootTutorial].SetActive(true);
        }

        if (Input.GetButtonDown("Fire1") && _tutorialsObjects[(int)Tutorials.ShootTutorial].activeInHierarchy)
        {
            Time.timeScale = 1f;
            _gun.disparar();
            _tutorialsObjects[(int)Tutorials.ShootTutorial].SetActive(false);
            return true;
        }
        return false;
    }

    private bool showReloadTutorial() {
        if (_gun.isReloading()){
            Time.timeScale = 0f;
            _tutorialsObjects[(int)Tutorials.ReloadTutorial].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R) && _tutorialsObjects[(int)Tutorials.ReloadTutorial].activeInHierarchy)
        {
            Time.timeScale = 1f;
            _tutorialsObjects[(int)Tutorials.ReloadTutorial].SetActive(false);
            return true;
        }
        return false;
    }

    private bool showLevelUpTutorial() {
        if (_playerStats.stat > 0){
            Time.timeScale = 0f;
            _tutorialsObjects[(int)Tutorials.LevelUpTutorial].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.C) && _tutorialsObjects[(int)Tutorials.LevelUpTutorial].activeInHierarchy) {
            _tutorialsObjects[(int)Tutorials.LevelUpTutorial].SetActive(false);

            return true;
        }
        return false;
    }

    private bool showRotateCameraTutorial() {
        if (_playerTransform.position.z > 120f)
        {
            Time.timeScale = 0f;
            _tutorialsObjects[(int)Tutorials.RotateCameraTutorial].SetActive(true);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && _tutorialsObjects[(int)Tutorials.RotateCameraTutorial].activeInHierarchy)
        {
            Time.timeScale = 1f;
            _tutorialsObjects[(int)Tutorials.RotateCameraTutorial].SetActive(false);
            return true;
        }
        return false;
    }
}
