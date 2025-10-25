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
            GameProfileSession gameSession) : base(viewFactory, navigator)
        {
            _gameSession = gameSession;
        }
        protected override void Subscribe()
        {
            OnCoffeeRevenueChanged(_gameSession.Revenue);
            OnCoffeeReputationChanged(_gameSession.Reputation);
            _gameSession.RevenueChanged += OnCoffeeRevenueChanged;
            _gameSession.ReputationChanged += OnCoffeeReputationChanged;
        }
        protected override void Unsubscribe()
        {
            _gameSession.RevenueChanged -= OnCoffeeRevenueChanged;
            _gameSession.ReputationChanged -= OnCoffeeReputationChanged;
        }

        private readonly GameProfileSession _gameSession;
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