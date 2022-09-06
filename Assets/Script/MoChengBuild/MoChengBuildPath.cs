using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoChengBuilder
{
    public static partial class MoChengBuildUtil
    {
        public static string BuildPath
        {
            get
            {
                return "LocalData/Res";
            }
        }
        public static string LoadPath
        {
            get
            {
                return "LocalData/Res";
            }
        }

        public const string BuildText = "BuildPath";
        public const string LoadText = "LoadPath";

        public static Dictionary<string, string> pathDic = new Dictionary<string, string>();

        public static void AddPath(string name, string path)
        {
            profileSettings.CreateValue(name, path);
        }
        public static void RemovePath(string path)
        {
            var res = profileSettings.GetValueByName(setting.activeProfileId, path);

            var res1 = profileSettings.EvaluateString(setting.activeProfileId, res);
            foreach (var i in profileSettings.GetAllVariableIds())
            {
                var e = profileSettings.GetValueById(setting.activeProfileId, i);
                if (e == path)
                {
                    profileSettings.RemoveValue(i);
                    return;
                }

            }


        }
        public static Dictionary<string, string> GetAllPath()
        {

            var allValueNames = profileSettings.GetVariableNames();
            pathDic.Clear();
            foreach (var i in allValueNames)
            {
                if (!pathDic.ContainsKey(i))
                {
                    var path = profileSettings.GetValueByName(setting.activeProfileId, i);
                    pathDic.TryAdd(i, path);
                }

            }



            return pathDic;
        }






    }
}