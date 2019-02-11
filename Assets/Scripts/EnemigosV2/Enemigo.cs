using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public Item potionsToDrop;
	public Item experienceToDrop;

	public GameObject pathContainer;

	// Use this for initialization
	protected void Start () {
		_playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		_blood = transform.Find ("blood_splat").GetComponent<ParticleSystem> ();

		_currentHealth = health;
	}
	
	// Update is called once per frame
	protected void Update () {
		switch (_state) {
		case States.Iddle:
			if(shouldAlarm())
				_state = States.Chasing;
			break;

		case States.Walking:
			if (shouldAlarm())
				_state = States.Chasing;

			if (moveToObjective ())
				_state = States.Iddle;
			break;

		case States.Chasing:
			chasing ();
			break;

		case States.Targeting:
			targeting ();
			break;

		case States.Shooting:
			Shooting();
			break;

		case States.Dying:
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
		return (_playerTransform.position - this.transform.position).magnitude < 10f;
	}

	protected virtual void OnParticleCollision(GameObject other){
		Debug.Log(_currentHealth);
		if (other.transform.tag == "BalaPlayer"){
			_currentHealth -= other.GetComponent<DañoBalas>().getDaño();
		}

		if (other.transform.tag == "FuegoPlayer"){
			_currentHealth -= other.GetComponent<flameDamage>().getDaño();
		}

		_blood.Emit(1);
		Debug.Log(_currentHealth);
		if (_currentHealth <= 0) {
			_state = States.Dying;
		}
	}
}
