using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Tea;
using DG.Tweening;
using RootMotion.FinalIK;

namespace Tea.PolygonHit
{
	/// <summary>
	/// 输入控制器
	/// </summary>
	public class InputController : ButtonControlBase
	{
		#region 变量 
		/// <summary>
		/// 反向拖拽模式
		/// </summary>
		[Space(20)]
		public bool reverseDragMode;
		/// <summary>
		/// 拖拽线
		/// </summary>
		public LineRenderer LR;
		/// <summary>
		/// 拖拽方向箭头
		/// </summary>
		public Transform dragImage;
		[SerializeField]
		private RectTransform rectImage;
		[SerializeField]
		private Image _image;
		public Transform targets;
		/// <summary>
		/// 瞄准镜(鼠标一开始的位置)
		/// </summary>
		public Image aim;

		/// <summary>
		/// 拖拽偏移位置 为初始与当前的差值
		/// </summary>
		Vector3 dragOffset => movePosition - awakeNewPosition;
		float dragPower
		{
			get
			{
				if (dragOffset.magnitude < 1000)
					return  dragOffset.magnitude;
				else
					return 1000;
			}
		}

		/// <summary>
		/// 返回阈值
		/// </summary>
		private bool backValve;
		/// <summary>
		/// 距离大于最小距离 则启动射击阈值
		/// </summary>

		private bool isShoot;

		/// <summary>
		/// 最小距离 由此判断设计与阈值
		/// </summary>
		public float minLong = 100;

		[SerializeField]
		private float Power = 10;

		/// <summary>
		/// 输出的力
		/// </summary>
		float outputPower
		{
			get
			{
				var _power = Power * 0.1f;

				if (reverseDragMode)
				{
					_power *= -1;
				}

				return _power * dragPower;
			}
		}

		#endregion

		#region unity void

		private void OnValidate()
		{
			rectImage = dragImage.GetComponent<RectTransform>();
			_image = dragImage.GetComponent<Image>();
		}

		private void OnDestroy()
		{
			RemoveEvent();
		}
		private void Update()
		{
			if (PlayerBase.I)
			{
				if (LR.enabled)
				{
					// 线段位置与小球一致
					LR.transform.localPosition = PlayerBase.Player.localPosition - new Vector3(0, 0, -1);
				}
				if (_image.enabled)
				{
					_image.transform.localPosition = PlayerBase.Player.localPosition - new Vector3(0, 0, -1);
				}
			}
		}

		#endregion

		protected override void AwakeSet()
		{
			if (dragSomeObj == null)
			{
				var A = new GameObject();
				A.transform.SetParent(transform);
				A.transform.localPosition = Vector3.zero;
				A.AddComponent<RectTransform>();
				dragSomeObj = A.transform as RectTransform;
			}
			base.AwakeSet();
			AddEvent();
		}

		#region Event
		void AddEvent()
		{
			EventController.AddListener(EventType.PlayerDestory, TouthOver);
		}
		void RemoveEvent()
		{
			EventController.RemoveListener(EventType.PlayerDestory, TouthOver);
		}
		#endregion

		/// <summary>
		/// 力量设置
		/// </summary>
		/// <param name="_long"></param>
		void SetPower(float _long)
		{
			// 当第一次大于最小距离 开启 返回阈值 
			if (!backValve && _long > minLong)
				backValve = true;

			// 初始化时 返回阈值关闭 不管怎么样都有力量值
			if (!backValve)
			{
				isShoot = true;
				LR.material.DOColor(Color.red, 0.2f);
			}
			else
			{
				if (!isShoot && _long > minLong)
				{
					isShoot = true;
					LR.material.DOColor(Color.red, 0.2f);
				}
				else if (isShoot && _long < minLong)
				{
					isShoot = false;
					LR.material.DOColor(Color.white, 0.2f);
				}
			}


		}
		public override void OnDragFrist(PointerEventData eventData)
		{
			base.OnDragFrist(eventData);
			// 拖拽开始 记录初始鼠标位置
			awakeNewPosition = MouseNowPosition(eventData);

			if (reverseDragMode)
			{
				SetDragLine();
			}
			else
			{
				SetDragImage();
			}

			aim.enabled = true;
			aim.transform.localPosition = awakeNewPosition;

			backValve = false;
		}
		public override void OnDrag(PointerEventData eventData)
		{
			base.OnDrag(eventData);

			if (reverseDragMode)
			{
				SetDragLine();
			}
			else
			{
				SetDragImage();
			}
			SetPower(dragOffset.magnitude);

			if (targets)
				targets.position = dragOffset;
			PlayerBase.I.FixMovement(dragOffset.normalized);
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			// 刚体运动
			if (isShoot)
			{
				PlayerBase.I.ShootMovement(dragOffset.normalized, outputPower);
			}

			base.OnEndDrag(eventData);

			EventController.Broadcast(EventType.action_Shoot);


			if (reverseDragMode)
				SetDragLine(false);
			else
				SetDragImage(false);

			aim.enabled = false;
		}

		/// <summary>
		/// 设置拖拽线
		/// </summary>
		/// <param name="open"></param>
		void SetDragLine(bool open = true)
		{
			LR.enabled = open;

			LR.SetPosition(1, dragOffset);

			if (!open)
			{
				// 线段隐藏
				LR.SetPosition(1, transform.position);

				LR.material.color = Color.white;
			}
		}
		/// <summary>
		/// 设置拖拽图标
		/// </summary>
		/// <param name="open"></param>
		void SetDragImage(bool open = true)
		{
			_image.enabled = open;
			try
			{
				// 计算目标与自身的向量差
				var dir = movePosition - awakeNewPosition;
				// 向量Z归零
				dir.z = 0;
				// 返回有符号的角度
				var angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
				dragImage.rotation = Quaternion.Euler(0, 0, angle);
				rectImage.sizeDelta = new Vector2(dragOffset.magnitude, 50);
			}
			catch (System.Exception)
			{
				Debug.LogWarning("拖拽错误");
				throw;
			}

		}

		void TouthOver()
		{
			gameObject.SetActive(false);
		}
	}
}
