using UnityEngine;


public class Overlord : MonoBehaviour
{
    protected UnityEngine.AI.NavMeshAgent navigator;
    protected Vector3 playerTf;
    public EnemyHealth _stats;
    protected State _estado;
    public Pattern _pattern;
    protected Event _event;
    protected int positionFormation;
    public int daño;
    protected Quaternion neededRotation;
    protected Rigidbody _rigidBody;
    protected ParticleSystem _balaE;
    protected ESMachine stateMachine;
    public int id;
    protected Vector3 neededPos;
    protected GameObject pointMan;
    protected Collider _collider;
    protected Animator _animations;
    public bool onlyOnce = false;
    protected Stats playerStats;
    protected virtual void Start()
    {
        _estado = State.NORMAL;
        _stats = GetComponent<EnemyHealth>();
        stateMachine = GetComponent<ESMachine>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        stateMachine.init((int)State.countState, (int)Events.countEvents); //Castear de nombre a int
        navigator.isStopped = true;

        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        _estado = (State)stateMachine.getState(); //pasar de int a numero con cast

        stateMachine.relation((int)State.NORMAL, (int)Events.findGratmos, (int)State.AGGRESIVE);
        stateMachine.relation((int)State.AGGRESIVE, (int)Events.loseGratmos, (int)State.NORMAL);

        stateMachine.relation((int)State.STUNNED, (int)Events.recover, (int)State.AGGRESIVE);
        stateMachine.relation((int)State.AGGRESIVE, (int)Events.blunted, (int)State.STUNNED);

        stateMachine.relation((int)State.NORMAL, (int)Events.capIsHere, (int)State.FORMATION);
        stateMachine.relation((int)State.AGGRESIVE, (int)Events.capIsHere, (int)State.FORMATION);
        stateMachine.relation((int)State.STUNNED, (int)Events.capIsHere, (int)State.FORMATION);
        stateMachine.relation((int)State.FORMATION, (int)Events.capIsOut, (int)State.AGGRESIVE);


        stateMachine.relation((int)State.NORMAL, (int)Events.dead, (int)State.DEAD);
        stateMachine.relation((int)State.AGGRESIVE, (int)Events.dead, (int)State.DEAD);

        //stateMachine.relation((int)State.NORMAL, (int)Events.blunted, (int)State.STUNNED);
        //stateMachine.relation((int)State.ALARM, (int)Events.blunted, (int)State.STUNNED);
        //stateMachine.relation((int)State.AGGRESIVE, (int)Events.blunted, (int)State.STUNNED);
        //stateMachine.relation((int)State.STUNNED, (int)Events.recover, (int)State.ALARM);

        _collider = GetComponent<Collider>();
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

        if (playerDistance(objective) < 30)
        {
            _estado = stateMachine.setEvent((int)Events.findGratmos);

            playerTf = objective;
            return true;
        }
        else 
        {
            _estado = stateMachine.setEvent((int)Events.loseGratmos);
            _pattern = Pattern.MOVING;
            navigator.isStopped = true;
            return false;
        }

    }

    public void reset()
    {
        stateMachine.setEvent((int)Events.loseGratmos);
        navigator.isStopped = true;
        _animations.SetBool("Running", false);
        _pattern = Pattern.MOVING;
    }

    public void setEvent(Events newEvent)
    {
        stateMachine.setEvent((int)newEvent);
        if ( _estado == State.FORMATION)
        {
            _animations.SetBool("Running", true);
        }
    }
    public virtual bool setPointMan(GameObject PM, int order)
    {
        return true;
    }

    public int status()
    {
        return _stats.inDanger();
    }
    protected virtual void Update()
    {
        if (_stats.isDead())
        {
            if (_estado == State.FORMATION)
            {
                disarm();
            }
            GetComponentInParent<Squad>().removeUnit(gameObject);
            this.navigator.isStopped = true;
            navigator.enabled = false;
            this.gameObject.SetActive(false);

        }
    }

    public State getState()
    {
        return _estado;
    }
    protected virtual void disarm()
    {

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