using AstronautHealthMonitoring.DTOs.Chat;
using AstronautHealthMonitoring.DTOs.Mood;
using AstronautHealthMonitoring.DTOs.Sleep;
using AstronautHealthMonitoring.DTOs.VitalSigns;

namespace AstronautHealthMonitoring.DTOs.Sync;

public class SyncBatchRequest
{
    public int AstronautId { get; set; }
    public DateTime DeviceSyncTime { get; set; }

    public List<CreateVitalSignReadingRequest> VitalSignReadings { get; set; } = new();
    public List<CreateSleepRecordRequest> SleepRecords { get; set; } = new();
    public List<CreateMoodEntryRequest> MoodEntries { get; set; } = new();
    public List<SendChatMessageRequest> ChatMessages { get; set; } = new();
}

public class SyncBatchResponse
{
    public bool Success { get; set; }
    public int VitalSignReadingsProcessed { get; set; }
    public int SleepRecordsProcessed { get; set; }
    public int MoodEntriesProcessed { get; set; }
    public int ChatMessagesProcessed { get; set; }
    public int DuplicatesIgnoredCount { get; set; }
    public DateTime ServerSyncedAt { get; set; }
    public List<string> Errors { get; set; } = new();
}
