using System;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Tests {
    public static class FakeUsers {
        public static UserProfile fakeUser = new UserProfile() {
            UID = Guid.NewGuid(),
            name = @"單元測試_教師",
            identity = "Editor",
            email = "fake.oneclubAccount@gmail.com",
            account = "fakeEditor",
            organization = new AttributeMap("NanI",
            "南一書局",
            "Agency"
            ),
            status = "Active",
            lastLogin = DateTime.Now,
        };

    }
}