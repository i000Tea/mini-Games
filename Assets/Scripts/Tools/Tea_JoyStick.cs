using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Tea
{
	public class Tea_JoyStick : ScrollRect
	{
		public Vector2 inputContent
		{
			get
			{
				if (m_Radius == 0)
					m_Radius = (transform as RectTransform).sizeDelta.x * 0.45f;

				return this.content.anchoredPosition / m_Radius;
			}
		}
		/// <summary>
		/// 半径
		/// </summary>
		float m_Radius;
		protected override void Awake()
		{
			base.Awake();
			m_Radius = (transform as RectTransform).sizeDelta.x * 0.45f;
		}
		public override void OnDrag(PointerEventData eventData)
		{
			base.OnDrag(eventData);

			//新建v2
			Vector2 pos;
			// 如果点击 RectTransform 平面，则无论点是否在矩形内，都返回 true。
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform,
				eventData.position,
				eventData.pressEventCamera,
				out pos))
			{
				content.anchoredPosition = pos;
				// 当前移动位置为移动后的值
			}

			if (content.anchoredPosition.magnitude > m_Radius)
			{
				var contentPosition = content.anchoredPosition.normalized * m_Radius;
				SetContentAnchoredPosition(contentPosition);
			}
		}
	}
}
