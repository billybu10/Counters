using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Counters.ViewModels;

internal class CounterViewModel : ObservableObject, IQueryAttributable
{
    private Models.Counter _counter;

    public string Text
    {
        get => _counter.Name;
        set
        {
            if (_counter.Name != value)
            {
                _counter.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public int Value
    {
        get => _counter.Value;
        set
        {
            if (_counter.Value != value)
            {
                _counter.Value = value;
                OnPropertyChanged();
            }
        }
    }

    public string Identifier => _counter.Filename;

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    public ICommand ResetCommand { get; private set; }
    public ICommand IncrementCommand { get; private set; }
    public ICommand DecrementCommand { get; private set; }


    public CounterViewModel()
    {
        _counter = new Models.Counter();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
        ResetCommand = new AsyncRelayCommand(Reset);
        IncrementCommand = new AsyncRelayCommand(Increment);
        DecrementCommand = new AsyncRelayCommand(Decrement);
    }

    public CounterViewModel(Models.Counter counter)
    {
        _counter = counter;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
        ResetCommand = new AsyncRelayCommand(Reset);
        IncrementCommand = new AsyncRelayCommand(Increment);
        DecrementCommand = new AsyncRelayCommand(Decrement);
    }

    private async Task Save()
    {
        _counter.Save();
        await Shell.Current.GoToAsync($"..?saved={_counter.Filename}");
    }

    private async Task Delete()
    {
        _counter.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_counter.Filename}");
    }

    private async Task Reset()
    {
        _counter.Value = 0;
        _counter.Save();
        await Shell.Current.GoToAsync($"..?saved={_counter.Filename}");
    }

    private async Task Increment()
    {
        _counter.Value++;
        _counter.Save();
        await Shell.Current.GoToAsync($"..?saved={_counter.Filename}");
    }

    private async Task Decrement()
    {
        _counter.Value--;
        _counter.Save();
        await Shell.Current.GoToAsync($"..?saved={_counter.Filename}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _counter = Models.Counter.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _counter = Models.Counter.Load(_counter.Filename);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Value));
    }
}