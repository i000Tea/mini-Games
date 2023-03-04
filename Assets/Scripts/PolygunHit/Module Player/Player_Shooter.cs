using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.PolygonHit
{
	/// <summary>
	/// 第一个做的角色：枪手
	/// </summary>
    public class Player_Shooter : PlayerBase
    {
		public GameObject[] skill;
		protected override void Start()
		{
			base.Start();
			AmmoMax(10);
			for (int i = 0; i < skill.Length; i++)
			{
				SkillManager.inst.AddSkill(skill[i]);
			}
		}
		public int fenshu;


		public void Add(int a)
		{
			fenshu += a;
		}
	}
}
