using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//===============================================================================================
public class BossFlamethrower : MonoBehaviour {
    private enum States {
        Iddle,
        Raging,
        Moving,
        Attacking,
        Dying,
    }
    private States _state = States.Moving;

    private enum Attacks {
        Dashing,
        FlameBarrier,
        FlameThrow,
        AttacksCount
    }

    public float moveSpeed = 5f;
    public float steerSpeed = 1f;
    public float life = 1000f;
    private float _currentLife;

    private Transform _playerTransform;
    private float _directionChange = 1f;
    private float _directionChangeTimer = 0f;
    private float _specialAttackTimer = 8f;

    private const float RAGE_MODE_DEACTIVATED = 1f;
    private const float RAGE_MODE_ACTIVATED = 0.25f;
    private float _rageMode = RAGE_MODE_DEACTIVATED;

    private Vector3 _prevForward;
    private Vector3 _playerPosToDash;

    private Vector3 _vectorToRotateDash;

    private delegate bool attackAction();

    private Queue<attackAction> _actions = new Queue<attackAction>();

    private Rigidbody _rigidBody;
    //----------------------------------------------------------------------------------
	void Start () {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _directionChangeTimer = Random.Range(5f, 15f);

        _rigidBody = GetComponent<Rigidbody>();

        _currentLife = life;
	}
    //----------------------------------------------------------------------------------
    void Update () {
        switch (_state)
        {
            case States.Iddle:
                break;

            case States.Raging:
                _rageMode = RAGE_MODE_ACTIVATED;
                _state = States.Moving;
                break;

            case States.Moving:
                if (_currentLife / life <= 0.3f && _rageMode == RAGE_MODE_DEACTIVATED) {
                    _state = States.Raging;
                    break;
                }

                if (shouldChangeDirection())
                    changeDirection();

                lookAtPosition(_playerTransform.position);
                shooting();

                if (shouldDoSpecialAttack())
                {
                    Debug.Log("start attack");
                    //startSpecialAttack((Attacks)Random.Range(0, (int)Attacks.AttacksCount));
                    startSpecialAttack(Attacks.Dashing);
                    _state = States.Attacking;
                    _prevForward = transform.forward;
                }
                break;

            case States.Attacking:
                if (_actions.Count > 0){
                    if (_actions.Peek().Invoke()){
                        _actions.Dequeue();
                        Debug.Log("current actions: " + _actions.Count);
                    }
                }

                if (_actions.Count == 0){
                    Debug.Log("moving");
                    _state = States.Moving;
                }
                break;

            default:
                break;
        }

        if(_currentLife <= 0){
            _state = States.Dying;
        }
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

        float damage = 10f;
        applyDamage(damage);
    }
    //----------------------------------------------------------------------------------
    private void applyDamage(float damage){
        _currentLife -= damage;
    }

    private void startSpecialAttack(Attacks attack) {
        switch (attack)
        {
            case Attacks.Dashing:
                _actions.Enqueue(dashActionRotate);
                _actions.Enqueue(dashActionDash);
                _actions.Enqueue(dashActionWaitingToAttack);
                _actions.Enqueue(dashActionAttack);
                break;
            case Attacks.FlameBarrier:
                break;
            case Attacks.FlameThrow:
                break;
            default:
                break;
        }
    }
    //----------------------------------------------------------------------------------
    float rotatingTimer = 1.2f;
    private bool dashActionRotate() {
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -_prevForward, steerSpeed * Time.deltaTime, 0f));
        rotatingTimer -= Time.deltaTime;
        if (rotatingTimer <= 0)
        {
            rotatingTimer = 1.2f;
            return true;
        }

        return false;
    }

    private bool dashActionDash() {
        Vector3 playerDir = _playerTransform.position - transform.position;
        _playerPosToDash = _playerTransform.position;

        if (_rigidBody.IsSleeping())
            _rigidBody.WakeUp();
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
            return true;
        }

        return false;
    }

    private bool dashActionAttack() {
        return true;
    }
    //----------------------------------------------------------------------------------
}
//===============================================================================================
