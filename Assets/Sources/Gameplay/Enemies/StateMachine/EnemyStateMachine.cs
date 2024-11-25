using System;

namespace Assets.Sources.Gameplay.Enemies.StateMachine
{
    public class EnemyStateMachine : Services.StateMachine.StateMachine, IDisposable
    {
        public void Dispose()
        {
            foreach(Services.StateMachine.IExitableState state in States)
            {
                if (state is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}