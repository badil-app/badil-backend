﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Badil.Backend.Data.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public virtual ICollection<UserReview> UserReviews { get; set; } = new List<UserReview>();
}