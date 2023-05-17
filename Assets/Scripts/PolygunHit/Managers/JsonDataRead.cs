using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor;
using WeChatWASM;

namespace Tea
{
   public class JsonDataRead : Singleton<JsonDataRead>
   {
      /// <summary>
      /// 缓存json字符
      /// </summary>
      string cacheJsonData;
      /// <summary>
      /// 读取技能信息
      /// </summary>
      public AllSkillData LoadSkillData()
      {
         string filePath = Path.Combine(Application.streamingAssetsPath, "Skill/skill.json");
#if UNITY_ANDROID && !UNITY_EDITOR
        // 在Android平台上，使用UnityWebRequest来读取StreamingAssets文件夹中的内容
         Coroutine coroutine = StartCoroutine(ReadAndroidJSON(filePath));
//#elif UNITY_WEBGL
#else
         // 在其他平台上，直接使用File类来读取StreamingAssets文件夹中的内容
         ReadJSON(filePath);
#endif
         var data = ConvertingToData();
         cacheJsonData = default;
         if (!data)
         {
            Debug.LogWarning("数据读取异常");
         }
         return data;
      }

      private void ReadJSON(string filePath)
      {
         if (File.Exists(filePath))
         {
            string jsonContent = File.ReadAllText(filePath);
            cacheJsonData = jsonContent;
         }
         else
         {
            Debug.LogError($"在路径 [{filePath}] 上加载JSON文件失败:");
         }
      }
      private IEnumerator ReadAndroidJSON(string filePath)
      {
         // 使用UnityWebRequest来读取StreamingAssets文件夹中的内容
         using (var www = new UnityEngine.Networking.UnityWebRequest(filePath))
         {
            if (filePath.Contains("://"))
            {
               www.url = filePath;
            }
            else
            {
               www.url = "file://" + filePath;
            }

            www.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
               string jsonContent = www.downloadHandler.text;
               cacheJsonData = jsonContent;
            }
            else
            {
               Debug.LogError("在路径上加载JSON文件失败:" + filePath + "\n" + www.error);
            }
         }
      }

      /// <summary>
      /// 字符转化为数据集
      /// </summary>
      /// <returns></returns>
      public AllSkillData ConvertingToData()
      {
         // 将JSON字符串反序列化为ScriptableObject
         AllSkillData getSkillData = ScriptableObject.CreateInstance<AllSkillData>();
         //AllSkillData getSkillData = JsonConvert.DeserializeObject<AllSkillData>(cacheJsonData) as AllSkillData;

         JsonConvert.PopulateObject(cacheJsonData, getSkillData);

         // 处理ScriptableObject
         if (getSkillData != null)
         {
            return getSkillData;
         }
         else
         {
            return null;
         }
      }
   }
}
