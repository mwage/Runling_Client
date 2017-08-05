using System;
using System.Collections;
using Launcher;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Players
{
    public class PlayerMovement : TrueSyncBehaviour
    {
        #region Variables
        public GameObject MouseClick;

        private readonly FP _rotationSpeed = 30;
        private readonly FP _acceleration = 100;
        private readonly FP _deceleration = 100;
        private readonly FP _stopSensitivity = 20;   // Adjust for better accuracy at other decelerations

        public bool AutoClickerActive;
        public bool IsAutoClicking;

        private TSRigidBody _rb;
        private TSVector _targetPos, _clickPos, _currentPos;
        private TSVector _direction;
//        private TSVector _localGravity = new TSVector(0, -10, 0);
        private FP _targetRotation;
        private FP _maxSpeed, _currentSpeed, _highestSpeedReached;
        private FP _distance, _lastDistance;
        private int _distanceCounter;
        private bool _accelerate;
        private bool _stop = true;
        private int _defLayer = 1 << 15;            // Ground Layer for Raycasting;
        private Animator _anim;
        private Coroutine _autoClickRoutine;
        private bool _clicked;
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private PlayerManager _playerManager;


        private void Awake()
        {
            _rb = GetComponent<TSRigidBody>();
            _targetPos = _rb.tsTransform.position;
            _anim = GetComponent<Animator>();
            _playerManager = GetComponent<PlayerManager>();
        }
        #endregion

        #region UserInput
        private void Update()
        {

            // Start autoclicking
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateClicker))
            {
                if (!AutoClickerActive)
                    AutoClickerActive = true;
            }

            // Stop autoclicking
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactivateClicker))
            {
                if (AutoClickerActive)
                    AutoClickerActive = false;
            }


            if (AutoClickerActive)
            {
                if (!IsAutoClicking)
                {
                    _autoClickRoutine = StartCoroutine(DoAutoclick());
                    IsAutoClicking = true;
                }
            }


            if (!AutoClickerActive)
            {
                if (IsAutoClicking)
                {
                    StopCoroutine(_autoClickRoutine);
                    IsAutoClicking = false;
                }
            }


            // On right mouseclick, set new target location
            if (Input.GetMouseButtonDown(1))
            {
                _clicked = true;
            }

            // Control animation
            _anim.SetFloat(_speedHash, (float)_currentSpeed);
        }

        private IEnumerator DoAutoclick()
        {
            while (true)
            {
                _clicked = true;
                yield return new WaitForSeconds(0.05f);
            }
        }

        public override void OnSyncedInput()
        {
            if (_clicked)
            {
                TrueSyncInput.SetBool(GameControl.InputManager.MouseClick, true);
                _clicked = false;
            }
            else
            {
                TrueSyncInput.SetBool(GameControl.InputManager.MouseClick, false);
                return;
            }

            RaycastHit hit;
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _defLayer))
            {
                TrueSyncInput.SetTSVector(GameControl.InputManager.ClickPosition, hit.point);

            }
        }

        public override void OnSyncedUpdate()
        {
            // receive click pos from input queue
            if (TrueSyncInput.GetBool(GameControl.InputManager.MouseClick))
            {
                _clickPos = TrueSyncInput.GetTSVector(GameControl.InputManager.ClickPosition);
                ProcessInput();
            }

            MovetoPosition();
        }
        

        private void ProcessInput()
        {
        
            _maxSpeed = _playerManager.CharacterController.Speed.Current;

            if (TrueSyncManager.LocalPlayer == owner)
            {
                // Play click animation
                var click = Instantiate(MouseClick, new Vector3((float)_clickPos.x, (float)_clickPos.y, (float)_clickPos.z), Quaternion.Euler(0, 45, 0));

                if (_playerManager.IsImmobile)
                {
                    foreach (Transform child in click.transform)
                    {
                        child.GetComponent<Renderer>().material.color = Color.red;
                        foreach (Transform ch in child)
                        {
                            ch.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }
                }
            }


            if (!_playerManager.IsImmobile)
            {
                if ((_clickPos - _rb.tsTransform.position).magnitude < 0.05f) return;

                _targetPos = new TSVector(_clickPos.x, 0, _clickPos.z);
                _direction = (_targetPos - _currentPos).normalized;
                _targetRotation = Quaternion.LookRotation(new Vector3((float)_direction.x, 0, (float)_direction.z)).eulerAngles.y % 360;

                _targetRotation = _targetRotation > 0 ? _targetRotation : 360 + _targetRotation;
                _accelerate = true;
                _stop = false;

                if ((_targetPos - _rb.tsTransform.position).magnitude < 0.5f)
                {
                    _rb.velocity = _direction * _currentSpeed / 2;
                }
                else
                {
                    _rb.velocity = _direction * _currentSpeed;
                }

                _highestSpeedReached = _rb.velocity.magnitude;
                _lastDistance = Mathf.Infinity;
                _distanceCounter = 3;
            }
        }
        #endregion


        #region Physics
        public void MovetoPosition()
        {
            if (_playerManager.CharacterController != null)
            {
                _maxSpeed = _playerManager.CharacterController.Speed.Current;
            }
            _currentPos = new TSVector(_rb.tsTransform.position.x, 0, _rb.tsTransform.position.z);
            _currentSpeed = _rb.velocity.magnitude;

            if (_playerManager.IsImmobile)
            {
                _targetPos = _currentPos;
            }

            _distance = (_targetPos - _currentPos).magnitude;

            // Stop - Accelerate - Decelerate Conditions
            if (_distance < _highestSpeedReached / (10 * _stopSensitivity) || _distance < 0.02f || _lastDistance < _distance && _distanceCounter < 1)
            {
                Stop();
            }
            if (_accelerate && _distance > _currentSpeed * _currentSpeed / (2 * _deceleration))
            {
                Accelerate();
            }
            if (_distance < _highestSpeedReached * _highestSpeedReached / (2 * _deceleration) && !_stop || _currentSpeed > _maxSpeed + 0.2f)
            {
                Decelerate();
            }


            if (TSMath.Abs(_currentSpeed) > 0.001f) { Rotate(); }

            _lastDistance = _distance;
            if (_distanceCounter > 0)
            {
                _distanceCounter--;
            }
        }

        // Stop Player
        private void Stop()
        {
            _rb.velocity = TSVector.zero;
            _stop = true;
            _accelerate = false;
            _currentSpeed = 0;
        }

        // Accelerate Player
        private void Accelerate()
        {
            if (_currentSpeed < _maxSpeed)
            {
                _rb.AddForce(_direction * _acceleration, ForceMode.Force);
                _currentSpeed = _rb.velocity.magnitude;
                _highestSpeedReached = _currentSpeed;
            }
            else
            {
                _currentSpeed = _rb.velocity.magnitude;
            }
        }

        // Decelerate Player
        private void Decelerate()
        {
            _rb.AddForce(_direction * -_deceleration, ForceMode.Force);
        }


        #endregion

        // Wall Collision
        //        public void OnSyncedCollisionStay(TSCollision collision)
        //        {
        //
        //            if (collision.gameObject.layer == 14)
        //            {
        //                Debug.Log("col" + collision.contacts[0].normal);
        //                _rb.tsTransform.position = _rb.tsTransform.position - collision.contacts[0].normal * 0.05f;
        //                _lastDistance = Mathf.Infinity;
        //                _distanceCounter = 3;
        //
        //                if (TSMath.Abs(collision.contacts[0].normal.x) < 0.001f)
        //                {
        //                    _rb.velocity = TSVector.right * _rb.velocity.x;
        //                    _targetPos = _rb.tsTransform.position + TSVector.right * _rb.velocity.x / 10;
        //                    _direction = (_targetPos - _rb.tsTransform.position).normalized;
        //                    _highestSpeedReached = _rb.velocity.magnitude;
        //                }
        //                else if (TSMath.Abs(collision.contacts[0].normal.z) < 0.001f)
        //                {
        //                    _rb.velocity = TSVector.forward * _rb.velocity.z;
        //                    _targetPos = _rb.tsTransform.position + TSVector.forward * _rb.velocity.z / 10;
        //                    _direction = (_targetPos - _rb.tsTransform.position).normalized;
        //                    _highestSpeedReached = _rb.velocity.magnitude;
        //                }
        //                else
        //                {
        //                    _rb.velocity = TSVector.zero;
        //                    _targetPos = _rb.tsTransform.position;
        //                    _stop = true;
        //                    _accelerate = false;
        //                }
        //            }
        //        }

        private void Rotate()
        {
            var lookRotation = _rb.tsTransform.rotation.eulerAngles.y % 360;
            int sign;
            FP difference;
            lookRotation = lookRotation > 0 ? lookRotation : 360 + lookRotation;

            if (_targetRotation > lookRotation)
            {
                difference = TSMath.Abs(_targetRotation - lookRotation) < 360 - TSMath.Abs(_targetRotation - lookRotation)
                    ? TSMath.Abs(_targetRotation - lookRotation) : 360 - TSMath.Abs(_targetRotation - lookRotation);
                sign = TSMath.Abs(_targetRotation - lookRotation) < 360 - TSMath.Abs(_targetRotation - lookRotation) ? 1 : -1;
            }
            else
            {
                difference = TSMath.Abs(lookRotation - _targetRotation) < 360 - TSMath.Abs(lookRotation - _targetRotation)
                    ? TSMath.Abs(lookRotation - _targetRotation) : 360 - TSMath.Abs(lookRotation - _targetRotation);
                sign = TSMath.Abs(lookRotation - _targetRotation) < 360 - TSMath.Abs(lookRotation - _targetRotation) ? -1 : 1;
            }

            if (TSMath.Abs(_targetRotation - lookRotation) > _rotationSpeed * difference / 90)
            {
                _rb.tsTransform.Rotate(0, sign * _rotationSpeed * difference / 180, 0);
            }
        }
    }
}