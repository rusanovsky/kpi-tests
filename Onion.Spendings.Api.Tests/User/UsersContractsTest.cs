using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using onion_spendings.Record;
using Spendings.Core.Record;
using System.Collections.Generic;

namespace Onion.Spendings.Api.Tests.User
{

    public class UsersContractsTest
    {

        [Fact]
        public async Task CreatRecord_IfThrowsException_ReturnNothing()
        {
            // Arrange

            try
            {
                global::Spendings.Orchrestrators.User.Users tooLongLogin = new global::Spendings.Orchrestrators.User.Users()
                {
                    Login = new string('s', 31),
                    Password = new string('s',10)
                };
                Assert.Equal(1, 0);
            }
            catch
            {}

            try
            {
                global::Spendings.Orchrestrators.User.Users tooShortLogin= new global::Spendings.Orchrestrators.User.Users()
                {
                    Login = new string('s', 2),
                    Password = new string('s', 10)
                };
                Assert.Equal(1, 0);
            }
            catch
            {}

            try
            {
                global::Spendings.Orchrestrators.User.Users tooLongPassword = new global::Spendings.Orchrestrators.User.Users()
                {
                    Login = new string('s', 10),
                    Password = new string('s', 31)
                };
                Assert.Equal(1, 0);
            }
            catch
            {}

            try
            {
                global::Spendings.Orchrestrators.User.Users tooShortPassword = new global::Spendings.Orchrestrators.User.Users()
                {
                    Login = new string('s', 10),
                    Password = new string('s', 2)
                };
                Assert.Equal(1, 0);
            }
            catch
            { }

        }
    }
}
