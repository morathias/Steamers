using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//===============================================================================================
public class BossFlamethrower : MonoBehaviour {
    private enum States {
        Iddle,
        Raging,
        Moving,
        Attacking,
        Dying,
    }
    private States _state = States.Iddle;

    private enum Attacks {
        Dashing,
        FlameThrow,
        FlameBarrier,
        FireRain,
        AttacksCount
    }

    public float moveSpeed = 5f;
    public float steerSpeed = 1f;
    public float life = 1000f;
    private float _currentLife;
    public int damage = 10;

    private Transform _playerTransform;
    private Vector3 _previousPlayerPosition = Vector3.zero;

    private float _directionChange = 1f;
    private float _directionChangeTimer = 0f;
    private float _specialAttackTimer = 8f;
    public float bulletEmissionRatePerSec = 4f;
    private float _shootTime = 0f;
    private Transform[] _flameThrowers = new Transform[2];
    private const int RIGHT_FLAME_THROWER = 0;
    private const int LEFT_FLAME_THROWER = 1;
    private int _currentFlamethrower = RIGHT_FLAME_THROWER;

    private const float RAGE_MODE_DEACTIVATED = 1f;
    private const float RAGE_MODE_ACTIVATED = 0.25f;
    private float _rageMode = RAGE_MODE_DEACTIVATED;
    [Range(0f, 1f)]
    public float rageThreshold = 0.5f;

    private Vector3 _playerPosToDash;

    private Vector3 _vectorToRotateDash;

    private delegate bool attackAction();

    private Queue<attackAction> _actions = new Queue<attackAction>();

    private Rigidbody _rigidBody;

    private Transform _basicShootingParticlesTransform;
    private ParticleSystem _basicShotingPaticleSystem;

    private GameObject _explotionColliderObject;
    private ParticleSystem _explotionParticleSystem;

    private float _chargingFlamethrowerTimer = 2f;
    private float _throwingFlamesTimer = 1.5f;
    private GameObject _flamethrowerFlames;
    public int flamesDamage = 5;

    private Vector3 _rightVector = Vector3.zero;
    public float barrierAngle = 52f;

    private Vector3 _upVector = Vector3.zero;
    private GameObject _fireRainFlamesObject;
    private float _fireRainTimer = 8f;

    GameObject _bossLifeBar;
    private Image _lifeBarImage;

    private ParticleSystem _blood;

    private Animator _animator;
    private const string RUNNING_TRIGGER = "Running";
    private const string SHOOTING_TRIGGER = "Shooting";
    private const string TARGETING_TRIGGER = "Targeting";
    private const string JUMPING_TRIGGER = "Jumping";

    public float distanceToFight = 50f;

