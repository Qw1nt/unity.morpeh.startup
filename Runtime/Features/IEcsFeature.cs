using Qw1nt.MorpehStartup.Aspects;

namespace Qw1nt.MorpehStartup.Features
{
    public interface IEcsFeature
    {
        void Build(IAspectsCollection aspects);
    }
}