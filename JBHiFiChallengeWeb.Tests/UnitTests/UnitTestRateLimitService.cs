using System;
using System.Collections.Generic;
using System.Data.Entity;
using JBHiFiChallengeWebAPI.Entities;
using JBHiFiChallengeWebAPI.ServiceContracts;
using JBHiFiChallengeWebAPI.ServiceImplementations;
using Xunit;

namespace JBHiFiChallengeWeb.Tests.UnitTests
{
    public class UnitTestRateLimitService
    {
        IDbSet<KeyUsage> keyUsageDbSet;
        IRateLimitService rateLimitService;

        public UnitTestRateLimitService()
        {
            keyUsageDbSet = Helpers.MockDbSet.CreateMockDbSet(new List<KeyUsage>()).Object;
            rateLimitService = new RateLimitService();
        }

        [Fact]
        public void TestAdd5ItemsForSameKey()
        {
            string keyName = "Key1";

            bool addedSuccessfully = true;
            for (int count = 0; count < 5; count++)
            {
                addedSuccessfully = rateLimitService.AddKeyUsage(keyUsageDbSet, keyName);
                if (!addedSuccessfully)
                {
                    break;
                }
            }

            Assert.True(addedSuccessfully);
        }

        [Fact]
        public void TestAdd5ItemsForMultipleKeys()
        {
            string key1Name = "Key1";
            string key2Name = "Key2";

            bool addedSuccessfully = true;
            for (int count = 0; count < 5; count++)
            {
                addedSuccessfully = rateLimitService.AddKeyUsage(keyUsageDbSet, key1Name);
                if (!addedSuccessfully)
                {
                    break;
                }

                addedSuccessfully = rateLimitService.AddKeyUsage(keyUsageDbSet, key2Name);
                if (!addedSuccessfully)
                {
                    break;
                }
            }

            Assert.True(addedSuccessfully);
        }

        [Fact]
        public void TestAdd6ItemsShouldResultInTooManyItems()
        {
            string keyName = "Key1";

            bool addedSuccessfully = true;
            for (int count = 0; count < 6; count++)
            {
                addedSuccessfully = rateLimitService.AddKeyUsage(keyUsageDbSet, keyName);
                if (!addedSuccessfully)
                {
                    break;
                }
            }

            Assert.False(addedSuccessfully);
        }

        [Fact]
        public void TestAdd5EntriesAfterAnExpiredEntry()
        {
            string keyName = "Key1";
            rateLimitService.AddKeyUsage(keyUsageDbSet, keyName, DateTime.UtcNow.AddHours(-1));

            bool addedSuccessfully = true;
            for (int count = 0; count < 5; count++)
            {
                addedSuccessfully = rateLimitService.AddKeyUsage(keyUsageDbSet, keyName);
                if (!addedSuccessfully)
                {
                    break;
                }
            }

            Assert.True(addedSuccessfully);
        }
    }
}
