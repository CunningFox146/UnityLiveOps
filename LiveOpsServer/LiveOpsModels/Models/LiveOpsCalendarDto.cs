using System;
using System.Collections.Generic;

namespace CunningFox.LiveOps.Models;


public record LiveOpsCalendarDto(Guid Id, long ServerTime, List<LiveOpDto> Events);