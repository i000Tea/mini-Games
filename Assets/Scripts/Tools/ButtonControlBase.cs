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
        protected bool isDebug = false;

        /// <summary>
        /// 鼠标是否在自身身上
        /// </summary>
        protected bool mouseIsIn = false;
        /// <summary>
        /// 是否处于拖拽中
        /// </summary>
        protected bool drawing = false;
        #region Draw 拖拽移动
        /// <summary>
        /// 父集位置
        /// </summary>
        public RectTransform ParentRectTrans => transform.parent.GetComponent<RectTransform>();
        /// <summary>
        /// 自身坐标位置
        /// </summary>
        protected RectTransform thisRectTrans;
        /// <summary>
        /// 拖拽偏移
        /// </summary>

        protected Vector2 dragOffsetPoint;

        /// <summary>
        /// 被拖拽的物体
        /// </summary>
        protected virtual Transform DragSomeTran
        {
            get
            {
                // 当拖拽对象为空 
                if (dragSomeObj == null)
                {
                    dragSomeObj = thisRectTrans;
                }
                else if (dragOffsetPoint == Vector2.zero)
                {
                    dragOffsetPoint = dragSomeObj.localPosition;
                }
                return dragSomeObj;
            }
        }
        protected RectTransform DragRTran => dragSomeObj as RectTransform;

        /// <summary>
        /// 被拖拽的某物体(手动配置)
        /// </summary>
        [SerializeField]
        protected Transform dragSomeObj;

        /// <summary>
        /// 初始坐标
        /// </summary>
        protected Vector3 awakePosition;
        /// <summary>
        /// 转换父集后的初始坐标
        /// </summary>
        protected Vector3 awakeNewPosition;

        /// <summary>
        /// 位移位置
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
		protected Vector3 MovePosition(PointerEventData eventData)
        {
            //新建v2
            Vector2 pos;
            // 如果点击 RectTransform 平面，则无论点是否在矩形内，都返回 true。
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentRectTrans,
                eventData.position,
                eventData.pressEventCamera,
                out pos))
            {
                // 当前移动位置为移动后的值
                movePosition = pos;
                // 若可以移动 则自身坐标为移动后的值
                if (m_Setting.CanDraw)
                    DragSomeTran.localPosition = pos;
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

        Tween inAnim;
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

        /// <summary>
        /// 初始化设置
        /// </summary>
        protected virtual void AwakeSet()
        {
            // 获取自身UI坐标
            if (TryGetComponent(out RectTransform RT)) thisRectTrans = RT;
        }

        #region 鼠标触碰 点击
        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isDebug)
                Debug.Log("PointerEnter");
            OnEnter();
            if (m_Setting.CanEnter)
            {
                //Debug.Log("鼠标进入");
                if (m_Setting.CanEnterAnim)
                {
                    inAnim = OnScale(m_Setting.enter_Scale, m_Setting.enter_Time);
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
            if (isDebug)
                Debug.Log("PointerExit");
            OnExit();
            if (m_Setting.CanEnter &&
                // 当不处于拖拽中 或 被拖拽的物体不是自身
                (!drawing || DragSomeTran != thisRectTrans))
            {
                //Debug.Log("鼠标离开"); 
                if (m_Setting.CanEnterAnim)
                {
                    if (inAnim != null)
                    {
                        inAnim.Kill();
                        inAnim = null;
                    }
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
            if (isDebug)
                Debug.Log("PointerDown");
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
                awakePosition = DragSomeTran.localPosition;
                if (DragSomeTran != thisRectTrans)
                {
                    DragSomeTran.SetParent(thisRectTrans.parent);
                }
            }
        }
        /// <summary>
        /// 拖拽中
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            if (awakeNewPosition == default)
            {
                awakeNewPosition = DragSomeTran.localPosition;
            }
            //if (isDebug)
            //    Debug.Log("isDrap");
            // 若可拖拽 自身坐标更新
            MovePosition(eventData);
            if (m_Setting.CanDraw)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentRectTrans,
                    eventData.position,
                    eventData.pressEventCamera,
                    out pos))
                {
                    DragSomeTran.localPosition = pos;
                    //Debug.Log($"{pos}  {DragTran.localPosition}");
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
                awakeNewPosition = default;
                dragOffsetPoint = default;
                if (DragSomeTran != thisRectTrans)
                {
                    DragSomeTran.SetParent(thisRectTrans);
                }
                StartCoroutine(ButtonDisAppear());
            }
        }
        #endregion

        #region Anim void 具体动画方法
        /// <summary>
        /// 动态改变自身的大小
        /// </summary>
        public virtual Tween OnScale(float Scale, float time = 0.25f)
        {
            return transform.DOScale(Scale, time);
        }

        /// <summary>
        /// 按钮返回
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator ButtonDisAppear()
        {
            yield return new WaitForFixedUpdate();
            DragSomeTran.transform.DOLocalMove(awakePosition /*+ new Vector3(dragOffsetPoint.x,  dragOffsetPoint.y, 0)*/, 0.25f);
            DragSomeTran.transform.DOScale(1f, 0.25f);
            yield return new WaitForSeconds(0.5f);
        }
        #endregion

        #region Other void
        #endregion
    }
}
