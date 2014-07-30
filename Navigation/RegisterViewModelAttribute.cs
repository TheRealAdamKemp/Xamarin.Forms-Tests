using System;

namespace Navigation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterViewModelAttribute : Attribute
    {
        public Type ViewModelType { get; private set; }

        public RegisterViewModelAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }
    }
}

