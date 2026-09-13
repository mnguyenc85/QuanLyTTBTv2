using System;
using Avalonia;
using Avalonia.Controls;

namespace QuanLyTTBTv2.Controls;

public partial class DateTimeFilter : UserControl
{
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DateTimeFilter, string?>(
            nameof(Title));

    public static readonly StyledProperty<bool> UsedProperty =
        AvaloniaProperty.Register<DateTimeFilter, bool>(
            nameof(Used));

    public static readonly StyledProperty<DateTime?> DateTimeProperty =
        AvaloniaProperty.Register<DateTimeFilter, DateTime?>(
            nameof(DateTime));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public bool Used
    {
        get => GetValue(UsedProperty);
        set => SetValue(UsedProperty, value);
    }

    public DateTime? DateTime
    {
        get => GetValue(DateTimeProperty);
        set => SetValue(DateTimeProperty, value);
    }

    private bool _updating;

    public DateTimeFilter()
    {
        InitializeComponent();

        DatePicker.PropertyChanged += DatePicker_PropertyChanged;
        TimePicker.PropertyChanged += TimePicker_PropertyChanged;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DateTimeProperty)
        {
            UpdateControlsFromDateTime();
        }
    }

    private void DatePicker_PropertyChanged(
        object? sender,
        AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == CalendarDatePicker.SelectedDateProperty)
        {
            UpdateDateTimeFromControls();
        }
    }

    private void TimePicker_PropertyChanged(
        object? sender,
        AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == TimePicker.SelectedTimeProperty)
        {
            UpdateDateTimeFromControls();
        }
    }

    private void UpdateDateTimeFromControls()
    {
        if (_updating)
            return;

        var date = DatePicker.SelectedDate;
        var time = TimePicker.SelectedTime;

        if (date is null)
        {
            DateTime = null;
            return;
        }

        var dateValue = date.Value.Date;

        if (time.HasValue)
            dateValue = dateValue.Add(time.Value);

        DateTime = dateValue;
    }

    private void UpdateControlsFromDateTime()
    {
        if (_updating)
            return;

        _updating = true;

        try
        {
            if (DateTime.HasValue)
            {
                var value = DateTime.Value;

                DatePicker.SelectedDate = value.Date;
                TimePicker.SelectedTime = value.TimeOfDay;
            }
            else
            {
                DatePicker.SelectedDate = null;
                TimePicker.SelectedTime = null;
            }
        }
        finally
        {
            _updating = false;
        }
    }
    
}