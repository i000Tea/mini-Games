using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace Tea
{
    /// <summary>
    /// 自定义的按钮基类(工程需要加载dotween) 目前有拖拽和点击两种事件
    /// </summary>
    public class ButtonControlBase : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Variable    变量
        [SerializeField]
        private ButtonControlSetting m_Setting;

        [SerializeField]
        private bool isDebug = false;

        /// <summary>
        /// 鼠标是否在自身身上
        /// </summary>
        private bool mouseIsIn = false;
        private bool drawing = false;
        #region Draw 拖拽移动
        /// <summary>
        /// 父集位置
        /// </summary>
        public RectTransform ParentRTran
        {
            get
            {
                if (!m_ParentRTran)
                    m_ParentRTran = transform.parent.GetComponent<RectTransform>();
                return m_ParentRTran;
            }
        }
        private RectTransform m_ParentRTran;
        /// <summary>
        /// 自身坐标位置
        /// </summary>
        private RectTransform myRTran;
        /// <summary>
        /// 初始坐标
        /// </summary>
        private Vector3 awakePosition;
		protected Vector3 _movePosition(PointerEventData eventData)
		{
			//新建v2
			Vector2 pos;
			// 如果点击 RectTransform 平面，则无论点是否在矩形内，都返回 true。
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentRTran,
				eventData.position,
				eventData.pressEventCamera,
				out pos))
			{
				// 当前移动位置为移动后的值
				movePosition = pos;
				// 若可以移动 则自身坐标为移动后的值
				if (m_Setting.CanDraw)
					myRTran.localPosition = pos;
				return pos;
			}
			else
			{
				return Vector3.zero;
			}

		}

		/// <summary>
		/// 鼠标的当前移动位置
		/// </summary>
		protected Vector3 movePosition;
		#endregion

		#endregion

		#region Unity void 基础方法

		private void OnValidate()
        {
            //if (!m_Setting)
            //{
            //    m_Setting = ButtonControlSetting.FindSetting();
            //}
        }

        private void Awake()
        {
			AwakeSet();
		}
        #endregion

		protected virtual void AwakeSet()
		{
			// 获取自身UI坐标
			if (TryGetComponent(out RectTransform RT)) myRTran = RT;
		}

        #region 鼠标触碰 点击
        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter();
            if (m_Setting.CanEnter)
            {
                //Debug.Log("鼠标进入");
                if (m_Setting.CanEnterAnim)
                {
                    OnScale(m_Setting.enter_Scale, m_Setting.enter_Time);
                }
                mouseIsIn = true;
            }
        }
        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit();
            if (m_Setting.CanEnter && !drawing)
            {
                //Debug.Log("鼠标离开"); 
                if (m_Setting.CanEnterAnim)
                {
                    OnScale(1f, m_Setting.enterBack_Time);
                }
                mouseIsIn = false;
            }
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (m_Setting.CanClick)
            {
                //Debug.Log("按下鼠标");
                if (m_Setting.CanClickAnim)
                {
                    OnScale(m_Setting.click_Scale, m_Setting.click_Time);
                }
            }
        }
        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (m_Setting.CanClick)
            {
                //Debug.Log("抬起鼠标");
                if (m_Setting.CanClickAnim)
                {
                    if (!mouseIsIn)
                        OnScale(1f, m_Setting.clickBack_Time);
                    else
                        OnScale(m_Setting.enter_Scale, m_Setting.enter_Time);
                }
            }
        }
        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="eventData"></param>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("鼠标单击");
            if (m_Setting.CanClick)
            {
                OnClick();
            }
        }
        /// <summary>
        /// 点击事件
        /// </summary>
        protected virtual void OnClick()
        {
        }
        /// <summary>
        /// 进入事件
        /// </summary>
        protected virtual void OnEnter()
        {
        }
        /// <summary>
        /// 离开事件
        /// </summary>
        protected virtual void OnExit()
        {
        }

        #endregion

        #region 鼠标拖拽
        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (isDebug)
                Debug.Log("StartDrap");

            if (m_Setting.CanDraw)
            {
                drawing = true;
                awakePosition = myRTran.localPosition;
            }
        }
        /// <summary>
        /// 拖拽中
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnDrag(PointerEventData eventData)
        {
            if (isDebug)
                Debug.Log("isDrap");
			// 若可拖拽 自身坐标更新
			_movePosition(eventData);
			if (m_Setting.CanDraw)
            {
                Vector2 pos;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentRTran,
                    eventData.position,
                    eventData.pressEventCamera,
                    out pos))
                {
                    myRTran.localPosition = pos;
                }
            }
        }
        /// <summary>
        /// 结束拖拽
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (isDebug)
                Debug.Log("DrapOvre");
            if (m_Setting.CanDraw)
            {
                drawing = false;
                StartCoroutine(ButtonDisAppear());
            }
        }
        #endregion

        #region Anim void 具体动画方法
        /// <summary>
        /// 动态改变自身的大小
        /// </summary>
        public virtual void OnScale(float Scale, float time = 0.25f)
        {
            transform.DOScale(Scale, time);
        }

        /// <summary>
        /// 按钮返回
        /// </summary>
        /// <returns></returns>
        IEnumerator ButtonDisAppear()
        {
            transform.DOLocalMove(awakePosition, 0.25f);
            transform.DOScale(1f, 0.25f);
            yield return new WaitForSeconds(0.5f);
        }
        #endregion

        #region Other void
        #endregion
    }
}
