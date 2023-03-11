using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tea_CrossSlip : ScrollRect
{
	public Vector2 inputContent
	{
		get { return this.content.anchoredPosition / m_Radius; }
	}
	float m_Radius;

	public GameObject elementBase;

	protected override void Awake()
	{
		elementBase = transform.GetChild(0).GetChild(0).gameObject;
	}
	protected override void Start()
	{
		base.Start();
	}
	private void Update()
	{

	}
	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		ReturnPoint();
	}
	public override void OnEndDrag(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);
		ReturnPoint();
	}
	void ReturnPoint()
	{
		var aP = (content.transform as RectTransform).anchoredPosition;
		Debug.Log(aP);
		if (Mathf.Abs(aP.x) > 100)
		{
			(content.transform as RectTransform).anchoredPosition = new Vector3(aP.x % 160, aP.y);
		}
	}
}
