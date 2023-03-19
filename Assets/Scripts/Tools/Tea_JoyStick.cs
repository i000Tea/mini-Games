using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tea_JoyStick : ScrollRect
{
	public Vector2 inputContent
	{
		get { return this.content.anchoredPosition / m_Radius; }
	}
	float m_Radius;
	protected override void Start()
	{
		base.Start();
		m_Radius = (transform as RectTransform).sizeDelta.x * 0.45f;
	}
	public override void OnDrag(PointerEventData eventData)
	{
		base.OnDrag(eventData);

		var contentPosition = this.content.anchoredPosition * 2.4f;

		if (contentPosition.magnitude > m_Radius)
		{
			contentPosition = contentPosition.normalized * m_Radius;
			SetContentAnchoredPosition(contentPosition);
		}
	}
}
