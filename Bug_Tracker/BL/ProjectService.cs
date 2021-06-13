﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;

namespace Bug_Tracker.BL
{
    public class ProjectService
    {
        public ProjectRepo repo;

        public void Create(Project project)
        {
            repo.Add(project);
        }
    }
}