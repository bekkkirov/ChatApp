﻿namespace ChatApp.Application.Common.Models;

public class CurrentUserModel
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string About { get; set; }

    public ImageModel ProfileImage { get; set; }

    public ImageModel BackgroundImage { get; set; }

}