using System;
using System.Collections.Generic;

namespace CunningFox.Models;

public record LiveOpCalendarDto(Guid Id, long ServerTime, List<LiveOpDto> Events);