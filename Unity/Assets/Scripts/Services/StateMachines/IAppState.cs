namespace Services.StateMachines
{
    public interface IAppState : IExitState
    {
        void Enter();
    }

    public interface IExitState
    {
        void Exit();
    }
}