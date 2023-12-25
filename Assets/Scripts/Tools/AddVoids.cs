using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using Tea.PolygonHit;
using Tea.NewRouge;

namespace Tea
{
   public static class AddVoids
   {
      /// <summary>
      /// 图像在一段时间内闪烁一次
      /// </summary>
      /// <param name="targetImage"></param>
      /// <param name="Start"></param>
      /// <param name="End"></param>
      public static void ColorFlicker(this Image targetImage, Color Start, Color End,
          float flickerTime = 0.3f)
      {
         targetImage.DOColor(Start, flickerTime / 2).OnComplete(() => targetImage.DOColor(End, flickerTime / 2));
      }

      #region List
      /// <summary>
      /// 提取列表与目标点距离最小的一个物体
      /// </summary>
      /// <param name="_List"></param>
      /// <param name="target"></param>
      /// <param name="_long">可传入最大范围</param>
      /// <returns></returns>
      public static GameObject ListMin(List<GameObject> _List, Vector3 target, float _long = 999999)
      {
         // 新建物体和最大长度
         GameObject targetObj = null;
         float longTarget;
         // 循环
         for (int i = 0; i < _List.Count; i++)
         {
            longTarget = Vector3.Distance(_List[i].transform.position, target);
            if (longTarget < _long)
            {
               targetObj = _List[i];
               _long = longTarget;
            }
         }
         // 返回物体
         return targetObj;
      }

      /// <summary>
      /// 提取列表中与目标小于一定距离的所有物体
      /// </summary>
      /// <param name="_List"></param>
      /// <param name="traget"></param>
      /// <param name="_long"></param>
      /// <returns></returns>
      public static GameObject[] ListDistance(List<GameObject> _List, Vector3 target, float _long)
      {
         // 新建数列
         var newList = new List<GameObject>();
         // 循环添加
         for (int i = 0; i < _List.Count; i++)
            if (Vector3.Distance(_List[i].transform.position, target) < _long)
               newList.Add(_List[i]);
         // 返回数组
         return newList.ToArray();
      }

      /// <summary>
      /// 列表随机
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="list"></param>
      /// <param name="Seed"></param>
      /// <returns></returns>
      public static T RandomListValue<T>(this List<T> list, float Seed = -1)
      {
         if (list == null | list.Count <= 0)
            return default(T);
         int num = -1;
         if (Seed == -1)
            num = UnityEngine.Random.Range(0, list.Count);
         //Debug.Log($"{list.Count} + {num}");
         return list[num];
      }

      /// <summary>
      /// 生成子物体集
      /// </summary>
      /// <returns></returns>
      public static void CreateChildList(this Transform parent, ref List<GameObject> _list)
      {
         if (_list == null || _list.Count != parent.childCount)
         {
            _list = new List<GameObject>();

            for (int i = 0; i < parent.childCount; i++)
            {
               _list.Add(parent.GetChild(i).gameObject);
            }
         }
      }

      /// <summary>
      /// 将数锁定在列表中
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="num"></param>
      /// <param name="targetList"></param>
      /// <returns></returns>
      public static void NumInTheList<T>(this List<T> targetList, ref int num)
      {
         if (num >= targetList.Count)
            num -= targetList.Count;
         else if (num < 0)
            num += targetList.Count;
      }

      /// <summary>
      /// 对带有权重的枚举列表进行随机
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="someType"></param>
      /// <returns></returns>
      public static T GetRandomThrown<T>(this List<T> someType) where T : Weight
      {
         var totalWeight = someType.Sum(item => item.weight);
         var randomValue = UnityEngine.Random.value * totalWeight;

         foreach (var enumWithWeight in someType)
         {
            randomValue -= enumWithWeight.weight;
            if (randomValue <= 0)
            {
               return enumWithWeight;
            }
         }
         // 如果出现异常情况，返回第一个枚举值
         return someType[0];
      }
      #endregion

      public static int GetEnumItemCount<T>() where T : Enum
      {
         T[] values = (T[])Enum.GetValues(typeof(T));
         return values.Length;
      }
      public static T[] GetEnumItem<T>() where T : Enum
      {
         T[] values = (T[])Enum.GetValues(typeof(T));
         return values;
      }

      #region Game1
      /// <summary>
      /// 在画布上生成物体
      /// </summary>
      /// <param name="prefab"></param>
      /// <returns></returns>
      public static GameObject CreateObjInCanvas(this GameObject prefab, RectTransform rect, float inputLocalScale = 1)
      {
         var obj = GameObject.Instantiate(prefab);
         obj.transform.SetParent(rect);
         obj.transform.localScale = Vector3.one * inputLocalScale;
         return obj;
      }
      public static UnCollision SetUnColl(float Power, Vector3 Target)
      {
         UnCollision data = new UnCollision();
         data.Power = Power;
         data.Target = Target;
         return data;
      }

