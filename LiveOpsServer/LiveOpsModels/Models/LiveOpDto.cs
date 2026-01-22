using System;

namespace CunningFox.Models;

public record LiveOpDto(Guid Id, DateTime Start, DateTime Finish, string BundleName, int EntryLevel);