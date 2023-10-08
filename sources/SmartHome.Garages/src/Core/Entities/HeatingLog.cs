﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Common;

namespace SmartHome.Core.Entities;

[Table("HeatingLogs", Schema = "Garages")]
public partial class HeatingLog: IEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Info { get; set; }
}
