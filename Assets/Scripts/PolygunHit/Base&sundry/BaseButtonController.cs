using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace Tea.CyberCard
{
    /// <summary>
    /// 自定义的按钮 基类 目前有拖拽和点击两种事件
    /// </summary>
    public class BaseButtonController : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region variable

        [SerializeField]
        private bool isDebug = false;

        #region Touth 点击
        [Tooltip("鼠标[点击]时是否有缩放动画")]
        public bool isClickScale;
        [Tooltip("鼠标[进入]时是否有缩放动画")]
        public bool isEnterScale;
        [Tooltip("鼠标位于自身上")]
        bool inClick;
        #endregion

        #region Draw 拖拽移动
        [Tooltip("是否可以被拖拽")]
        public bool isCanDrag = true;
        /// <summary>
        /// 父集位置
        /// </summary>
        private RectTransform PartRTran
        {
            get
            {
                if (!partRTran)
                    partRTran = transform.parent.GetComponent<RectTransform>();
                return partRTran;
            }
        }
        private RectTransform partRTran;
        /// <summary>
        /// 自身坐标位置
        /// </summary>
        private RectTransform myRTran;
        /// <summary>
        /// 初始坐标
        /// </summary>
        Vector3 awakePosition;
        protected Vector3 _movePosition(PointerEventData eventData)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(PartRTran,
                eventData.position,
                eventData.pressEventCamera,
                out pos))
            {
                movePosition = pos;
                if (isCanDrag)
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

        #region Unity void
        private void Awake()
        {
            if (TryGetComponent(out RectTransform RT)) myRTran = RT;
            //Debug.Log("0");
        }
        #endregion

        #region 鼠标触碰 点击
        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isEnterScale)
                OnScale(1.5f);
            inClick = true;
        }
        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (isEnterScale)
                OnScale(1f);
            inClick = false;
        }
       
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (isClickScale)
                if (!isEnterScale)
                    OnScale(0.75f, 0.1f);
                else
                    OnScale(0.9f, 0.1f);
        }
        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (isClickScale)
                if (!isEnterScale || !inClick)
                    OnScale(1);
                else
                    OnScale(1.5f);
        }
        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="eventData"></param>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }
        /// <summary>
        /// 点击
        /// </summary>
        public virtual void OnClick()
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

            if (isCanDrag)
                awakePosition = myRTran.localPosition;
        }
        /// <summary>
        /// 拖拽中
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnDrag(PointerEventData eventData)
        {
            if (isDebug)
                Debug.Log("isDrap");

            _movePosition(eventData);
        }
        /// <summary>
        /// 结束拖拽
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (isDebug)
                Debug.Log("DrapOvre");
            if (isCanDrag)
                StartCoroutine(CardDisAppear());
        }
        IEnumerator CardDisAppear()
        {
            transform.DOLocalMove(awakePosition, 0.25f);
            transform.DOScale(1f, 0.25f);
            yield return new WaitForSeconds(0.5f);
        }
        #endregion

        #region Add void
        /// <summary>
        /// 动态改变自身的大小
        /// </summary>
        public virtual void OnScale(float Scale, float time = 0.25f)
        {
            transform.DOScale(Scale, time);
        }
        #endregion
    }
}
