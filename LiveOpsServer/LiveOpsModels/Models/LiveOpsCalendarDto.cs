using System;
using System.Collections.Generic;
using NCrontab;

namespace CunningFox.LiveOps.Models;


public record LiveOpsCalendarDto(Guid Id, long ServerTime, List<LiveOpDto> Events);