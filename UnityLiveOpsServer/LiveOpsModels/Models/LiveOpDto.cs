using System;

namespace CunningFox.LiveOps.Models;

public record LiveOpDto(string Id, string Schedule, TimeSpan Duration, string EventName, int EntryLevel);