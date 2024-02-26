using VContainer;

namespace Qw1nt.MorpehStartup.Systems
{
    public interface IInjectableSystem
    {
        void InjectDependencies(IObjectResolver resolver);
    }
}
