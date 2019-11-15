using AlertApp.Services.Alert;
using AlertApp.Services.Community;
using AlertApp.Services.Contacts;
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
            RegisterSingleton<IAlertService, AlertService>();
            RegisterSingleton<IContactsService, ContactsService>();


            // View models
            _unityContainer.RegisterType<SelectLanguagePageViewModel>();
            _unityContainer.RegisterType<EnterMobileNumberPageViewModel>();
            _unityContainer.RegisterType<EnterActivationCodePageViewModel>();
            _unityContainer.RegisterType<EnterApplicationPinCodePageViewModel>();
            _unityContainer.RegisterType<RegistrationFieldsPageViewModel>();
            _unityContainer.RegisterType<SendingAlertPageViewModel>();
            _unityContainer.RegisterType<MainPageViewModel>();
            _unityContainer.RegisterType<DialogSelectLanguageViewModel>();
            _unityContainer.RegisterType<MyCommunityPageViewModel>();
            _unityContainer.RegisterType<DependandsPageViewModel>();            
            _unityContainer.RegisterType<MainTabbedPageViewModel>();
            _unityContainer.RegisterType<SettingsPageViewModel>();
            _unityContainer.RegisterType<AddContactPageViewModel>();
            _unityContainer.RegisterType<CommunityRequestPageViewModel>();
            _unityContainer.RegisterType<ManageContactsPageViewModel>();
            _unityContainer.RegisterType<WhoAlertsMePageViewModel>();
            _unityContainer.RegisterType<AlertRespondPageViewModel>();
            _unityContainer.RegisterType<BlockedUsersPageViewModel>();
            _unityContainer.RegisterType<SettingContainerPageViewModel>();
            _unityContainer.RegisterType<SettingsChangePinViewModel>();            
            _unityContainer.RegisterType<SettingsAccountHistoryViewModel>();

        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }
        public T Resolve<T>(Dictionary<string,object> param)
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
