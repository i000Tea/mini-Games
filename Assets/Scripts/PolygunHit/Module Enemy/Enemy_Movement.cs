using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit.Enemy
{
   public class Enemy_Movement : Enemy_AddMode
   {
      #region variable     行动相关

      #region Movement
      Vector3 Target => Base.TargetTransform.position;
      [SerializeField]
      /// <summary>
      /// 移动和旋转目标(玩家)
      /// </summary>
      private Vector3 targetPoint;

      [Tooltip("移动速度")]
      public float moveSpeed;

      /// <summary>
      /// 与玩家之间的距离
      /// </summary>
      private float DistanceFromPlayer => Vector3.Distance(transform.position, Base.TargetTransform.position) * transform.lossyScale.x;

      /// <summary>
      /// 带有方向的基础速度
      /// </summary>
      private Vector2 BaseDirSpeed => 0.1f * moveSpeed * transform.up;
      /// <summary>
      /// 带有方向的输出速度
      /// </summary>
      private Vector2 OutputDirSpeed
      {
         get
         {
            var power = BaseDirSpeed;
            // 当与玩家的角度差大于15 算法降低移动速度
            if (DiffOfAngle > 15)
               power *= Mathf.Cos(DiffOfAngle / 75 * 1.57f);

            if (Base.State == EnemyState.Charge)
               power *= 0.75f;
            if (Base.State == EnemyState.Atk)
               power *= 1.6f;
            return power;
         }
      }

      #endregion

      #region rotate
      /// <summary>
      /// 旋转速度
      /// </summary>
      public float rotateSpeed;
      /// <summary>
      /// 与玩家之间的旋转角度差
      /// </summary>
      float DiffOfAngle;
      /// <summary>
      /// 旋转参数
      /// </summary>
      Vector3 dir;
      /// <summary>
      /// 旋转目标Z值
      /// </summary>
      float angle;
      /// <summary>
      /// 旋转目标
      /// </summary>
      Quaternion rotateTarget;

      #endregion

      #endregion

      #region void         移动
      Coroutine movementCor;
      /// <summary>
      /// 停止时的距离
      /// </summary>
      private float _CloseDistance;
      protected override void Initialize()
      {
         movementCor = StartCoroutine(MovementUpdate());
      }
      private void OnDestroy()
      {
         StopAllCoroutines();
      }
      private IEnumerator MovementUpdate()
      {
         yield return 1;
         while (true)
         {
            if (!Base.Movement)
            {
               yield return new WaitUntil(() => Base.Movement == true);
            }
            UpdateMove();
            UpdateRotate();
            yield return new WaitForFixedUpdate();
         }
      }
      /// <summary>
      /// 移动方法
      /// </summary>
      private void UpdateMove()
      {
         // 计算与玩家之间的距离
         //DistanceFromPlayer = Vector3.Distance(transform.position, PlayerBase.Player.position);

         //Debug.Log($"{DistanceFromPlayer}  {CameraController.inst.MaxEdgeDistance}");
         // 当与玩家间的距离大于视线外 倍速移动靠近玩家
         if (DistanceFromPlayer > CameraController.inst.MaxEdgeDistance)
         {
            transform.localPosition += (Vector3)BaseDirSpeed * Mathf.Pow(DistanceFromPlayer, 2.4f);
         }

         //将速度应用到刚体上
         m_Rig.velocity += OutputDirSpeed;
      }
      /// <summary>
      /// 旋转方法
      /// </summary>
      private void UpdateRotate()
      {
         // 计算目标与自身的向量差
         dir = Target - transform.position;
         // 向量Z归零
         dir.z = 0;
         // 返回有符号的角度
         angle = Vector3.SignedAngle(Vector3.up, dir, Vector3.forward);
         // 角度转四元数
         rotateTarget = Quaternion.Euler(0, 0, angle);

         // 设置 旋转角度差
         DiffOfAngle = Mathf.Abs(rotateTarget.eulerAngles.z - transform.localRotation.eulerAngles.z);

         // 定义旋转速度
         var speed = rotateSpeed * Time.deltaTime;

         //if (rotateDiff > 10)
         //{
         //    speed += rotateDiff;
         //}

         // 当旋转角度差极小时 直接赋值 否则 插值计算
         if (DiffOfAngle < 0.025f)
            transform.localRotation = rotateTarget;
         else
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotateTarget, speed);
      }

      protected override void OnEnterHitTarget(Collision2D collision)
      {
         _CloseDistance = DistanceFromPlayer;
         StartCoroutine(WaitRestartMovement());
      }
      /// <summary>
      /// 等待重新开始移动
      /// </summary>
      /// <returns></returns>
      IEnumerator WaitRestartMovement()
      {
         Base.Movement = false;
         yield return new WaitUntil(() => DistanceFromPlayer >= _CloseDistance * 1.05f);
         Base.Movement = true;
      }
      #endregion
   }
}