using System;

namespace CunningFox.LiveOps.Models;

public record LiveOpDto(Guid Id, string Schedule, TimeSpan Duration, string EventName, int EntryLevel);