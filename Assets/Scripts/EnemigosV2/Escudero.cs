using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudero : Enemigo {

	private Transform _allyToCover;

	public float shearchRadius = 10f;
	public float coverDistance = 3f;

	private bool _alliesNearby = true;

	public LayerMask layers;

	private float _chargeTimer = 10f;
	private float _targetTimer = 2f;
	private float _chargingTimer = 3f;
	private float _attackTimer = 2f;

	public float chargeSpeed = 5f;
	public float walkSpeed = 1f;

	private Collider _shieldCollider;

	protected override void Start(){
		base.Start();

		_shieldCollider = transform.Find("shieldTrigger").GetComponent<Collider>();
	}
	protected override void chasing ()
	{
		_animations.Play("Walking");

		if (_allyToCover == null && _alliesNearby) {
			_allyToCover = findAllyToCover ();
		}

		else if(_allyToCover != null){
			_agent.destination = _allyToCover.position + (_allyToCover.position - _playerTransform.position).normalized * -coverDistance;

			lookAtPosition (_playerTransform.position);
		}

		else if(!_alliesNearby && _allyToCover == null){
			lookAtPosition (_playerTransform.position);
			_agent.velocity = transform.forward * walkSpeed;
		}

		_chargeTimer -= Time.deltaTime;
		if(_chargeTimer <= 0){
			if(_alliesNearby)
				_chargeTimer = 15f;
			else
				_chargeTimer = 2f;

			_state = States.Targeting;
			_agent.Stop();
			_agent.ResetPath();
		}
	}

	protected override void Shooting ()
	{
		_attackTimer -= Time.deltaTime;
		if(_attackTimer <= 0){
			_attackTimer = 2f;
			_shieldCollider.enabled = false;
		}

		_chargingTimer -= Time.deltaTime;
		if(_chargingTimer <= 0f){
			_chargingTimer = 3f;

			_state = States.Chasing;
		}
	}

	protected override void targeting ()
	{
		lookAtPosition (_playerTransform.position);

		_targetTimer -= Time.deltaTime;
		if(_targetTimer <= 0){
			_targetTimer = 2f;

			_agent.velocity = transform.forward * chargeSpeed;
			_shieldCollider.enabled = true;
			_state = States.Shooting;
		}
	}

	private Transform findAllyToCover(){
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, shearchRadius, layers);
		if (hitColliders.Length == 2) {
			_alliesNearby = false;
			_chargeTimer = 2f;
			return null;
		}

		float minDistance = float.MaxValue;
		int index = 0;
		Infanteria ally;
		for (int i = 0; i < hitColliders.Length; i++) {
			ally = hitColliders [i].transform.GetComponent<Infanteria> ();

			if (ally == null || ally.isProtected) {
				continue;
			}

			float distance = (hitColliders [i].transform.position - transform.position).magnitude;
			if (distance < minDistance) {
				minDistance = distance;
				index = i;
			}
		}
		ally = hitColliders [index].transform.GetComponent<Infanteria> ();
		ally.isProtected = true;

		return hitColliders [index].transform;
	}

	void OnDrawGizmosSelected()
	{
		// Display the explosion radius when selected
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, shearchRadius);
	}

}
