using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public enum States
    {
        Idle,
        Chasing,
        RangeAttack,
        Death
    }

    NavMeshAgent _agent;
    Animator _anim;
    public GameObject GunPoint;
    public GameObject Target;
    private States _state;
    public float Fov = 60;
    private Vector3 _playerLastSeen;
    private Quaternion _targetRotation;
    public GameObject Bullet;
    private AudioSource _audio;
    private SkinnedMeshRenderer _meshRenderer;
    private Color[] color;

    private float _shootDelay = 0.2f;

    private DateTime _lastShoot;
    public GameObject Spawn;
    public GameObject GController;


    // Use this for initialization
    void Start()
    {
        
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _state = States.Idle;
        Target = GameObject.FindGameObjectWithTag("Player");
        _audio = GetComponent<AudioSource>();
        _meshRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        color = new Color[_meshRenderer.materials.Length];
        for(int i = 0; i<_meshRenderer.materials.Length; i++)
            color[i] = _meshRenderer.materials[i].color;
        Debug.Log(color);
        Spawn = GameObject.Find("SpawnController");
        GController = GameObject.Find("GameController");
    }

    /// <summary>
    /// OnTriggerStay is called once per frame for every Collider other
    /// that is touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z));
        }
        else if (other.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }

    bool NaVisao()
    {
        var enemyDirection = Target.transform.position - transform.position;
        if (Vector3.Angle(enemyDirection, transform.forward) <= Fov)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, enemyDirection, out hit))
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    transform.LookAt(new Vector3(hit.transform.position.x, transform.position.y,
                        hit.transform.position.z));
                    _state = States.Chasing;
                    _playerLastSeen = Target.transform.position;
                    _agent.SetDestination(_playerLastSeen);
                    return true;
                }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case States.Idle:
            {
                if (NaVisao())
                {
                    _state = States.RangeAttack;
                }
                else
                {
                    _agent.SetDestination(transform.position + new Vector3(
                                              Random.Range(-5, 5),
                                              0,
                                              Random.Range(-5, 5)));
                    _state = States.Chasing;
                }
            }
                break;
            case States.RangeAttack:
            {
                if (_lastShoot.AddSeconds(_shootDelay) < DateTime.Now)
                {
                    transform.LookAt(new Vector3(Target.transform.position.x, transform.position.y,
                        Target.transform.position.z));
                    var bullet = Instantiate(Bullet, GunPoint.transform.position, transform.rotation);
                    var velocity = transform.forward * 50 +
                                   new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                    bullet.GetComponent<Rigidbody>().velocity = velocity;
                    _lastShoot = DateTime.Now;
                    _audio.Play();
                }
                if (!NaVisao())
                {
                    _state = States.Chasing;
                    _agent.SetDestination(_playerLastSeen);
                }
            }
                break;
            case States.Chasing:
            {
                if (NaVisao())
                {
                    _state = States.RangeAttack;
                }
                else if (_agent.remainingDistance <= 5)
                {
                    _state = States.Idle;
                }
            }
                break;
            case States.Death:
            {
                for (var i = 0; i < _meshRenderer.materials.Length; i++)
                {
                    color[i] = _meshRenderer.materials[i].color;
                    color[i].a = Mathf.Lerp(color[i].a, 0, Time.deltaTime * 3);
                    _meshRenderer.materials[i].color = color[i];
                }
                if (Math.Abs(color[0].a) < 0.01)
                {
                    Destroy(gameObject);
                }
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _anim.SetFloat("Speed", _agent.velocity.magnitude);
    }

    public void StartAutoDestroy()
    {
        _state = States.Death;
    }

    private void OnDestroy()
    {
        if(GController)
            GController.GetComponent<GameController>().IncrementScore();
        if(Spawn)
            Spawn.GetComponent<SpawnController>().EnemiesNumber--;
    }
}