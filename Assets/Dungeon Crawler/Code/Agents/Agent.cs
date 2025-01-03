using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dante.DungeonCrawler
{
    #region Enums


    #endregion

    #region Structs


    #endregion

    //Agent cannot operate without the Rigidbody2D
    [RequireComponent(typeof(Rigidbody2D))]
    public class Agent : MonoBehaviour
    {
        //Configuration parameter of this script
        #region Knobs

        public int maxHealthPoints = 3;

        #endregion

        #region References

        [SerializeField, HideInInspector] protected Rigidbody2D _rigidbody;
        [SerializeField] protected FiniteStateMachine _fsm;

        #endregion

        #region RuntimeVariables

        [SerializeField] protected bool _isDead;

        protected Vector2 _movementDirection;
        protected StateMechanics _movementStateMechanic;

        protected bool _isSprinting;

        #endregion

        #region LocalMethods

        protected virtual void CalculateStateMechanicDirection()
        {
            if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.down) > 0.1f)
            {
                if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.right) > 0.1f)
                {
                    if (!_isSprinting)
                    {
                        _movementStateMechanic = StateMechanics.MOVE_RIGHT;
                    }
                    else
                    {
                        _movementStateMechanic = StateMechanics.SPRINT_RIGHT;
                    }
                }
                else
                {
                    if (!_isSprinting)
                    {
                        _movementStateMechanic = StateMechanics.MOVE_DOWN;
                    }
                    else
                    {
                        _movementStateMechanic = StateMechanics.SPRINT_DOWN;
                    }
                }

            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.left) > 0.1f)
            {
                if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.up) > 0.1f)
                {
                    if (!_isSprinting)
                    {
                        _movementStateMechanic = StateMechanics.MOVE_LEFT;
                    }
                    else
                    {
                        _movementStateMechanic = StateMechanics.SPRINT_LEFT;
                    }
                }
                else
                {
                    if (!_isSprinting)
                    {
                        _movementStateMechanic = StateMechanics.MOVE_DOWN;
                    }
                    else
                    {
                        _movementStateMechanic = StateMechanics.SPRINT_DOWN;
                    }
                }

            }
            else if (Vector2.Dot(_fsm.GetMovementDirection, Vector2.up) > 0.1f)
            {
                if (!_isSprinting)
                {
                    _movementStateMechanic = StateMechanics.MOVE_UP;
                }
                else
                {
                    _movementStateMechanic = StateMechanics.SPRINT_UP;
                }
            }
            else
            {
                if (!_isSprinting)
                {
                    _movementStateMechanic = StateMechanics.MOVE_RIGHT;
                }
                else
                {
                    _movementStateMechanic = StateMechanics.SPRINT_RIGHT;
                }
            }
        }

        #endregion

        #region UnityMethods

        private void Start()
        {
            //InitializeAgent();
        }

        //ranges from 24 to 200 FPS
        //(according to the computer)
        void Update()
        {
            
        }

        //private void PhysicsUpdate()
        private void FixedUpdate()
        {
            //when we update the rigid body, we do it
            //during the Physics thread update
            //which is the FixedUpdate()
            //within the PhysX Engine (by NVidia) in Unity
            //_rigidbody.velocity = Vector3.right;
            //_rigidbody.AddForce(Vector2.right);
        }

        #endregion

        #region PublicMethods

        public virtual void InitializeAgent()
        {
            _isSprinting = false;
            //With the RequireComponent we guarantee
            //this reference will be ALWAYS retreived
            /*
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null ) {
                Debug.LogError("Rigid body has not been assigned to " +
                    gameObject.name);
            }
            */
        }

        public void StateMechanic(StateMechanics value)
        {
            _fsm.StateMechanic(value);
        }
        #endregion

        #region GettersSetters

        public StateMechanics GetMovementStateMechanic
        {
            get { return _movementStateMechanic; }
        }

        public bool IsDead { get { return _isDead; } set { _isDead = value; } }

        #endregion
    }
}