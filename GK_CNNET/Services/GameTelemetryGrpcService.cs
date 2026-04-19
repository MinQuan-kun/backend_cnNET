using Grpc.Core;
using System.Collections.Concurrent;

namespace GK_CNNET.Services
{
    public class GameTelemetryGrpcService : GameTelemetryGrpc.GameTelemetryGrpcBase
    {
        private static readonly ConcurrentDictionary<string, TelemetryAccumulator> Sessions = new();

        public override Task<TelemetryAck> LogEvent(TelemetryEventRequest request, ServerCallContext context)
        {
            var sessionId = string.IsNullOrWhiteSpace(request.SessionId)
                ? $"session_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}"
                : request.SessionId;

            var acc = Sessions.GetOrAdd(sessionId, _ => new TelemetryAccumulator(sessionId));
            acc.Record(request);

            Console.WriteLine($"[gRPC-telemetry] session={sessionId} type={request.EventType} latency={request.LatencyMs:0.00}ms");

            return Task.FromResult(new TelemetryAck
            {
                Success = true,
                Message = "Telemetry event received"
            });
        }

        public override Task<TelemetrySummaryResponse> GetSessionSummary(TelemetrySummaryRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.SessionId) || !Sessions.TryGetValue(request.SessionId, out var acc))
            {
                return Task.FromResult(new TelemetrySummaryResponse
                {
                    SessionId = request.SessionId ?? string.Empty,
                    TotalEvents = 0,
                    BounceEvents = 0,
                    SkillEvents = 0,
                    EndEvents = 0,
                    AvgLatencyMs = 0,
                    MinLatencyMs = 0,
                    MaxLatencyMs = 0,
                    EventsPerSecond = 0,
                    FirstEventMs = 0,
                    LastEventMs = 0,
                });
            }

            return Task.FromResult(acc.ToResponse());
        }

        private sealed class TelemetryAccumulator
        {
            private readonly string _sessionId;
            private long _totalEvents;
            private long _bounceEvents;
            private long _skillEvents;
            private long _endEvents;
            private double _latencySum;
            private double _minLatency = double.MaxValue;
            private double _maxLatency;
            private long _firstEventMs;
            private long _lastEventMs;
            private readonly object _lock = new();

            public TelemetryAccumulator(string sessionId)
            {
                _sessionId = sessionId;
            }

            public void Record(TelemetryEventRequest request)
            {
                lock (_lock)
                {
                    _totalEvents++;
                    if (request.EventType == "BOUNCE") _bounceEvents++;
                    if (request.EventType == "SKILL") _skillEvents++;
                    if (request.EventType == "MATCH_END") _endEvents++;

                    if (request.LatencyMs > 0)
                    {
                        _latencySum += request.LatencyMs;
                        _minLatency = Math.Min(_minLatency, request.LatencyMs);
                        _maxLatency = Math.Max(_maxLatency, request.LatencyMs);
                    }

                    if (_firstEventMs == 0 || request.TimestampMs < _firstEventMs)
                    {
                        _firstEventMs = request.TimestampMs;
                    }

                    if (request.TimestampMs > _lastEventMs)
                    {
                        _lastEventMs = request.TimestampMs;
                    }
                }
            }

            public TelemetrySummaryResponse ToResponse()
            {
                lock (_lock)
                {
                    var durationMs = Math.Max(1, _lastEventMs - _firstEventMs);
                    var eventsPerSecond = _totalEvents / (durationMs / 1000.0);
                    var avgLatency = _totalEvents > 0 ? _latencySum / _totalEvents : 0;

                    return new TelemetrySummaryResponse
                    {
                        SessionId = _sessionId,
                        TotalEvents = (int)_totalEvents,
                        BounceEvents = (int)_bounceEvents,
                        SkillEvents = (int)_skillEvents,
                        EndEvents = (int)_endEvents,
                        AvgLatencyMs = avgLatency,
                        MinLatencyMs = _minLatency == double.MaxValue ? 0 : _minLatency,
                        MaxLatencyMs = _maxLatency,
                        EventsPerSecond = eventsPerSecond,
                        FirstEventMs = _firstEventMs,
                        LastEventMs = _lastEventMs,
                    };
                }
            }
        }
    }
}
