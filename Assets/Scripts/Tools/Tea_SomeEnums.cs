namespace Tea
{
   public enum EventType
   {
      /// <summary>
      /// 空
      /// </summary>
      none,
      /// <summary>
      /// 按钮
      /// </summary>
      Button,
      GameState,
   }
   public enum ButtonType
   {
      none,
      //============直接调用===============
      /// <summary> 退回主页 </summary>
      Exit = 1,
      /// <summary> 重新开始 </summary>
      Restart,

      //============按钮广播===============
      /// <summary> 开始游戏 </summary>
      Menu_StartGame = 11,

      Menu_Before,
      Menu_After,

      //============状态广播===============
      Gameing_Pause = 21,
      Gameing_Again,
   }

}
