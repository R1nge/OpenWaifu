using Services.StateMachines.States;

namespace Services.StateMachines
{
    public class AppStatesFactory
    {
        public IAppState CreateAppState(AppStateMachine stateMachine)
        {
            //TODO: loading data from configs state, app loaded state (switch to gpt scene)
            return new AppState(stateMachine);
        }
    }
}