    public AudioSource theme;
    //----------------------------------------------------------------------------------
	void Start () {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _directionChangeTimer = Random.Range(5f, 15f);

        _rigidBody = GetComponent<Rigidbody>();

        _currentLife = life;

        _shootTime = Time.time;

        _basicShootingParticlesTransform = transform.Find("fireBall");
        _basicShootingParticlesTransform.gameObject.GetComponent<DañoBalas>().setDaño(damage);
        _basicShotingPaticleSystem = _basicShootingParticlesTransform.gameObject.GetComponent<ParticleSystem>();
        _flameThrowers[0] = transform.Find("flameThrower_right");
        _flameThrowers[1] = transform.Find("flameThrower_left");

        _explotionColliderObject = transform.Find("explotionCollider").gameObject;
        _explotionParticleSystem = transform.Find("explotion").gameObject.GetComponent<ParticleSystem>();

        _flamethrowerFlames = transform.Find("flamethrowerFlames").gameObject;
        _flamethrowerFlames.transform.Find("collisionFlames").GetComponent<DañoBalas>().setDaño(flamesDamage);
        _flamethrowerFlames.transform.Find("collisionFlamesLeft").GetComponent<DañoBalas>().setDaño(flamesDamage / 2);
        _flamethrowerFlames.transform.Find("collisionFlamesRight").GetComponent<DañoBalas>().setDaño(flamesDamage / 2);

        _fireRainFlamesObject = transform.Find("fireRainBall").gameObject;
        _fireRainFlamesObject.GetComponent<DañoBalas>().setDaño(flamesDamage);

        _blood = transform.Find("blood_splat").GetComponent<ParticleSystem>();

        _animator = transform.Find("steampunk_boss_animado").GetComponent<Animator>();
    }
    //----------------------------------------------------------------------------------
    void Update () {
        switch (_state)
        {
            case States.Iddle:
                _bossLifeBar = GameObject.Find("HUD").transform.Find("bossLifeBar").gameObject;

                _lifeBarImage = _bossLifeBar.transform.GetChild(1).GetComponent<Image>();

                if ((_playerTransform.position - transform.position).magnitude <= distanceToFight){
                    _state = States.Moving;
                    _animator.SetTrigger(RUNNING_TRIGGER);
                    theme.Play();
                    _bossLifeBar.SetActive(true);
                }
                break;

            case States.Raging:
                _rageMode = RAGE_MODE_ACTIVATED;
                _specialAttackTimer = Random.Range(4f * _rageMode, 8f * _rageMode);
                _throwingFlamesTimer = 1.5f * _rageMode;
                _state = States.Moving;
                _animator.SetTrigger(RUNNING_TRIGGER);
                break;

            case States.Moving:
                if (_currentLife / life <= rageThreshold && _rageMode == RAGE_MODE_DEACTIVATED) {
                    _state = States.Raging;
                    break;
                }

                if (shouldChangeDirection())
                    changeDirection();

                lookAtPosition(_playerTransform.position);
                shooting();

                if (shouldDoSpecialAttack())
                {
                    if(_rageMode == RAGE_MODE_DEACTIVATED)
                        startSpecialAttack((Attacks)Random.Range(0, (int)Attacks.AttacksCount) - 1);
                    else
                        startSpecialAttack((Attacks)Random.Range(0, (int)Attacks.AttacksCount));
                   //startSpecialAttack(Attacks.FireRain);
                    _state = States.Attacking;
                }
                break;

            case States.Attacking:
                if (_actions.Count > 0){
                    if (_actions.Peek().Invoke()){
                        _actions.Dequeue();
                    }
                }

                if (_actions.Count == 0){
                    _state = States.Moving;
                    _animator.SetTrigger(RUNNING_TRIGGER);
                }
                break;
            case States.Dying:
                _bossLifeBar = GameObject.Find("HUD").transform.Find("bossLifeBar").gameObject;
                _bossLifeBar.SetActive(false);
                resetFlames();
                StartCoroutine(startFadingOut());
                break;
            default:
                break;
        }

        if(_currentLife <= 0){
            _state = States.Dying;
        }

        if (_fireRainFlamesObject.activeInHierarchy) {
            _fireRainTimer -= Time.deltaTime;
            if (_fireRainTimer <= 0){
                _fireRainFlamesObject.SetActive(false);
                _fireRainTimer = 8f;
            }
        }

        _previousPlayerPosition = _playerTransform.position;

        calculateEmmisionRateTime();
    }
    //----------------------------------------------------------------------------------
    IEnumerator startFadingOut(){
        float timeStarted = Time.realtimeSinceStartup;

        while (theme.volume > 0f){
            float timeSinceStarted = Time.realtimeSinceStartup - timeStarted;
            float percentage = timeSinceStarted / 3f;

            theme.volume = Mathf.Lerp(0.1f, 0f, percentage);
            yield return null;
        }

        theme.Stop();
        //Esto es alta negrada, hay que cambiarla mas adelante
        Destroy(gameObject);
    }
    //----------------------------------------------------------------------------------
    private bool shouldDoSpecialAttack(){
        _specialAttackTimer -= Time.deltaTime;

        if (_specialAttackTimer <= 0) {
            _specialAttackTimer = Random.Range(4f * _rageMode, 8f * _rageMode);
            return true;
        }

        return false;
    }
    //----------------------------------------------------------------------------------
    private void shooting(){
        if (Time.time < _shootTime)
            return;

        _basicShootingParticlesTransform.position = _flameThrowers[_currentFlamethrower].position;
        _basicShootingParticlesTransform.LookAt(_playerTransform);

        if (_currentFlamethrower == LEFT_FLAME_THROWER)
            _currentFlamethrower = RIGHT_FLAME_THROWER;
        else if (_currentFlamethrower == RIGHT_FLAME_THROWER)
            _currentFlamethrower = LEFT_FLAME_THROWER;

        _basicShotingPaticleSystem.Emit(1);
    }
    //----------------------------------------------------------------------------------
    void FixedUpdate() {
        switch (_state)
        {
            case States.Moving:
                move();
                break;
            default:
                break;
        }
    }
    //----------------------------------------------------------------------------------
    void lookAtPosition(Vector3 target) {
        Quaternion lookDirection = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, steerSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
    //----------------------------------------------------------------------------------
    void calculateEmmisionRateTime()
    {
        if (Time.time > _shootTime)
            _shootTime = Time.time + (1f / bulletEmissionRatePerSec) - (Time.time - _shootTime);
    }
    //----------------------------------------------------------------------------------
    void move() {
        Vector3 direction;

        Vector3 playerDir = _playerTransform.position - transform.position;
        float distancePlayerBoss = playerDir.magnitude;

        direction = transform.right * _directionChange;
        if (distancePlayerBoss > 15f)
            direction += (transform.forward + new Vector3(transform.forward.x * 0.25f, 0f, transform.forward.z * 0.25f)) ;
        if (distancePlayerBoss < 10f)
            direction += (-transform.forward + new Vector3(transform.forward.x * 0.25f, 0f, transform.forward.z * 0.25f));

        _rigidBody.AddForce(direction.normalized * moveSpeed, ForceMode.Force);
    }
    //----------------------------------------------------------------------------------
    bool shouldChangeDirection() {
        _directionChangeTimer -= Time.deltaTime;
        return _directionChangeTimer <= 0;
    }
    //----------------------------------------------------------------------------------
    void changeDirection() {
        _directionChangeTimer = Random.Range(5f, 15f);
        _directionChange = -_directionChange;
    }
    //----------------------------------------------------------------------------------
    void OnParticleCollision(GameObject other){
        if (other.tag != "BalaPlayer")
            return;

        if (_state == States.Iddle) {
            _state = States.Moving;
            _animator.SetTrigger(RUNNING_TRIGGER);
            theme.Play();
            _bossLifeBar.SetActive(true);
        }

        applyDamage(other.GetComponent<DañoBalas>().getDaño());
        _blood.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.3f), Random.Range(1.8f, 2.5f));
        _blood.transform.LookAt(_playerTransform);
        _blood.Play(true);
    }
    //----------------------------------------------------------------------------------
    private void applyDamage(float damage){
        _currentLife -= damage;
        _lifeBarImage.fillAmount = _currentLife / life;
    }
    //----------------------------------------------------------------------------------
    private void startSpecialAttack(Attacks attack) {
        switch (attack)
        {
            case Attacks.Dashing:
                _actions.Enqueue(dashActionRotate);
                _actions.Enqueue(dashActionDash);
                _actions.Enqueue(dashActionWaitingToAttack);
                _actions.Enqueue(dashActionAttack);
                if (_rageMode == RAGE_MODE_ACTIVATED) {
                    _actions.Enqueue(dashActionRotate);
                    _actions.Enqueue(dashActionDash);
                    _actions.Enqueue(dashActionWaitingToAttack);
                    _actions.Enqueue(dashActionAttack);
                }
                break;
            case Attacks.FlameBarrier:
                _rightVector = transform.right;
                _actions.Enqueue(flameBarrierActionRightPosition);
                _actions.Enqueue(flameBarrierActionRightAttack);
                _actions.Enqueue(flameBarrierActionLeftPosition);
                _actions.Enqueue(flameBarrierActionLeftAttack);
                break;
            case Attacks.FlameThrow:
                for (int i = 0; i < 3; i++){
                    _actions.Enqueue(flameThrowActionCharge);
                    _actions.Enqueue(flameThrowActionAttack);
                    _actions.Enqueue(flameThrowActionDrawback);
                }
                if (_rageMode == RAGE_MODE_ACTIVATED) {
                    for (int i = 0; i < 3; i++)
                    {
                        _actions.Enqueue(flameThrowActionCharge);
                        _actions.Enqueue(flameThrowActionAttack);
                        _actions.Enqueue(flameThrowActionDrawback);
                    }
                }
                break;
            case Attacks.FireRain:
                _upVector = transform.up;
                _actions.Enqueue(fireRainActionLookUp);
                _actions.Enqueue(fireRainActionShoot);
                _actions.Enqueue(fireRainActionDrawBack);
                break;
            default:
                break;
        }
    }
    //----------------------------------------------------------------------------------
    float rotatingTimer = 1.2f;
    private bool dashActionRotate() {
        _animator.ResetTrigger(RUNNING_TRIGGER);
        _animator.SetBool(TARGETING_TRIGGER, true);
        Vector3 playerDir = _playerTransform.position - transform.position;
        playerDir.y = 0;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -playerDir.normalized, steerSpeed * Time.deltaTime, 0f));
        rotatingTimer -= Time.deltaTime;
        if (rotatingTimer <= 0)
        {
            rotatingTimer = 1.2f;
            return true;
        }

        return false;
    }

    private bool dashActionDash() {
        _animator.SetBool(JUMPING_TRIGGER, true);
        _animator.SetBool(TARGETING_TRIGGER, false);
        Vector3 playerDir = _playerTransform.position - transform.position;
        _playerPosToDash = _playerTransform.position;

        _rigidBody.velocity = Vector3.zero;
        _rigidBody.AddForce((playerDir + (transform.up * 2f)) * 300f, ForceMode.Impulse);

        return true;
    }

    private bool dashActionWaitingToAttack() {
        lookAtPosition(_playerPosToDash);
        float distance = (_playerPosToDash - transform.position).magnitude;

        if (_rigidBody.IsSleeping())
            _rigidBody.WakeUp();

        if (distance <= 3f || _rigidBody.velocity.magnitude <= 0.1f){
            Vector3 playerDir = _playerTransform.position - transform.position;
            _rigidBody.Sleep();
            _rigidBody.AddForce(-transform.forward * 30f, ForceMode.Impulse);
            _explotionColliderObject.SetActive(true);
            _explotionParticleSystem.Play();
            _animator.SetBool(JUMPING_TRIGGER, false);
            return true;
        }

        return false;
    }

    float attackingTimer = 1f;
    private bool dashActionAttack() {
        attackingTimer -= Time.deltaTime;
        if (attackingTimer <= 0)
        {
            attackingTimer = 1f * _rageMode;
            _explotionColliderObject.SetActive(false);
            _animator.SetTrigger(RUNNING_TRIGGER);
            return true;
        }

        return false;
    }
    //----------------------------------------------------------------------------------
    private bool flameThrowActionCharge() {
        Vector3 predictionDir = _playerTransform.position - _previousPlayerPosition;
        lookAtPosition(predictionDir.normalized * 5f + _playerTransform.position);
        _animator.SetBool(TARGETING_TRIGGER, true);

        _chargingFlamethrowerTimer -= Time.deltaTime;
        if (_chargingFlamethrowerTimer <= 0) {
            _chargingFlamethrowerTimer = 2f * _rageMode;
            return true;
        }
        return false;
    }

    private bool flameThrowActionAttack() {
        _flamethrowerFlames.SetActive(true);

        _throwingFlamesTimer -= Time.deltaTime;
        if (_throwingFlamesTimer <= 0) {
            _throwingFlamesTimer = 1.5f * _rageMode;

            for (int j = 0; j < _flamethrowerFlames.transform.childCount; j++){
                Transform flamesTransform = _flamethrowerFlames.transform.GetChild(j);
                for (int i = 0; i < flamesTransform.childCount; i++)
                    flamesTransform.GetChild(i).GetComponent<ParticleSystem>().Stop();
            }
            
            return true;
        }
        return false;
    }

    float drawBackTimer = 0.5f;
    private bool flameThrowActionDrawback() {
        drawBackTimer -= Time.deltaTime;
        if (drawBackTimer <= 0) {
            drawBackTimer = 0.5f;
            _flamethrowerFlames.SetActive(false);
            _animator.SetBool(TARGETING_TRIGGER, false);
            return true;
        }
        return false;
    }
    //----------------------------------------------------------------------------------
    private float _barrierTimer = 1f;
    private bool flameBarrierActionRightPosition() {
        _animator.SetBool(TARGETING_TRIGGER, true);
        transform.rotation = transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, _rightVector, steerSpeed * Time.deltaTime, 0f));
        _barrierTimer -= Time.deltaTime;
        if (_barrierTimer <= 0) {
            _barrierTimer = 1f;

            _flamethrowerFlames.SetActive(true);
            _flamethrowerFlames.transform.GetChild(LEFT_FLAME_THROWER).gameObject.SetActive(false);
            _flamethrowerFlames.transform.Find("collisionFlames").gameObject.SetActive(false);
            _flamethrowerFlames.transform.Find("collisionFlamesRight").gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    private float _barrierAttackTimer = 3f;
    private bool flameBarrierActionRightAttack() {
        _barrierAttackTimer -= Time.deltaTime;
        transform.Rotate(transform.up * Time.deltaTime * -barrierAngle);
        if (_barrierAttackTimer <= 0) {
            _barrierAttackTimer = 3f;
            _rightVector = transform.right;
            resetFlames();
            return true;
        }

        return false;
    }

    private bool flameBarrierActionLeftPosition()
    {
        transform.rotation = transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -_rightVector, steerSpeed * Time.deltaTime, 0f));
        _barrierTimer -= Time.deltaTime;
        if (_barrierTimer <= 0)
        {
            _barrierTimer = 1f;

            _flamethrowerFlames.SetActive(true);
            _flamethrowerFlames.transform.GetChild(RIGHT_FLAME_THROWER).gameObject.SetActive(false);
            _flamethrowerFlames.transform.Find("collisionFlames").gameObject.SetActive(false);
            _flamethrowerFlames.transform.Find("collisionFlamesLeft").gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    private bool flameBarrierActionLeftAttack()
    {
        _barrierAttackTimer -= Time.deltaTime;
        transform.Rotate(transform.up * Time.deltaTime * barrierAngle);
        if (_barrierAttackTimer <= 0)
        {
            _barrierAttackTimer = 3f;
            resetFlames();

            _animator.SetBool(TARGETING_TRIGGER, false);
            return true;
        }

        return false;
    }
    //----------------------------------------------------------------------------------
    float lookUpTimer = 1f;
    private bool fireRainActionLookUp() {
        _basicShootingParticlesTransform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(_basicShootingParticlesTransform.forward, _upVector, steerSpeed * Time.deltaTime, 0f));

        lookUpTimer -= Time.deltaTime;
        if (lookUpTimer <= 0) {
            lookUpTimer = 1f;
            return true;
        }
        return false;
    }

    float shootRateTime = 0.0625f;
    private void shootFireRain() {
        _animator.SetBool(SHOOTING_TRIGGER, true);

        shootRateTime -= Time.deltaTime;
        if (shootRateTime > 0)
        {
            return;
        }
        shootRateTime = 0.125f;

        _basicShootingParticlesTransform.position = _flameThrowers[_currentFlamethrower].position;

        if (_currentFlamethrower == LEFT_FLAME_THROWER)
            _currentFlamethrower = RIGHT_FLAME_THROWER;
        else if (_currentFlamethrower == RIGHT_FLAME_THROWER)
            _currentFlamethrower = LEFT_FLAME_THROWER;

        _basicShotingPaticleSystem.Emit(1);
    }

    float fireRainShootTimer = 5f;
    private bool fireRainActionShoot() {
        shootFireRain();

        fireRainShootTimer -= Time.deltaTime;

        if (fireRainShootTimer <= 2.5f)
            _fireRainFlamesObject.SetActive(true);

        if (fireRainShootTimer <= 0) {
            fireRainShootTimer = 5f;
            return true;
        }
        return false;
    }

    float fireRainDrawbackTimer = 1f;
    private bool fireRainActionDrawBack() {
        _animator.SetBool(SHOOTING_TRIGGER, false);
        fireRainDrawbackTimer -= Time.deltaTime;
        if (fireRainDrawbackTimer <= 0) {
            fireRainDrawbackTimer = 1f;
            return true;
        }
        return false;
    }
    //----------------------------------------------------------------------------------
    private void resetFlames() {
        for (int i = 0; i < _flamethrowerFlames.transform.childCount; i++){
            _flamethrowerFlames.transform.GetChild(i).gameObject.SetActive(true);
        }

        _flamethrowerFlames.transform.Find("collisionFlamesLeft").gameObject.SetActive(false);
        _flamethrowerFlames.transform.Find("collisionFlamesRight").gameObject.SetActive(false);
        _flamethrowerFlames.SetActive(false);
    }
}
//===============================================================================================
