using System;


namespace AnalyseInterfaces.Services.Contracts
{
    public interface ITestWindowService
    {
        public void Show(Type windowType);

        public T Show<T>() where T : class;
    }
}
