using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tea.PolygonHit
{
   /// <summary>
   /// 技能基类
   /// </summary>
   [System.Serializable]
   public class SkillBase :MonoBehaviour
   {
      #region 变量
      [SerializeField]
      Text costText;
      /// <summary>
      /// 技能需求值
      /// </summary>
      public int needSp = 3;
      /// <summary>
      /// 当前技能值
      /// </summary>
      private float Sp
      {
         get
         {
            return sp;
         }
         set
         {
            sp = value;
            if (sp > needSp)
               sp = needSp;
            UIUpdate();
         }
      }
      /// <summary>
      /// 不做更改
      /// </summary>
      private float sp;

      [Tooltip("回复方式")]
      [SerializeField]
      private SkillReplyType m_ReplyType;
      [Tooltip("使用方式")]
      [SerializeField]
      private SkillUseType m_UseType;
      #endregion

      private void OnValidate()
      {
         if (!costText)
         {
            //costText = transform.GetChild(1).GetComponent<Text>();
         }
      }
      private void Start()
      {
         if (needSp <= 0 && m_ReplyType != SkillReplyType.none)
         {
            needSp = 1;
         }
         Sp = 0;
      }
      private void FixedUpdate()
      {
         if (m_ReplyType == SkillReplyType.Time)
            Sp += Time.deltaTime;
      }

      /// <summary>
      /// UI更新
      /// </summary>
      void UIUpdate()
      {
         costText.text = (int)Sp + "/" + needSp;
      }

      /// <summary>
      /// 通过事件触发 增加sp
      /// </summary>
      public void EventTrigger(SkillReplyType inputType, EnemyBase enemy = null)
      {
         // 未释放技能时 才增加sp
         if (inputType == m_ReplyType)
            Sp++;
      }

      /// <summary>
      /// 通过撞击事件触发 额外传入敌人的脚本
      /// </summary>
      public void StrikeTrigger(EnemyBase enemy)
      {
         // 未释放技能时 才增加sp
         if (!CompareSkill(SkillUseType.intensifyStrike, enemy) &&
             m_ReplyType == SkillReplyType.Strike)
            Sp++;
      }

      /// <summary>
      /// 检查技能是否可以使用 
      /// 不可用 返回false
      /// 可用 则返回true 使用技能 sp重置
      /// </summary>
      /// <param name="inputType"></param>
      /// <returns></returns>
      public bool CompareSkill(SkillUseType inputType, EnemyBase enemy = null)
      {
         // 事件传入的类型 与 自身触发类型 不一致 打回
         if (inputType != m_UseType)
         {
            return false;
         }
         // 技能需求值不足 且 回复方式不为空时 打回
         else if (sp < needSp && m_ReplyType != SkillReplyType.none)
         {
            return false;
         }
         // 满足条件 释放 返回真
         else
         {
            Sp = 0;
            UIUpdate();
            if (enemy)
               UseStrikeSkill(enemy);
            else
               UseSkill();
            return true;
         }
      }

      /// <summary>
      /// 不需要检测敌人的 使用技能的方法
      /// </summary>
      public virtual void UseSkill() { }

      /// <summary>
      /// 需要检测敌人的 使用技能的方法
      /// </summary>
      public virtual void UseStrikeSkill(EnemyBase enemy) { }

   }
}
