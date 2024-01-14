using System.Runtime.CompilerServices;
using Scellecs.Morpeh;

namespace Qw1nt.MorpehStartup.Aspects
{
    public class SingleSystemEcsAspect<T> : EcsAspectBase
        where T : class, IInitializer, new()
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Init()
        {
            Add(new T());
        }
    }
}