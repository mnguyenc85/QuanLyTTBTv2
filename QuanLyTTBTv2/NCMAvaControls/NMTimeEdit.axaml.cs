using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace NCMAvaControls;

public partial class NMTimeEdit : UserControl
{
    // Cờ chặn vòng lặp khi tự set lại Text bên trong code
    private bool _isUpdating;

    public NMTimeEdit()
    {
        InitializeComponent();

        TxtHour.TextChanged += OnTextChanged;
        TxtMinute.TextChanged += OnTextChanged;
        TxtSecond.TextChanged += OnTextChanged;
    }

    #region Public API (không MVVM, chỉ property + event thường)

    public event EventHandler<TimeSpan>? TimeChanged;

    public TimeSpan Time
    {
        get => new TimeSpan(GetVal(TxtHour), GetVal(TxtMinute), GetVal(TxtSecond));
        set
        {
            SetVal(TxtHour, Math.Clamp(value.Hours, 0, 23));
            SetVal(TxtMinute, Math.Clamp(value.Minutes, 0, 59));
            SetVal(TxtSecond, Math.Clamp(value.Seconds, 0, 59));
        }
    }

    #endregion

    #region Focus / Điều hướng phím

    private void OnGotFocus(object? sender, FocusChangedEventArgs e)
    {
        if (sender is TextBox box)
        {
            // box.SelectAll();   // bôi đen để dễ nhìn
            box.CaretIndex = 0; // nhưng luôn bắt đầu ghi từ vị trí đầu tiên
        }
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox box) return;

        switch (e.Key)
        {
            case Key.Right:
                MoveNext(box);
                e.Handled = true;
                return;

            case Key.Left:
                MovePrev(box);
                e.Handled = true;
                return;

            case Key.OemSemicolon: // phím ':' (Shift + ;) trên layout phổ biến
                MoveNext(box);
                e.Handled = true;
                return;
            
            case Key.Enter:
                
                e.Handled = true;
                return;
        }

        if (TryGetDigit(e.Key, out int digit))
        {
            InsertDigit(box, digit);
            e.Handled = true;
        }
    }

    private void MoveNext(TextBox box)
    {
        if (box == TxtHour) TxtMinute.Focus();
        else if (box == TxtMinute) TxtSecond.Focus();
        // TxtSecond là ô cuối -> không làm gì
    }

    private void MovePrev(TextBox box)
    {
        if (box == TxtSecond) TxtMinute.Focus();
        else if (box == TxtMinute) TxtHour.Focus();
        // TxtHour là ô đầu -> không làm gì
    }

    #endregion

    #region Nhập số (xử lý ở KeyDown, trước khi TextBox kịp tự chèn ký tự)

    // Nhận diện phím số hàng trên cùng (D0-D9) và numpad (NumPad0-NumPad9)
    private static bool TryGetDigit(Key key, out int digit)
    {
        if (key >= Key.D0 && key <= Key.D9)
        {
            digit = key - Key.D0;
            return true;
        }
        if (key >= Key.NumPad0 && key <= Key.NumPad9)
        {
            digit = key - Key.NumPad0;
            return true;
        }
        digit = 0;
        return false;
    }

    // Ghi đè 1 chữ số vào đúng vị trí caret (0: chữ số hàng chục, 1: chữ số hàng đơn vị)
    private void InsertDigit(TextBox box, int digit)
    {
        string text = box.Text ?? "00";
        if (text.Length != 2) text = GetVal(box).ToString("00");

        int caret = box.CaretIndex;
        if (caret < 0) caret = 0;
        if (caret > 1) caret = 1; // chỉ có 2 vị trí hợp lệ: 0 và 1

        char[] chars = text.ToCharArray();
        chars[caret] = (char)('0' + digit);

        int newVal = int.Parse(new string(chars));
        int max = GetMax(box);
        if (newVal > max) newVal = max;

        SetVal(box, newVal);

        if (caret == 0)
        {
            // Vừa ghi vị trí đầu -> nhảy caret sang vị trí thứ 2 để ghi tiếp
            //box.CaretIndex = 1;
        }
        else
        {
            // Vừa ghi vị trí thứ 2 (đã đủ 2 số) -> tự nhảy sang ô kế tiếp
            //box.CaretIndex = 2;
            MoveNext(box);
        }
    }

    // Dự phòng cho Backspace / Delete / Paste / phím không mong muốn làm sai định dạng 2 chữ số
    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isUpdating) return;
        if (sender is not TextBox box) return;

        string raw = new string((box.Text ?? string.Empty).Where(char.IsDigit).ToArray());
        int max = GetMax(box);
        int v = string.IsNullOrEmpty(raw) ? 0 : Math.Min(int.Parse(raw), max);

        SetVal(box, v);
        box.CaretIndex = box.Text?.Length ?? 0;
    }

    #endregion

    #region Cuộn chuột (tăng/giảm, quay vòng, liên kết Hour-Minute-Second)

    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (sender is not TextBox box) return;

        int delta = e.Delta.Y > 0 ? 1 : -1;

        if (box == TxtHour) ChangeHour(delta);
        else if (box == TxtMinute) ChangeMinute(delta);
        else if (box == TxtSecond) ChangeSecond(delta);

        e.Handled = true;
    }

    // Hour: 0..23, quay vòng, không có cấp cao hơn để cộng dồn
    private void ChangeHour(int delta)
    {
        int v = GetVal(TxtHour) + delta;
        if (v > 23) v = 0;
        else if (v < 0) v = 23;
        SetVal(TxtHour, v);
    }

    // Minute: 0..59, quay vòng, tràn thì kéo Hour tăng/giảm theo
    private void ChangeMinute(int delta)
    {
        int v = GetVal(TxtMinute) + delta;
        if (v > 59) { v = 0; ChangeHour(1); }
        else if (v < 0) { v = 59; ChangeHour(-1); }
        SetVal(TxtMinute, v);
    }

    // Second: 0..59, quay vòng, tràn thì kéo Minute (và có thể kéo cả Hour theo dây chuyền)
    private void ChangeSecond(int delta)
    {
        int v = GetVal(TxtSecond) + delta;
        if (v > 59) { v = 0; ChangeMinute(1); }
        else if (v < 0) { v = 59; ChangeMinute(-1); }
        SetVal(TxtSecond, v);
    }

    #endregion

    #region Helpers

    private int GetMax(TextBox box) => box == TxtHour ? 23 : 59;

    private int GetVal(TextBox box) => int.TryParse(box.Text, out int v) ? v : 0;

    private void SetVal(TextBox box, int v)
    {
        _isUpdating = true;
        box.Text = v.ToString("00");
        _isUpdating = false;

        TimeChanged?.Invoke(this, Time);
    }

    #endregion
}