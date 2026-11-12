using System.Diagnostics;

namespace MawWww.Models;

public sealed class WriteLimitExceededException(string message)
    : Exception(message);

// a write-only wrapper that caps how much can be written and for how long.  this
// exists for tools that run visitor supplied code (xslt) whose only observable
// progress is the output it produces - the limits are checked on every write, so
// they bound runaway output and long running work that keeps writing, but cannot
// interrupt work that never writes.
//
// once a limit trips, the first write throws and later writes are dropped, so the
// flush/close that follows in the caller's cleanup does not throw a second time.
public sealed class LimitedWriteStream(Stream inner, long maxBytes, TimeSpan maxDuration)
    : Stream
{
    readonly Stream _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    readonly long _maxBytes = maxBytes;
    readonly TimeSpan _maxDuration = maxDuration;
    readonly long _startTimestamp = Stopwatch.GetTimestamp();
    long _written;
    bool _tripped;

    public override bool CanRead => false;
    public override bool CanSeek => false;
    public override bool CanWrite => true;
    public override long Length => throw new NotSupportedException();
    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        Write(buffer.AsSpan(offset, count));
    }

    public override void Write(ReadOnlySpan<byte> buffer)
    {
        if (_tripped)
        {
            return;
        }

        if (_written + buffer.Length > _maxBytes)
        {
            _tripped = true;
            throw new WriteLimitExceededException($"Output exceeded the limit of {_maxBytes:N0} bytes.");
        }

        if (Stopwatch.GetElapsedTime(_startTimestamp) > _maxDuration)
        {
            _tripped = true;
            throw new WriteLimitExceededException($"Processing exceeded the limit of {_maxDuration.TotalSeconds} second(s).");
        }

        _inner.Write(buffer);
        _written += buffer.Length;
    }

    public override void Flush()
    {
        _inner.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
}
