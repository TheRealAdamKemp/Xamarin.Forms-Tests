using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Navigation
{
    public class NavigationFrame : IViewModelNavigation
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
        }

        public Page Root { get { return _navigationPage; } }

        public async Task<object> PopAsync()
        {
            var currentViewModel = CurrentViewModel;
            await _navigationPage.PopAsync();;
            return currentViewModel;
        }

        public async Task<object> PopModalAsync()
        {
            var poppedPage = await _navigationPage.Navigation.PopModalAsync();
            return poppedPage.BindingContext;
        }

        public Task PopToRootAsync()
        {
            return _navigationPage.PopToRootAsync();
        }

        public Task PushAsync(object viewModel)
        {
             return _navigationPage.PushAsync(CreatePageForViewModel(viewModel));
        }

        public Task PushModalAsync(object viewModel)
        {
            return _navigationPage.Navigation.PushModalAsync(CreatePageForViewModel(viewModel));
        }

        public object CurrentViewModel
        {
            get
            {
                return _navigationPage.CurrentPage.BindingContext;
            }
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

        private static void SetFrameReference(object viewModel, IViewModelNavigation frame)
        {
            if (viewModel == null)
            {
                return;
            }

            var navigatedPage = viewModel as INavigatingViewModel;
            if (navigatedPage != null)
            {
                navigatedPage.ViewModelNavigation = frame;
            }
        }
    }
}

