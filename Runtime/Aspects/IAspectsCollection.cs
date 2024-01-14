namespace Qw1nt.MorpehStartup.Aspects
{
    public interface IAspectsCollection
    {
        AspectsCollection Add(EcsAspectBase aspectBase);

        AspectsCollection Add<T>() where T : EcsAspectBase, new();
    }
}