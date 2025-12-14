using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using YelpAIDemo.Core.Models;

namespace YelpAIDemo.Core.Services;

public class AppState : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public Coordinates? UserLocation { get; set => SetField(ref field, value); }
    public List<YelpAiResponse?> RecentYelpAiResponses { get; } = [];
    public YelpAiResponse? LatestYelpAiResponse
    {
        get;
        set
        {
            if (RecentYelpAiResponses.Count > 10)
            {
                RecentYelpAiResponses.RemoveAt(0);
            }
            RecentYelpAiResponses.Add(value);
            SetField(ref field, value);
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}