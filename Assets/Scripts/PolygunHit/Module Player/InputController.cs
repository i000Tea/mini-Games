using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Tea;
using Tea.CyberCard;
using DG.Tweening;

namespace Tea.PolygonHit
{
    /// <summary>
    /// 输入控制器
    /// </summary>
    public class InputController : ButtonControlBase
    {
		#region bianliang 

		[Space(20)]
		public bool reverseDrag;
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
		/// 鼠标的初始所在坐标
		/// </summary>
		Vector3 StartPoint;
        /// <summary>
        /// 鼠标所在位置
        /// </summary>
        Vector3 target;

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

        public float Power = 10;

		#endregion

		#region void

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
            if (LR.enabled)
                // 线段位置与小球一致
                LR.transform.localPosition = PlayerBase.Player.localPosition - new Vector3(0, 0, -1);
			if(_image.enabled)
				_image.transform.localPosition = PlayerBase.Player.localPosition - new Vector3(0, 0, -1);
		}

		#endregion

		protected override void AwakeSet()
		{
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
        public override void OnDrag(PointerEventData eventData)
        {
			base.OnDrag(eventData);
			// 拖拽开始 记录初始鼠标位置
			if (StartPoint == Vector3.zero)
            {
                StartPoint = _movePosition(eventData);

				if (reverseDrag)
					SetDragLine();
				else
					SetDragImage();

                aim.enabled = true;
                aim.transform.localPosition = _movePosition(eventData);

                backValve = false;
            }


			Debug.Log("Drag1+1");
			// 线段末尾为初始与当前的插值
			target = movePosition - StartPoint;

			Debug.Log("Drag2+1");

			if (reverseDrag)
				SetDragLine();
			else
				SetDragImage();


			Debug.Log("Drag3");
			SetPower(target.magnitude);

            if (targets)
                targets.position = target;
			Debug.Log("Drag3");
			Debug.Log($"{movePosition} - {StartPoint}");
		}

		public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);


            StartPoint = Vector3.zero;

            // 刚体运动
            if (isShoot)
			{
				Vector2 _power = target.normalized * 0.1f * Power;

				if (reverseDrag)
					_power *= -1;

				if (target.magnitude < 1000)
					_power *= target.magnitude;
				else
					_power *= 1000;

				PlayerBase.inst.m_Rig.velocity += _power;
            }

			EventController.Broadcast(EventType.action_Shoot);


			if (reverseDrag)
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

			LR.SetPosition(1, target);

			if (!open)
			{
				// 线段隐藏
				LR.SetPosition(1, transform.position);

				LR.material.color = Color.white;
			}
		}
		void SetDragImage(bool open = true)
		{
			_image.enabled = open;
			try
			{
				// 计算目标与自身的向量差
				var dir = movePosition - StartPoint;
				// 向量Z归零
				dir.z = 0;
				// 返回有符号的角度
				var angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
				dragImage.rotation = Quaternion.Euler(0, 0, angle);
				rectImage.sizeDelta = new Vector2(target.magnitude / 2, 50);
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
