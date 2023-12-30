
namespace Lighten
{
    public interface IStateMachine<T> where T : class
    {
        void AddState<STATE>(string name, STATE state) where STATE : IState<T>;
        void Start(T obj, string initialState);
        void Update(float elapsedTime);
        void ChangeState(string stateName);
    }
}
