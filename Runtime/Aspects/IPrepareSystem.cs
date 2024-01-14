using Cysharp.Threading.Tasks;

namespace Qw1nt.MorpehStartup.Aspects
{
    public interface IPrepareSystem
    {
        UniTask Prepare();
    }
}