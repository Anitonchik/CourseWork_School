﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabase.Models;

public class Worker
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FIO { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Mail { get; set; }
    [ForeignKey("WorkerId")]
    public List<Interest>? Interests { get; set; }

    /*[ForeignKey("WorkerId")]
    public List<Lesson>? Lessons { get; set; }*/

    [ForeignKey("WorkerId")]
    public List<Achievement>? Achievements { get; set; }
}
