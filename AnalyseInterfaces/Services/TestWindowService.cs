using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnalyseInterfaces.Services.Contracts;

namespace AnalyseInterfaces.Services
{
    public class TestWindowService : ITestWindowService
    {
        private readonly IServiceProvider _serviceProvider;

        public TestWindowService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show(Type windowType)
        {
            if (!typeof(Window).IsAssignableFrom(windowType))
                throw new InvalidOperationException($"La classe de fenêtre doit être dérivée de {typeof(Window)}.");

            var windowInstance = _serviceProvider.GetService(windowType) as Window;

            windowInstance?.Show();
        }

        public T Show<T>() where T : class
        {
            if (!typeof(Window).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException($"La classe de fenêtre doit être dérivée de {typeof(Window)}.");

            var windowInstance = _serviceProvider.GetService(typeof(T)) as Window;

            if (windowInstance == null)
                throw new InvalidOperationException("La fenêtre n'est pas enregistrée en tant que service.");

            windowInstance.Show();

            return (T)Convert.ChangeType(windowInstance, typeof(T));
        }
    }
}