using UnityEngine;


public class Overlord : MonoBehaviour
{
    protected UnityEngine.AI.NavMeshAgent navigator;
    protected Vector3 playerTf;
    protected EnemyHealth _stats;
    protected State _estado;
    protected Pattern _pattern;
    protected Event _event;
    protected int ordenPos;
    public int daño;
    protected Quaternion neededRotation;
    protected Rigidbody _rigidBody;
    protected ParticleSystem _balaE;
    protected ESMachine stateMachine;
    public int id;
    protected Vector3 neededPos;

    protected virtual void Start()
    {
        _estado = State.NORMAL;
        _stats = GetComponent<EnemyHealth>();
        stateMachine = GetComponent<ESMachine>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        stateMachine.init((int)State.countState, (int)Events.countEvents); //Castear de nombre a int
        navigator.isStopped = true;

        _estado = (State)stateMachine.getState(); //pasar de int a numero con cast

        stateMachine.relation((int)State.NORMAL, (int)Events.findGratmos, (int)State.AGGRESIVE);
        stateMachine.relation((int)State.AGGRESIVE, (int)Events.findGratmos, (int)State.AGGRESIVE);
        stateMachine.relation((int)State.STUNNED, (int)Events.recover, (int)State.AGGRESIVE);
        stateMachine.relation((int)State.AGGRESIVE, (int)Events.blunted, (int)State.STUNNED);
        // stateMachine.relation((int)State.ALARM, (int)Events.findGratmos, (int)State.AGGRESIVE);

        stateMachine.relation((int)State.NORMAL, (int)Events.dead, (int)State.DEAD);
        stateMachine.relation((int)State.AGGRESIVE, (int)Events.dead, (int)State.DEAD);

        //stateMachine.relation((int)State.NORMAL, (int)Events.blunted, (int)State.STUNNED);
        //stateMachine.relation((int)State.ALARM, (int)Events.blunted, (int)State.STUNNED);
        //stateMachine.relation((int)State.AGGRESIVE, (int)Events.blunted, (int)State.STUNNED);
        //stateMachine.relation((int)State.STUNNED, (int)Events.recover, (int)State.ALARM);
    }
    public float playerDistance(Vector3 objective)
    {

        return Vector3.Distance(transform.position, objective);
    }

    protected virtual Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public bool setDestination(Vector3 objective)
    {
        navigator.isStopped = false;

        if (playerDistance(objective) > 30)
        {
            _estado = stateMachine.setEvent((int)Events.loseGratmos);
            _pattern = Pattern.MOVING;
            navigator.isStopped = true;
            return false;
        }
        else
        {
            _estado = stateMachine.setEvent((int)Events.findGratmos);
            Debug.Log(stateMachine.getState());

            playerTf = objective;
            return true;
        }

    }

    public void reset()
    {
        stateMachine.setEvent((int)Events.loseGratmos);
    }

    public void fear()
    {
        // _estado = AttackPattern.FEAR;
    }

    //protected virtual void moveIt(float chaos)
    //{
    //    neededRotation = Quaternion.LookRotation(playerTf - transform.position);
    //    neededRotation *= Quaternion.Euler(0, Random.Range(90.0f, 270.0f), 0);
    //    neededRotation.x = 0;
    //    neededRotation.z = 0;
    //    transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 5f);

    //    neededPos.x = playerTf.x + Random.Range(10, 20.0f) * Random.Range(-1, 1);
    //    neededPos.z = playerTf.z + Random.Range(10, 20.0f) * Random.Range(-1, 1);
    //    navigator.SetDestination(neededPos);
    //    navigator.transform.position = transform.position;
    //    _rigidBody.velocity = navigator.desiredVelocity;
    //}

    public int status()
    {
        return _stats.inDanger();
    }
    protected virtual void Update()
    {
        if (_stats.isDead())
        {
            GetComponentInParent<Squad>().removeUnit(gameObject);
            this.navigator.isStopped = true;
            navigator.enabled = false;
            this.gameObject.SetActive(false);

        }
    }
    protected Vector3 setPosition(int order, Vector3 _posicionLider, Transform capitanPos)
    {
        ordenPos = order;
        switch (ordenPos)
        {
            case 0:
                _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 3);
                break;
            case 1:
                _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 3) - (capitanPos.right * 3);

                break;
            case 2:
                _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 3) - (capitanPos.right * -3);

                break;
            case 3:
                _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 6);

                break;
            case 4:
                _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 6) - (capitanPos.right * 3);

                break;
            case 5:
                _posicionLider = new Vector3(capitanPos.transform.position.x, capitanPos.transform.position.y, capitanPos.transform.position.z) - (capitanPos.forward * 6) - (capitanPos.right * -3);
                break;

        }
        return _posicionLider;
    }
}


//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class Overlord : MonoBehaviour
//{
//    protected GameObject objective;
//    protected Transform fichador;
//    protected Vector3 playerTF;
//    protected EnemyHealth _stats;
//    public estados _estado;
//    protected int ordenPos;
//    public int countdown = 6;
//    public int daño;
//    public string phase = "0";
//    public ParticleSystem blood;
//    protected UnityEngine.AI.NavMeshAgent navigator { get; private set; }
//    public int id;

//    protected virtual void Start()
//    {
//        _estado = estados.normal;
//        objective = GameObject.FindGameObjectWithTag("Player");
//        _stats = GetComponent<EnemyHealth>();
//        fichador = objective.transform;
//        playerTF = fichador.position;

//        navigator = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
//        navigator.isStopped = true;
//    }
//    public enum estados
//    {
//        normal,
//        bajoOrdenes,
//        recarga,
//        apuntando,
//        rage,
//        fear,
//        protect,
//        Durazno,
//        brag,
//    }
//    public bool setDestination(Vector3 objective)
//    {


//            navigator.isStopped = false;
//            navigator.destination = objective;
//            Debug.Log("holllaaa" + navigator.remainingDistance);
//            if (navigator.remainingDistance < 0.001f)
//            {

//                navigator.isStopped = true;
//                return false;
//            }
//            else
//            {

//                playerTF = objective;
//                return true;
//            }

//        return true;
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        if (countdown < 1)
//        {
//            phase = "1";
//        }
//    }

//    public void reset()
//    {
//        _estado = estados.normal;
//    }

//    public void fear()
//    {
//        _estado = estados.fear;
//    }

//    protected virtual void OnParticleCollision(GameObject other)
//    {
//        if (other.transform.tag == "BalaPlayer")
//        {
//            // _stats.applyDamage(10000);

//            _stats.applyDamage(other.GetComponent<DañoBalas>().getDaño());

//        }

//        if (other.transform.tag == "FuegoPlayer")
//        {
//            _stats.applyDamage(other.GetComponent<flameDamage>().getDaño());
//        }
//        blood.Emit(1);
//    }
//    public int status()
//    {
//        return _stats.inDanger();
//    }
//    public string bossSat()
//    {
//        return phase;
//    }
//}