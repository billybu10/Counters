using CommunityToolkit.Mvvm.Input;
using Counters.Models;
using Counters.ViewModels;
using Counters.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Counters.ViewModels;

internal class CountersViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.CounterViewModel> AllCounters { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectCounterCommand { get; }

    public CountersViewModel()
    {
        AllCounters = new ObservableCollection<ViewModels.CounterViewModel>(Models.Counter.LoadAll().Select(n => new CounterViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewCounterAsync);
        SelectCounterCommand = new AsyncRelayCommand<ViewModels.CounterViewModel>(SelectCounterAsync);
    }

    private async Task NewCounterAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.CounterPage));
    }

    private async Task SelectCounterAsync(ViewModels.CounterViewModel counter)
    {
        if (counter != null)
            await Shell.Current.GoToAsync($"{nameof(Views.CounterPage)}?load={counter.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string counterId = query["deleted"].ToString();
            CounterViewModel matchedCounter = AllCounters.Where((n) => n.Identifier == counterId).FirstOrDefault();

            if (matchedCounter != null)
                AllCounters.Remove(matchedCounter);
        }
        else if (query.ContainsKey("saved"))
        {
            string counterId = query["saved"].ToString();
            CounterViewModel matchedCounter = AllCounters.Where((n) => n.Identifier == counterId).FirstOrDefault();


            if (matchedCounter != null)
            {
                matchedCounter.Reload();
            }

            else
                AllCounters.Insert(0, new CounterViewModel(Models.Counter.Load(counterId)));
        }
    }
}