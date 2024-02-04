namespace Services.StateMachines.States
{
    public class AppState : IAppState
    {
        private readonly AppStateMachine _stateMachine;

        public AppState(AppStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Enter() { }

        public void Exit() { }
    }
}