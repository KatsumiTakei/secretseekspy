using System;
using UnityEngine;

namespace FilterTemplateAssetPostprocessor.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class NotEmptyAttribute : PropertyAttribute
    {
        public bool IsValid { get; set; }
    }
}