using System;
using System.Collections.Generic;
using ChatApp.Application.Common.Models;

namespace ChatApp.Infrastructure.Tests.Comparers;

public class CurrentUserModelComparer : IEqualityComparer<CurrentUserModel>
{
    public bool Equals(CurrentUserModel x, CurrentUserModel y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        return x.UserName == y.UserName &&
               x.Email == y.Email &&
               x.FirstName == y.FirstName &&
               x.LastName == y.LastName &&
               x.About == y.About &&
               x.ProfileImage?.Url == y.ProfileImage?.Url &&
               x.BackgroundImage?.Url == y.BackgroundImage?.Url;
    }

    public int GetHashCode(CurrentUserModel model)
    {
        return model.UserName.GetHashCode();
    }
}