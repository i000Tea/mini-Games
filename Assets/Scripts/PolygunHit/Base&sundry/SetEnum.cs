namespace Tea.PolygonHit
{
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
    public enum EventType
    {
        ShowText0,
        ShowText1,
        ShowText2,
        ShowText3,
        ShowText4,
        ShowText5,

        #region 行为事件
        /// <summary>
        /// 拖拽弹射时
        /// </summary>
        action_Shoot,
        /// <summary>
        /// 撞击时
        /// </summary>
        action_Strike,
        /// <summary>
        /// 受击时
        /// </summary>
        action_UnStrike,

        /// <summary>
        /// 击杀时
        /// </summary>
        action_Kill,

        /// <summary>
        /// 等级提升
        /// </summary>
        action_LevelUp,
        #endregion

        /// <summary>
        /// 玩家死亡
        /// </summary>
        PlayerDestory,
    }
}
