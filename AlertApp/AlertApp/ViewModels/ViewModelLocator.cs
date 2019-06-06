using AlertApp.Services.Community;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Profile;
using AlertApp.Services.Registration;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Resolution;

namespace AlertApp.ViewModels
{
    public class ViewModelLocator
    {
        private readonly IUnityContainer _unityContainer;

        private static readonly ViewModelLocator _instance = new ViewModelLocator();

        public static ViewModelLocator Instance
        {
            get
            {
                return _instance;
            }
        }
        protected ViewModelLocator()
        {
            _unityContainer = new UnityContainer();

            // Providers
            //_unityContainer.RegisterType<IRequestProvider, RequestProvider>();
            //_unityContainer.RegisterType<ILocationProvider, LocationProvider>();
            

            // Services          
            RegisterSingleton<ILocalSettingsService, LocalSettingsService>();
            RegisterSingleton<ICryptographyService, CryptographyService>();
            

            // Data services
            RegisterSingleton<IRegistrationService, RegistrationService>();
            RegisterSingleton<IUserProfileService, UserProfileService>();
            RegisterSingleton<ICommunityService, CommunityService>();
            

            // View models
            _unityContainer.RegisterType<SelectLanguagePageModel>();
            _unityContainer.RegisterType<EnterMobileNumberPageModel>();
            _unityContainer.RegisterType<EnterActivationCodePageModel>();
            _unityContainer.RegisterType<EnterApplicationPinCodePageModel>();
            _unityContainer.RegisterType<RegistrationFieldsPageViewModel>();
            _unityContainer.RegisterType<SendingAlertPageViewModel>();
            _unityContainer.RegisterType<MainPageViewModel>();
            _unityContainer.RegisterType<DialogSelectLanguageViewModel>();
            _unityContainer.RegisterType<MyCommunityPageViewModel>();
            _unityContainer.RegisterType<DependandsPageViewModel>();            
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }
        public T Resolve<T>(Dictionary<string,string> param)
        {
            var parameters = new ParameterOverrides();
            foreach (var item in param)
            {
                parameters.Add(item.Key,item.Value);
            }            
            return _unityContainer.Resolve<T>(parameters.OnType<T>());

        }
        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public void Register<T>(T instance)
        {
            _unityContainer.RegisterInstance<T>(instance);
        }

        public void Register<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>();
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }

    }
}
