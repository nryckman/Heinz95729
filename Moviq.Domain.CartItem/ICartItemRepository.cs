﻿using Moviq.Interfaces.Models;
using Moviq.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.CartItem
{
    /// <summary>
    /// Extends IRepository<IUser> to add specific repo queries
    /// </summary>
    public interface ICartItemRepository : IRepository<ICartItem>
    {
        /// <summary>
        /// Get a user by their username, instead of by GUID
        /// </summary>
        /// <param name="username">the user's username</param>
        /// <returns>The user that matches the username, if any</returns>
        void GetAllCarts();
    }
}
