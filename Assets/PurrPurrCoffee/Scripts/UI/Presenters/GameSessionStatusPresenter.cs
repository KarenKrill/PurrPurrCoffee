using KarenKrill.Diagnostics.Abstractions;
using KarenKrill.UI.Presenters.Abstractions;
using KarenKrill.UI.Views.Abstractions;

namespace PurrPurrCoffee.UI.Presenters
{
    using PurrPurrCoffee.Abstractions;
    using Views.Abstractions;

    public class GameSessionStatusPresenter : PresenterBase<IGameSessionStatusView>, IPresenter<IGameSessionStatusView>
    {
        public GameSessionStatusPresenter(IViewFactory viewFactory,
            IPresenterNavigator navigator,
            GameSession gameSession) : base(viewFactory, navigator)
        {
            _gameSession = gameSession;
        }
        protected override void Subscribe()
        {
            OnCoffeeRevenueChanged(_gameSession.CoffeeRrevenue);
            OnCoffeeReputationChanged(_gameSession.CoffeeReputation);
            _gameSession.CoffeeRevenueChanged += OnCoffeeRevenueChanged;
            _gameSession.CoffeeReputationChanged += OnCoffeeReputationChanged;
        }
        protected override void Unsubscribe()
        {
            _gameSession.CoffeeRevenueChanged -= OnCoffeeRevenueChanged;
            _gameSession.CoffeeReputationChanged -= OnCoffeeReputationChanged;
        }

        private readonly GameSession _gameSession;
        private void OnCoffeeReputationChanged(float reputation)
        {
            View.Reputation = $"Репутация: {reputation:0.0}";// ✫";
        }
        private void OnCoffeeRevenueChanged(float revenue)
        {
            View.Money = $"Выручка: {revenue:0.0} $";
        }

    }
}