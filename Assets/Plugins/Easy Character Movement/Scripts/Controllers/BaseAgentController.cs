using UnityEngine;
using UnityEngine.AI;

namespace ECM.Controllers
{
	/// <summary>
	/// Base Agent (NavMesh) Controller.
	/// 
	/// Base class for a 'NavMeshAgent' controlled characters.
	/// It inherits from 'BaseCharacterController' and extends it to control a 'NavMeshAgent'
	/// and intelligently move in response to mouse click (click to move).
	/// 
	/// As the base character controller, this default behaviour can easily be modified completely replaced in a derived class.
	/// </summary>

	public class BaseAgentController : BaseCharacterController
	{
		#region EDITOR EXPOSED FIELDS 编辑器公开的字段

		[Header("Navigation")]
		[Tooltip("代理是否应该自动刹车以避免超过目的点? \n" +
					"如果该属性被设置为true，代理将在接近目的地时自动刹车。 \n" +
					"Should the agent brake automatically to avoid overshooting the destination point? \n" +
					"If this property is set to true, the agent will brake automatically as it nears the destination.")]
		[SerializeField]
		private bool _autoBraking = true;

		[Tooltip("从目标位置开始制动的距离。 Distance from target position to start braking.")]
		[SerializeField]
		private float _brakingDistance = 2.0f;

		[Tooltip("Stop within this distance from the target position.")]
		[SerializeField]
		private float _stoppingDistance = 1.0f;

		[Tooltip("Layers to be considered as ground (walkables). Used by ground click detection.")]
		[SerializeField]
		public LayerMask groundMask = 1;            // Default layer

		#endregion

		#region PROPERTIES 属性

		/// <summary>
		/// 缓存的 导航网格代理 组件。
		/// Cached NavMeshAgent component.
		/// </summary>

		public NavMeshAgent agent { get; private set; }

		/// <summary>
		/// 代理是否应该自动刹车以避免超过目的点?
		/// 如果该属性设置为true，代理将在接近目的地时自动刹车。
		/// Should the agent brake automatically to avoid overshooting the destination point?
		/// If this property is set to true, the agent will brake automatically as it nears the destination.
		/// </summary>

		public bool autoBraking
		{
			get { return _autoBraking; }
			set
			{
				_autoBraking = value;

				if (agent != null)
					agent.autoBraking = _autoBraking;
			}
		}

		/// <summary>
		/// 从目标位置开始制动的距离。
		/// Distance from target position to start braking.
		/// </summary>

		public float brakingDistance
		{
			get { return _brakingDistance; }
			set { _brakingDistance = Mathf.Max(0.0001f, value); }
		}

		/// <summary>
		/// 药剂的剩余距离与制动距离之比(0 - 1范围)。
		/// 如果没有自动制动或药剂的剩余距离大于制动距离。
		/// 如果代理的剩余距离小于制动距离，则小于1。
		/// The ratio (0 - 1 range) of the agent's remaining distance and the braking distance.
		/// 1 If no auto braking or if agent's remaining distance is greater than brakingDistance.
		/// less than 1, if agent's remaining distance is less than brakingDistance.
		/// </summary>

		public float brakingRatio
		{
			get
			{
				if (!autoBraking || agent == null)
					return 1f;

				return agent.hasPath ? Mathf.Clamp(agent.remainingDistance / brakingDistance, 0.1f, 1f) : 1f;
			}
		}

		/// <summary>
		/// 在距离目标位置的距离内停止
		/// Stop within this distance from the target position.
		/// </summary>

		public float stoppingDistance
		{
			get { return _stoppingDistance; }
			set
			{
				_stoppingDistance = Mathf.Max(0.0f, value);

				if (agent != null)
					agent.stoppingDistance = _stoppingDistance;
			}
		}

		#endregion

		#region METHODS 方法

		/// <summary>
		/// 同步NavMesh Agent模拟位置与角色移动位置
		/// 我们控制代理。
		/// 注意:必须在LateUpdate方法中调用。
		/// Synchronize the NavMesh Agent simulation position with the character movement position,
		/// we control the agent.
		/// 
		/// NOTE: Must be called in LateUpdate method.
		/// </summary>

