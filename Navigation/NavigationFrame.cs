using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Navigation
{
    public class NavigationFrame : INavigate
    {
        private static readonly Dictionary<Type,Type> ViewModelTypeToPageType;

        private readonly NavigationPage _navigationPage;

        static NavigationFrame()
        {
            ViewModelTypeToPageType = new Dictionary<Type, Type>();

            // This is the hacky way we have to get the list of assemblies in a PCL for now.
            // Hopefully Xamarin will expose Device.GetAssemblies() in a future version of Xamarin.Forms.
            var currentDomain = typeof(string).GetTypeInfo().Assembly.GetType("System.AppDomain").GetRuntimeProperty("CurrentDomain").GetMethod.Invoke(null, new object[] {});
            var getAssemblies = currentDomain.GetType().GetRuntimeMethod("GetAssemblies", new Type[]{ });
            var assemblies = getAssemblies.Invoke (currentDomain, new object[]{ }) as Assembly[];

            var allTypes = assemblies.SelectMany(a => a.DefinedTypes);
            var typesWithRegisterAttributes = allTypes
                .Select(t => new { TypeInfo = t, Attribute = t.GetCustomAttribute<RegisterViewModelAttribute>() })
                .Where(p => p.Attribute != null);
            foreach (var pair in typesWithRegisterAttributes)
            {
                if (!typeof(Page).GetTypeInfo().IsAssignableFrom(pair.TypeInfo))
                {
                    var message = string.Format(
                        "RegisterViewModelAttribute applied to a class ({0}) that is not a Page",
                        pair.TypeInfo.FullName);
                    throw new InvalidOperationException(message);
                }
                if (ViewModelTypeToPageType.ContainsKey(pair.Attribute.ViewModelType))
                {
                    var message = string.Format(
                        "Multiple Page types (new = {0}, previous = {1}) registered for the same view model type ({2})",
                        pair.TypeInfo.FullName,
                        ViewModelTypeToPageType[pair.Attribute.ViewModelType].FullName,
                        pair.Attribute.ViewModelType.FullName
                    );
                    throw new InvalidOperationException(message);
                }
                ViewModelTypeToPageType[pair.Attribute.ViewModelType] = pair.TypeInfo.AsType();
            }
        }

        public NavigationFrame(object rootViewModel)
        {
            _navigationPage = new NavigationPage(CreatePageForViewModel(rootViewModel));
            _navigationPage.Popped += (sender, e) => SetFrameReference(e.Page.BindingContext, null);
        }

        public Page Root { get { return _navigationPage; } }

        public void NavigateTo(object viewModel)
        {
            _navigationPage.PushAsync(CreatePageForViewModel(viewModel));
        }

        private Page CreatePageForViewModel(object viewModel)
        {
            Type newPageType = null;
            if (!ViewModelTypeToPageType.TryGetValue(viewModel.GetType(), out newPageType))
            {
                throw new ArgumentException("Trying to create a Page for an unrecognized view model type. Did you forget to use the RegisterViewModel attribute?");
            }
            var newPage = (Page)Activator.CreateInstance(ViewModelTypeToPageType[viewModel.GetType()]);
            newPage.BindingContext = viewModel;
            SetFrameReference(viewModel, this);

            return newPage;
        }

        private static void SetFrameReference(object viewModel, INavigate frame)
        {
            if (viewModel == null)
            {
                return;
            }

            var navigatedPage = viewModel as INavigatedPage;
            if (navigatedPage != null)
            {
                navigatedPage.NavigationFrame = frame;
            }
        }
    }
}

