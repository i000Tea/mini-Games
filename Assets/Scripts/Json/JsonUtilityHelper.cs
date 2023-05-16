using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace Tea
{
   /// <summary>
   /// Json实用助手
   /// </summary>
   public static class JsonUtilityHelper
   {
      /// <summary>
      /// 将ScriptableObject转换为JSON字符串
      /// </summary>
      /// <param name="scriptableObject"></param>
      /// <returns></returns>
      public static string ToJson(ScriptableObject scriptableObject)
      {
         return JsonUtility.ToJson(scriptableObject);
      }

      /// <summary>
      /// 将JSON字符串转换为ScriptableObject
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="json"></param>
      /// <returns></returns>
      public static T FromJson<T>(string json) where T : ScriptableObject
      {
         return JsonUtility.FromJson<T>(json);
      }

      /// <summary>
      /// 从文件中加载JSON并转换为ScriptableObject
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="filePath"></param>
      /// <returns></returns>
      public static T LoadFromJsonFile<T>(string filePath) where T : ScriptableObject
      {
         if (File.Exists(filePath))
         {
            string json = File.ReadAllText(filePath);
            return FromJson<T>(json);
         }
         else
         {
            Debug.LogError("File not found: " + filePath);
            return null;
         }
      }

      /// <summary>
      /// 将ScriptableObject保存为JSON文件
      /// </summary>
      /// <param name="scriptableObject">序列化文件</param>
      /// <param name="filePath">地址</param>
      public static void SaveToJsonFile(ScriptableObject scriptableObject, string filePath)
      {
         // 格式化JSON字符串
         string json = JsonConvert.SerializeObject(scriptableObject, Formatting.Indented);

         File.WriteAllText(filePath, json);
      }
   }
}