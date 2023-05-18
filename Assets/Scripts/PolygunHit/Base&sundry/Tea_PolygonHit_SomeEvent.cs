namespace Tea.PolygonHit
{
   #region Buff & Skill

   /// <summary>
   /// 数值 修改 生效 阶段
   /// </summary>
   public enum ValueAlterEffectPhase
   {
      none,
      /// <summary> 
      /// 基础增加 <br/>
      /// 如果在这里尽可能用加法
      /// </summary>
      BaseAdd = 1,
      /// <summary> 
      /// 叠加乘算 <br/>
      /// 如果在这里尽可能用乘法
      /// </summary>
      StackMulti = 5,
      /// <summary> 
      /// 最终增加 <br/>
      /// 如果在这里尽可能用加法
      /// </summary>
      FinalAdd = 11,
   }


   /// <summary>
   /// 技能恢复sp值的方式
   /// </summary>
   public enum SkillReplyType
   {
      none,
      /// <summary> 撞击 </summary>
      Strike,
      /// <summary> 受击 </summary>
      UnStrike,
      /// <summary> 杀敌 </summary>
      Kill,
      /// <summary> 时间恢复 </summary>
      Time,
      /// <summary> 升级 </summary>
      LevelUp,
   }
   /// <summary>
   /// 技能使用的方式
   /// </summary>
   public enum SkillUseType
   {
      /// <summary>
      /// 被动技能
      /// </summary>
      passive,
      /// <summary>
      /// 开始弹射时
      /// </summary>
      IsShoot,
      /// <summary>
      /// 强化撞击
      /// </summary>
      intensifyStrike,
      /// <summary>
      /// 主动按钮使用
      /// </summary>
      activeButton,
   }

   public enum BuffTarget
   {
      player,
      enemy,
      enemy_Boss,
   }
   #endregion
   /// <summary>
   /// 敌人状态
   /// </summary>
   public enum EnemyState
   {
      /// <summary> 默认 向玩家靠近 </summary>
      Default,
      /// <summary> 蓄力 </summary>
      Charge,
      /// <summary> 攻击 </summary>
      Atk,
      /// <summary> 攻击后冷却 </summary>
      CD,
   }

   /// <summary>
   /// 敌人攻击模组
   /// </summary>
   public enum EnemyAttackMode
   {
      /// <summary>
      /// 默认 近战
      /// </summary>
      Melee,

      speedUp,
      /// <summary>
      /// 蓄力发射
      /// </summary>
      Charge,
   }
   /// <summary> 
   /// 游戏状态 
   /// </summary>
   public enum GameState
   {
      /// <summary> 游戏中 </summary>
      Gameing,
      /// <summary> 升级 </summary>
      LevelUp,
      /// <summary> 暂停 </summary>
      Pause,
      /// <summary> 游戏结束 </summary>
      Over,
   }
   /// <summary>
   /// 行动
   /// </summary>
   public enum ActionType
   {
      /// <summary> 拖拽弹射时<br/>无参 </summary>
      Shoot,
      /// <summary> 撞击时<br/>一个参数为撞击对象 </summary>
      Strike,
      /// <summary> 受击时 </summary>
      UnStrike,
      /// <summary> 击杀时 </summary>
      Kill,
      /// <summary> 获取经验 </summary>
      ExpAdd,
      /// <summary> 等级提升 </summary>
      LevelUp,
      /// <summary> 玩家死亡 无参 </summary>
      PlayerDestory,
   }
}
