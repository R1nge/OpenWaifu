using GPT;
using VContainer;
using VContainer.Unity;

namespace LifeCycle
{
    public class ChatLifetime : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<MicrophoneRecorder>(Lifetime.Singleton).AsSelf();
        }
    }
}