		protected void SyncAgent()
		{
			agent.speed = speed;
			agent.angularSpeed = angularSpeed;

			agent.acceleration = acceleration;
			agent.velocity = movement.velocity;

			agent.nextPosition = transform.position;
		}

		/// <summary>
		/// 根据 代理 的信息指定角色的移动方向(输入)
		/// Assign the character's desired move direction (input) based on agent's info.
		/// </summary>

		protected virtual void SetMoveDirection()
		{
			// 如果代理没有移动，返回
			// If agent is not moving, return

			moveDirection = Vector3.zero;

			if (!agent.hasPath)
				return;

			// 当目标移动方向时
			// 如果没有到达目标，输入代理的期望速度(仅横向)
			// If destination not reached,
			// feed agent's desired velocity (lateral only) as the character move direction

			if (agent.remainingDistance > stoppingDistance)
				moveDirection = Vector3.ProjectOnPlane(agent.desiredVelocity, transform.up);
			else
			{
				// If destination is reached,
				// reset stop agent and clear its path

				agent.ResetPath();
			}
		}

		/// <summary>
		/// 重载 基类 的 CalcDesiredVelocity 方法，
		/// 添加自动制动支持。
		/// Overrides 'BaseCharacterController' CalcDesiredVelocity method,
		/// adding auto braking support.
		/// </summary>

		protected override Vector3 CalcDesiredVelocity()
		{
			SetMoveDirection();

			var desiredVelocity = base.CalcDesiredVelocity();
			return autoBraking ? desiredVelocity * brakingRatio : desiredVelocity;
		}

		/// <summary>
		/// 重载 HandleInput 方法，
		/// 执行自定义输入代码，在这种情况下，点击移动。
		/// Overrides 'BaseCharacterController' HandleInput method,
		/// to perform custom input code, in this case, click-to-move.
		/// </summary>

		protected override void HandleInput()
		{
			// Toggle pause / resume.
			// By default, will restore character's velocity on resume (eg: restoreVelocityOnResume = true)

			if (Input.GetKeyDown(KeyCode.P))
				pause = !pause;

			crouch = Input.GetKey(KeyCode.C);

			// Handle mouse input

			if (!Input.GetButton("Fire2"))
				return;

			// If mouse right click,
			// found click position in the world

			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundMask.value))
				return;

			// 设置代理目的地为地面命中点
			// Set agent destination to ground hit point

			agent.SetDestination(hitInfo.point);
		}

		#endregion

		#region MONOBEHAVIOUR unity

		/// <summary>
		/// Validate this editor exposed fields.
		/// 验证此编辑器公开的字段。
		/// </summary>

		public override void OnValidate()
		{
			// Calls the parent class' version of method.

			base.OnValidate();

			// This class validation

			autoBraking = _autoBraking;

			brakingDistance = _brakingDistance;
			stoppingDistance = _stoppingDistance;
		}

		/// <summary>
		/// Initialize this.
		/// 初始化。
		/// </summary>

		public override void Awake()
		{
			// Calls the parent class' version of method.

			base.Awake();

			// 缓存并初始化组件
			// Cache and initialize components

			agent = GetComponent<NavMeshAgent>();
			if (agent != null)
			{
				agent.autoBraking = autoBraking;
				agent.stoppingDistance = stoppingDistance;

				// Turn-off NavMeshAgent control,
				// we control it, not the other way

				agent.updatePosition = false;
				agent.updateRotation = false;

				agent.updateUpAxis = false;
			}
			else
			{
				Debug.LogError(
					string.Format(
						"NavMeshAgentCharacterController: There is no 'NavMeshAgent' attached to the '{0}' game object.\n" +
						"Please add a 'NavMeshAgent' to the '{0}' game object.",
						name));
			}
		}

		public virtual void LateUpdate()
		{
			// 同步代理与移动
			// Synchronize agent with character movement

			SyncAgent();
		}

		#endregion
	}
}
