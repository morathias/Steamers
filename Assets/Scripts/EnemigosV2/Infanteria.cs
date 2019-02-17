using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infanteria : Enemigo {
	
	Vector3 destination = Vector3.zero;

	public float dodgingDistance = 23f;

	int _randomDirection = 1;
	float changeDirectionTimer = 0f;

	ParticleSystem _bullet;

	float _targetTime = 1f;
	float _shootTime = 1.2f;

	public bool isProtected = false;

	protected override void Start(){
		base.Start();

		_bullet = transform.Find("BalaE").GetComponent<ParticleSystem>();
		_bullet.gameObject.GetComponent<DañoBalas> ().setDaño (damage);

		changeDirectionTimer = Random.Range (1f, 3f);
		int startRandomDir = Random.Range (0, 2);

		if (startRandomDir == 0) {
			_randomDirection = 1;
		} else {
			_randomDirection = -1;
		}

		_targetTime = Random.Range (3f, 6f);
	}

	protected override void chasing ()
	{
		_animations.Play ("Armature|running");

		lookAtPosition (_playerTransform.position);

		changeDirectionTimer -= Time.deltaTime;

		if (changeDirectionTimer <= 0) {
			changeDirectionTimer = Random.Range (1f, 3f);
			_randomDirection *= -1;
		}

		float distance = (_playerTransform.position - this.transform.position).magnitude;
		if (distance > dodgingDistance) {
			//_agent.destination = _playerTransform.position;
			_agent.velocity = transform.forward * chaseSpeed;
		}
		else if (distance < dodgingDistance / 2f) {
			//_agent.destination = transform.position + ((transform.forward + transform.right * _randomDirection).normalized * -10f);
			_agent.velocity = (transform.forward + transform.right * _randomDirection).normalized * -chaseSpeed;
		}
		else {
			//_agent.destination = transform.position + (transform.right * _randomDirection * 10);
			_agent.velocity = (transform.right * _randomDirection).normalized * chaseSpeed;
		}
			
		_targetTime -= Time.deltaTime;
		if (_targetTime <= 0) {
			_targetTime = Random.Range (3f, 6f);

			_state = States.Targeting;
		}
	}



	protected override void Shooting ()
	{
		_animations.Play ("Armature|shoot");

		_bullet.transform.LookAt (_playerTransform.position);
		_bullet.Emit (1);

		_state = States.Chasing;
	}

	protected override void targeting ()
	{
		_animations.Play ("Armature|apuntando");

		lookAtPosition (_playerTransform.position);

		_shootTime -= Time.deltaTime;
		if (_shootTime <= 0f) {
			_shootTime = 1.2f;

			_state = States.Shooting;
		}
	}

}
