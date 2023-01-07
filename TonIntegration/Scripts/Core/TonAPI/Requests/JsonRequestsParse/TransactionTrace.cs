using System;

namespace Assets.TonIntegration.Scripts.Core.TonAPI.Requests.JsonRequestsParse
{
    [Serializable]
    public class TransactionTrace
    {
        public Annotation[] annotations;
        public Child[] children;
    }
    
    [Serializable]
    public class Child
    {
        public Annotation[] annotations;
        public double input_value;
        public bool success;
    }
    
    [Serializable]
    public class Annotation
    {
        public AnnotationData data;
        public string name;
    }
    
    [Serializable]
    public class AnnotationData
    {
        public double balance_diff;
        public string text;
    }
}