      /// <summary>
      /// 需要连带命名空间一同输入
      /// </summary>
      /// <param name="fullClassName"></param>
      /// <returns></returns>
      public static Type GetClass(this string fullClassName)
      {
         // 使用反射获取对应类的类型
         Type classType = Type.GetType(fullClassName);
         if (classType != null)
         {
            // 创建对应的类实例
            object instance = Activator.CreateInstance(classType);
            Type GetType = Activator.CreateInstance(classType) as Type;
            return instance.GetType();
         }
         else
         {
            Debug.LogWarning("无法找到对应的默认类" + fullClassName);
            return null;
         }
      }
      public static String GetStringFromJson(this string jsonField)
      {
         return jsonField;
      }
      /// <summary>
      /// 输入类的名字 返回技能
      /// </summary>
      /// <param name="className"></param>
      /// <returns></returns>
      public static ISkill CreateSkillFromString(this string className, string @namespace = "Tea")
      {
         string fullName = @namespace + "." + className;
         // 使用反射获取对应类的类型
         Type classType = fullName.GetClass();
         Debug.Log(classType);

         if (classType != null)
         {
            // 创建对应的类实例
            ISkill GetSkill = Activator.CreateInstance(classType) as ISkill;
            return GetSkill;
         }
         else
         {
            Debug.LogWarning("无法找到对应的技能类" + fullName);
            return null;
         }
      }
      public static Sprite GetSkillImage(this string fileName)
      {
         // 构建完整的资源路径
         string resourcePath = "Images/" + fileName; // 这里假设图片存储在"Resources/Images/"文件夹下
                                                     // 加载图片资源
         Sprite image = Resources.Load<Sprite>(resourcePath);

         return image;
      }

      /// <summary>
      /// 添加一个buff
      /// </summary>
      /// <param name="effectObj"></param>
      public static void AddBuff(this List<IBuff> effectObj, IBuff someBuff)
      {
         effectObj.Add(someBuff);
      }
      public static IBuff GetBuffFromString(this string className, string @namespace = "Tea")
      {
         // 使用反射获取对应类的类型
         Type classType = GetClass(@namespace + "." + className);
         if (classType != null)
         {
            // 创建对应的类实例
            IBuff GetBuff = Activator.CreateInstance(classType) as IBuff;
            return GetBuff;
         }
         else
         {
            Debug.LogWarning("无法找到对应的增益类" + className);
            return null;
         }
      }

      #endregion

      #region Game2

      #endregion

      #region Game3

      /// <summary>
      /// 将dir转化为v3
      /// </summary>
      /// <param name="dir"></param>
      /// <returns></returns>
      public static Vector3 DirToPoint(this BreakThroughWall.MoveDirection dir)
      {
         switch (dir)
         {
            case BreakThroughWall.MoveDirection.up:
               return Vector3.up;
            case BreakThroughWall.MoveDirection.down:
               return Vector3.down;
            case BreakThroughWall.MoveDirection.left:
               return Vector3.left;
            case BreakThroughWall.MoveDirection.right:
               return Vector3.right;
            default:
               break;
         }
         return default;
      }
      #endregion

      #region Anther

      /// <summary>
      /// 加载环
      /// </summary>
      /// <param name="targetImage">图片</param>
      /// <param name="isAdd">是否加载</param>
      /// <returns>加载是否完成</returns>
      public static bool LoadingRim(this Image targetImage, bool isAdd)
      {
         if (targetImage.fillAmount == 1)
            return true;
         if (isAdd)
            targetImage.fillAmount += Time.deltaTime * PlayerMove.inst.RimRate;
         else
            targetImage.fillAmount -= Time.deltaTime;

         return false;
      }

      /// <summary>
      /// 随机获取枚举值
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public static T RandomEnum<T>()
      {
         T[] results = Enum.GetValues(typeof(T)) as T[];
         T result = results[UnityEngine.Random.Range(0, results.Length)];
         return result;
      }

      /// <summary>
      /// 经过角度转换后的V3
      /// </summary>
      /// <param name="beforePoint"></param>
      /// <param name="angle"></param>
      /// <returns></returns>
      public static Vector3 AngleTransfor(this Vector3 beforePoint, float angle)
      {
         Vector3 a = new Vector3(
             beforePoint.x * Mathf.Cos(angle * Mathf.Deg2Rad) +
             beforePoint.z * Mathf.Sin(angle * Mathf.Deg2Rad),
             beforePoint.y,
             beforePoint.x * -Mathf.Sin(angle * Mathf.Deg2Rad) +
             beforePoint.z * Mathf.Cos(angle * Mathf.Deg2Rad)
             );

         return a;
      }

      #endregion
   }
}
/// <summary>
/// 受撞击的数据(撞击力度和撞击点)
/// </summary>
[System.Serializable]
public class UnCollision
{
   public float Power;
   [HideInInspector]
   public Vector3 Target;
}
public class Weight
{
   [Range(0, 1f)] public float weight = 1f;
}