using System;

namespace AkkaLifecycleAndSupervision
{
    public class MyCustomException : Exception
    {
        public MyCustomException(string exception) : base(exception)
        {
            
        }
    }
}