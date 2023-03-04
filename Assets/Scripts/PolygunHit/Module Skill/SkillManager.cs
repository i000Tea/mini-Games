using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit
{
	/// <summary>
	/// 技能管理器
	/// </summary>
    public class SkillManager : MonoBehaviour
    {
		public static SkillManager inst;
        public List<SkillBase> mySkills;
        private void Awake()
        {
            mySkills = new List<SkillBase>();
			inst = this;

		}

        private void Start()
        {
            AddEvent();
        }
        private void OnDestroy()
        {
            RemoveEvent();
        }
        private void AddEvent()
        {
            EventController.AddListener<EnemyBase>(EventType.action_Strike, Trigger_Strike);
            EventController.AddListener(EventType.action_Shoot, Trigger_Shoot);
        }
        private void RemoveEvent()
        {
            EventController.RemoveListener<EnemyBase>(EventType.action_Strike, Trigger_Strike);
            EventController.RemoveListener(EventType.action_Shoot, Trigger_Shoot);
        }

		public void AddSkill(GameObject inst)
		{
			// 实例化技能 设置参数
			Transform @object = Instantiate(inst).transform;
			@object.SetParent(transform);
			@object.localPosition = Vector3.zero;
			@object.localScale = Vector3.one;

			// 技能脚本添加到方法
			mySkills.Add(@object.GetComponent<SkillBase>());

			// 返回游戏
			GameManager.inst.SetState(GameState.Gameing);
		}
		public void AddSkill(int filePathNum)
		{
			// 获取技能位置
			string filePath = "Prefabs/Skills/Skill";
            if (filePathNum < 10)
                filePath += "0";
            filePath += filePathNum.ToString();
			//Debug.Log(filePath);

			AddSkill(Resources.Load<GameObject>(filePath));
		}

        #region Trigger 触发方式
		/// <summary>
		/// 弹射
		/// </summary>
        private void Trigger_Shoot()
        {
            for (int i = 0; i < mySkills.Count; i++)
                mySkills[i].CompareSkill(SkillUseType.IsShoot);
        }
		/// <summary>
		/// 撞击
		/// </summary>
        private void Trigger_Strike(EnemyBase enemy)
        {
            for (int i = 0; i < mySkills.Count; i++)
                mySkills[i].StrikeTrigger(enemy);
        }
        /// <summary>
        /// 受击时
        /// </summary>

        public virtual void Trigger_UnStrike()
        {
            for (int i = 0; i < mySkills.Count; i++)
                mySkills[i].EventTrigger(SkillReplyType.UnStrike);
        }
        /// <summary>
        /// 杀敌时
        /// </summary>

        public virtual void Trigger_Kill()
        {

            for (int i = 0; i < mySkills.Count; i++)
                mySkills[i].EventTrigger(SkillReplyType.Kill);
        }

        /// <summary>
        /// 升级时
        /// </summary>
        public virtual void Trigger_LevelUp()
        {

            for (int i = 0; i < mySkills.Count; i++)
                mySkills[i].EventTrigger(SkillReplyType.LevelUp);
        }

        #endregion
    }
}
