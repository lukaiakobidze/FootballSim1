using System;
using System.Collections.Generic;
using FootballManagerGame.Data;


namespace FootballManagerGame.Models;

public class Goal{

    public int TimeScored { get; set; }
    public Player PlayerScored { get; set; }
    public Player PlayerAssisted { get; set; }
    public bool IsPenalty { get; set; }
    
}