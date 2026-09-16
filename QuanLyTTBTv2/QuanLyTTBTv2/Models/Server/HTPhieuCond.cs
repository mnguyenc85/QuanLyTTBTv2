using System;

namespace QuanLyTTBTv2.Models.Server;

public class HTPhieuCond
{
    public bool Changed  { get; set; } = true;

    private long _srcId, _dhId;

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

    public long DonHangId
    {
        get => _dhId;
        set
        {
            if (_dhId == value) return;
            _dhId = value;
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
    
    private string? _xe, _laixe, _ct;
    public string? Xe {
        get => _xe;
        set {
            if (_xe == value) return;
            _xe = value;
            Changed = true;
        }
    }
    public string? LaiXe {
        get => _laixe;
        set {
            if (_laixe == value) return;
            _laixe = value;
            Changed = true;
        }
    }
    public string? CongThuc {
        get => _ct;
        set {
            if (_ct == value) return;
            _ct = value;
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