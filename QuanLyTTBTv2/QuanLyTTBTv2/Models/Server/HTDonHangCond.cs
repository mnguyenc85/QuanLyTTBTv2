using System;

namespace QuanLyTTBTv2.Models.Server;

public class HTDonHangCond
{
    public bool Changed { get; set; } = true;

    private long _srcId;

    public long SourceId
    {
        get => _srcId;
        set
        {
            if (_srcId == value) return;
            _srcId = value;
            Changed = true;
        }
    }
    
    #region Thời gian
    private bool _useFrom, _useTo;
    public bool UseFrom {
        get => _useFrom;
        set {
            if (_useFrom == value) return;
            _useFrom = value;
            Changed = true;
        }
    }
    public bool UseTo {
        get => _useTo;
        set {
            if (_useTo == value) return;
            _useTo = value;
            Changed = true;
        }
    }

    private DateTime _fromTime, _toTime;
    public DateTime FromTime {
        get => _fromTime;
        set {
            if (_fromTime == value) return;
            _fromTime = value;
            Changed = true;
        }
    }
    public DateTime ToTime {
        get => _toTime;
        set {
            if (_toTime == value) return;
            _toTime = value;
            Changed = true;
        }
    }
    #endregion

    private string? _khachHang, _duAn;
    public string? KhachHang {
        get => _khachHang;
        set {
            if (_khachHang == value) return;
            _khachHang = value;
            Changed = true;
        }
    }
    public string? DuAn {
        get => _duAn;
        set {
            if (_duAn == value) return;
            _duAn = value;
            Changed = true;
        }
    }

    private int _limit = 10;
    /// <summary>
    /// Offet = page * Limit
    /// </summary>
    public int Offset { get; set; }
    /// <summary>
    /// Số ban ghi hiển thị
    /// </summary>
    public int Limit
    {
        get => _limit; 
        set 
        {
            if (_limit == value) return;
            _limit = value;
            Changed = true;
        }
    }
    
    /// <summary>
    /// Tổng số bản ghi theo điều kiện
    /// </summary>
    public int Total { get; set; }
}