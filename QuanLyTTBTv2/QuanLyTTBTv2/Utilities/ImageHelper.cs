using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.Runtime.InteropServices;

namespace QuanLyTTBTv2.Utilities;

public static class ImageHelper
{
    public static Bitmap Invert(Bitmap source)
    {
        var size = source.PixelSize;
        var dpi = source.Dpi;

        var writeable = new WriteableBitmap(
            size,
            dpi,
            PixelFormat.Bgra8888,
            AlphaFormat.Unpremul);

        using var locked = writeable.Lock();
        var stride = locked.RowBytes;
        var bufferSize = size.Height * stride;
        var buffer = new byte[bufferSize];

        // Copy từ source vào buffer (dùng GCHandle để lấy IntPtr)
        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        try
        {
            source.CopyPixels(
                new PixelRect(0, 0, size.Width, size.Height),
                handle.AddrOfPinnedObject(),
                bufferSize,
                stride);
        }
        finally
        {
            handle.Free();
        }

        // Invert RGB, giữ Alpha
        for (int i = 0; i < buffer.Length; i += 4)
        {
            buffer[i]     = (byte)(255 - buffer[i]);     // B
            buffer[i + 1] = (byte)(255 - buffer[i + 1]); // G
            buffer[i + 2] = (byte)(255 - buffer[i + 2]); // R
            // Alpha (i+3) giữ nguyên
        }

        // Ghi buffer đã invert vào WriteableBitmap
        Marshal.Copy(buffer, 0, locked.Address, buffer.Length);

        return writeable;
    }
}