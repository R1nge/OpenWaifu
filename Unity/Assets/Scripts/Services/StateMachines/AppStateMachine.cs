using System.Collections.Generic;

namespace Services.StateMachines
{
    public class AppStateMachine
    {
        private readonly Dictionary<AppStateType, IAppState> _states;
        private IAppState _currentAppState;
        private AppStateType _currentAppStateType;

        public AppStateMachine(AppStatesFactory appStatesFactory)
        {
            _states = new Dictionary<AppStateType, IAppState>
            {
                { AppStateType.Game, appStatesFactory.CreateAppState(this) }
            };
        }

        public void SwitchState(AppStateType appStateType)
        {
            _currentAppState?.Exit();
            _currentAppState = _states[appStateType];
            _currentAppStateType = appStateType;
            _currentAppState.Enter();
        }
    }
}