using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capitan : Enemigo {
    private Transform _formationContainer;
    public LayerMask layers;
    public float shearchRadius;

    Dictionary<int, Enemigo> _enemiesInFormation = new Dictionary<int, Enemigo>();

    private bool _startFormation = false;

    protected override void Start(){
        base.Start();

        _formationContainer = transform.Find("formationContainer");
    }

    protected override void chasing(){
        if(_formationContainer == null)
            return;

        if(!_startFormation){
            startFormation();
        }

        else{
            for (int i = 0; i < _enemiesInFormation.Count; i++){
                _enemiesInFormation[i].moveInFormation(_formationContainer.GetChild(i).position);
            }

            lookAtPosition(_playerTransform.position);
        }
    }

    private void startFormation(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, shearchRadius, layers);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            Enemigo enemyToOrder = hitColliders[i].gameObject.GetComponent<Enemigo>();
            if (enemyToOrder == null || enemyToOrder.gameObject == gameObject)
                continue;

            setInFormation(enemyToOrder);
            enemyToOrder.followOrders();
        }

        _startFormation = true;
    }

    void setInFormation(Enemigo enemyToOrder){
        for (int i = 0; i < _formationContainer.childCount; i++)
        {
            if(_formationContainer.GetChild(i).tag == enemyToOrder.gameObject.tag){
                if(!_enemiesInFormation.ContainsKey(i)){
                    _enemiesInFormation.Add(i, enemyToOrder);
                    return;
                }
            }
        }
    }

    protected override void Shooting(){

    }

    protected override void targeting(){

    }

    void OnDrawGizmosSelected()
	{
		// Display the explosion radius when selected
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, shearchRadius);
	}
}
