using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState
{
    IDLE,
    SNEAKING,
    JOGGING,
    RUNNING,
    JUMPING,
    FALLING,
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    //Toutes les variables accessibles dans l'inspector
    #region Exposed
    [SerializeField] private float _movespeed = 10f;

    [SerializeField] private float _turnSpeed = 10f;

    [SerializeField] private float _jumpForce = 10f;
    #endregion

    #region Unity Life Cycle
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        TransitionToState(PlayerState.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();
        
    }

    private void FixedUpdate()
    {
        if(_isJumping)
        {
            _direction.y = _jumpForce;
            _isJumping = false;
        }
        else
        {

            _direction.y = _rigidbody.velocity.y;
        }

        RotateTowardsCamera();
        _rigidbody.velocity = _direction;
    }
    #endregion
    //Toutes les fonctions créées par l'équipe
    #region Main Methods

    private void OnStateEnter()
    {
        switch (_currentState)
        {
            case PlayerState.IDLE:
                if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                break;
            case PlayerState.SNEAKING:
                break;
            case PlayerState.JOGGING:
                break;
            case PlayerState.RUNNING:
                break;
            case PlayerState.JUMPING:
                _isJumping = true;
                break;
            case PlayerState.FALLING:
                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (_currentState)
        {
            case PlayerState.IDLE:
                Move();

                if(_direction.magnitude > 0)
                {
                    TransitionToState(PlayerState.JOGGING);
                }
                else if  (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                break;
            case PlayerState.SNEAKING:
                Move();
                if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                break;
            case PlayerState.JOGGING:
                Move();

                if(_direction.magnitude == 0)
                {
                    TransitionToState(PlayerState.IDLE);
                }
                else if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                else if (_rigidbody.velocity.y > 0)
                {
                    TransitionToState(PlayerState.FALLING);
                }
                break;
            case PlayerState.RUNNING:
                if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                else if (_rigidbody.velocity.y > 0)
                {
                    TransitionToState(PlayerState.FALLING);
                }
                break;
            case PlayerState.JUMPING:
                Move();

                if(_rigidbody.velocity.y > 0)
                {
                    TransitionToState(PlayerState.FALLING);
                }
                break;
            case PlayerState.FALLING:
                break;
            default:
                break;
        }
    }
    private void OnStateExit()
    {
        switch (_currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.SNEAKING:
                break;
            case PlayerState.JOGGING:
                break;
            case PlayerState.RUNNING:
                break;
            case PlayerState.JUMPING:
                break;
            case PlayerState.FALLING:
                break;
            default:
                break;
        }
    }

    private void TransitionToState(PlayerState ToState)
    {
        OnStateExit();
        _currentState = ToState;
        OnStateEnter();
    }
    private void Move()
    {
        _direction = (_cameraTransform.forward * Input.GetAxis("Vertical") + _cameraTransform.right * Input.GetAxis("Horizontal")) * _movespeed;
        // _direction *= _movespeed;

        /* float horizontalInput = Input.GetAxisRaw("Horizontal");
         float forwardInput = Input.GetAxisRaw("Vertical");
         transform.Translate(Vector3.forward * Time.deltaTime * _movespeed * forwardInput);
         transform.Translate(Vector3.right * Time.deltaTime * _movespeed * horizontalInput);*/
    }

    private void RotateTowardsCamera()
    {
        Vector3 cameraForward = _cameraTransform.forward;
        cameraForward.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(cameraForward);
        //pour un mvt plus fluide
        Quaternion rotation = Quaternion.RotateTowards(_rigidbody.rotation, lookRotation, _turnSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(rotation);
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 80, 20), _currentState.ToString()))
        {
            TransitionToState(PlayerState.IDLE);
        }
    }
    #endregion

    //Les variables privées et protégées
    #region Private & Protected
    private Vector3 _direction = new Vector3();
    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private PlayerState _currentState;
    private bool _isJumping = false;
    #endregion
}
