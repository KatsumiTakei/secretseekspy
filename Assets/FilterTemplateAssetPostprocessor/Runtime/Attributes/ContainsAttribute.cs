using System;
using UnityEngine;

namespace FilterTemplateAssetPostprocessor.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ContainsAttribute : PropertyAttribute
    {
        public ContainsAttribute(string value, string message)
        {
            this.Value = value;
            this.Message = message;
        }

        public string Value { get; }
        
        public string Message { get; }
        
        public bool IsValid { get; set; }
    }
}