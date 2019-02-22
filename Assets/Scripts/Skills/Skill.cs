using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour{
    public GameObject skillUIPrefab;
    public GameObject gearsConnectorPrefab;

    private Animator _rotatingAnimator;
    private Animator _selectionAnimator;
    private Image _iconImg;
    private Transform _statesTransform;
    private Button _skillBtn;
    private List<Image> _connectionActiveImgs = new List<Image>();
    public Sprite iconSprite;

    public List<Skill> _childs;
    Skill _parent = null;

    protected Prota _player;
    protected Stats _playerStats;

    protected enum States {
        Locked,
        Available,
        Unlocked
    }
    States _state = States.Locked;

    protected int requiredLevel = 2;

    public string title = "";
    [TextArea]
    public string description = "";

    public float fillSpeed = 1f;

    void Awake()
    {
        for (int i = 0; i < _childs.Count; i++)
        {
            _childs[i].parent = this;
        }
    }

    protected virtual void Start() {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        _player = playerGameObject.GetComponent<Prota>();
        _playerStats = playerGameObject.GetComponent<Stats>();

        for (int i = 0; i < _childs.Count; i++)
        {
            GameObject gearsConnector = Instantiate(gearsConnectorPrefab, transform);
            RectTransform childTransform = _childs[i].GetComponent<RectTransform>();
            RectTransform thisTransform = GetComponent<RectTransform>();

            Vector2 childThisVec = thisTransform.localPosition  - childTransform.localPosition;
            float distance = (childThisVec.magnitude / 0.1f);
            float angle = Mathf.Atan2(-childThisVec.x, childThisVec.y) * Mathf.Rad2Deg;
            gearsConnector.transform.rotation = Quaternion.Euler(0, 0, angle);
            gearsConnector.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(50f, distance);
            gearsConnector.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(50f, distance);
            _connectionActiveImgs.Add(gearsConnector.transform.GetChild(1).GetComponent<Image>());
        }

        GameObject uiObject = Instantiate(skillUIPrefab, transform);
        _rotatingAnimator = uiObject.transform.Find("gear").GetComponent<Animator>();
        _iconImg = uiObject.transform.Find("icon").GetComponent<Image>();
        _iconImg.sprite = iconSprite;
        _statesTransform = uiObject.transform.Find("states");
        _skillBtn = uiObject.GetComponent<Button>();
        _selectionAnimator = uiObject.GetComponent<Animator>();
        _skillBtn.onClick.AddListener(onPressed);

        //the first skill should be available at startup
        if (!_parent)
            makeAvailable();
    }

    public void onPressed() {
        _selectionAnimator.SetBool("isSelected", true);
        SkillTreeManager.getInstance().setSelectedSkill(this);
    }

    public void onDeselect() {
        _selectionAnimator.SetBool("isSelected", false);
    }
    public void onUnlockPressed() {
        addSkillToPlayer();
        _playerStats.stat--;

        _state = States.Unlocked;
        _statesTransform.GetChild((int)States.Available).gameObject.SetActive(false);
        _statesTransform.GetChild((int)States.Unlocked).gameObject.SetActive(false);
        _rotatingAnimator.SetTrigger("isUnlocked");

        for (int i = 0; i < _connectionActiveImgs.Count; i++){
            _childs[i].makeAvailable();
            StartCoroutine(startFillingConnection(_connectionActiveImgs[i], _childs[i]));
        }
    }

    protected virtual void addSkillToPlayer() {}

    public Skill parent{
        set{
            _parent = value;
        }
    }

    IEnumerator startFillingConnection(Image connectionImg, Skill childToEnable) {
        float timeStarted = Time.realtimeSinceStartup;

        while (connectionImg.fillAmount < 1f)
        {
            float timeSinceStarted = Time.realtimeSinceStartup - timeStarted;
            float percentage = timeSinceStarted / fillSpeed;

            connectionImg.fillAmount = Mathf.Lerp(0f, 1f, percentage);

            yield return null;
        }

        childToEnable.makeAvailableView();
    }

    public bool isUnlocked() {
        return _state == States.Unlocked;
    }

    public bool canUnlock() {
        if (_parent == null) {
            return _playerStats.stat > 0;
        }
        return _playerStats.stat > 0 && _parent.isUnlocked();
    }

    public void makeAvailableView(){
        _statesTransform.GetChild((int)States.Unlocked).gameObject.SetActive(false);
    }
    public void makeAvailable() {
        _state = States.Available;
    }

    void OnEnable() {
        if (_state == States.Unlocked){
            _statesTransform.GetChild((int)States.Unlocked).gameObject.SetActive(false);
            _statesTransform.GetChild((int)States.Available).gameObject.SetActive(false);
            _rotatingAnimator.SetTrigger("isUnlocked");
            for (int i = 0; i < _connectionActiveImgs.Count; i++){
            _connectionActiveImgs[i].fillAmount = 1f;
        }
        }
        else if (_state == States.Available) {
            _statesTransform.GetChild((int)States.Unlocked).gameObject.SetActive(false);
        }
    }
    void OnDisable()
    {
        
    }
}
