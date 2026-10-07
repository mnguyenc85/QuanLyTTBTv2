using System.Collections.Generic;

namespace QuanLyTTBTv2.Models.Local;

public class DbInPhieuCond
{
    public bool Changed { get; set; } = true;

    private long _dhId, _phId;

    public long DonHangId
    {
        get => _dhId; 
        set { if (_dhId != value) { _dhId = value; Changed = true; } }
    }

    public long PhieuId
    {
        get => _phId; 
        set { if (_phId != value) { _phId = value; Changed = true; } }
    }
    public List<long> PhieuIds { get; } = [];

    #region Offet, Limit, Total
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
    #endregion
}