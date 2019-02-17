using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour {
	protected enum States
	{
		Iddle,
		Walking,
		Chasing,
		Targeting,
		Shooting,
		Dying
	}
	protected States _state = States.Iddle;

	protected Transform _playerTransform;
	protected Transform _objectiveToChase;

	public float patroleSpeed = 1f;
	public float chaseSpeed = 5f;

	public float rotateSpeed = 45f;
	private float _currentRotateSpeed = 0;

	public int health = 10;
	protected int _currentHealth = 0;

	private ParticleSystem _blood;

	public float alarmRadius = 10f;
	public float chasingRadius = 40f;

	public Item potionsToDrop;
	public Item experienceToDrop;

	public GameObject pathContainer;

	protected NavMeshAgent _agent;

	public int damage = 1;

	protected Animator _animations;

	// Use this for initialization
	protected virtual void Start () {
		_playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		_blood = transform.Find ("blood_splat").GetComponent<ParticleSystem> ();
		_agent = GetComponent<NavMeshAgent> ();

		_currentHealth = health;

		_animations = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		switch (_state) {
		case States.Iddle:
			if (shouldAlarm ()) {
				_state = States.Chasing;
			}
			break;

		case States.Walking:
			if (shouldAlarm())
				_state = States.Chasing;

			if (moveToObjective ())
				_state = States.Iddle;
			break;

		case States.Chasing:
			chasing ();

			if (isOutOfSight ()) {
				_state = States.Iddle;
			}
			break;

		case States.Targeting:
			targeting ();
			break;

		case States.Shooting:
			Shooting();
			break;

		case States.Dying:
			int random = Random.Range(0, 11);
			if(random == 0){
				Instantiate(potionsToDrop, transform.position, Quaternion.identity);
			}

			Instantiate(experienceToDrop, transform.position, Quaternion.identity);

			Destroy (gameObject);
			break;

		default:
			break;
		}
	}

	bool moveToObjective(){
		return true;
	}

	protected virtual void chasing(){
	}

	protected virtual void targeting(){
	}

	protected virtual void Shooting(){
	}

	bool shouldAlarm(){
		return (_playerTransform.position - this.transform.position).magnitude < alarmRadius;
	}

	bool isOutOfSight(){
		return (_playerTransform.position - this.transform.position).magnitude > chasingRadius;
	}

	protected virtual void OnParticleCollision(GameObject other){



		if (other.transform.tag == "BalaPlayer"){
			_currentHealth -= other.GetComponent<DañoBalas>().getDaño();

			if (_state == States.Iddle || _state == States.Walking)
				_state = States.Chasing;
			
			_blood.Emit(1);
		}

		if (other.transform.tag == "FuegoPlayer"){
			_currentHealth -= other.GetComponent<flameDamage>().getDaño();

			if (_state == States.Iddle || _state == States.Walking)
				_state = States.Chasing;
			_blood.Emit(1);
		}



		if (_currentHealth <= 0) {
			_state = States.Dying;
		}
	}

	protected void lookAtPosition(Vector3 target) {
		Quaternion lookDirection = Quaternion.LookRotation(target - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, rotateSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
	}
}
