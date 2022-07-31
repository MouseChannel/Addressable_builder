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
        public static string LoadPath;

        public const string BuildText = "BuildText";
        public const string LoadText = "LoadText";
    }
}