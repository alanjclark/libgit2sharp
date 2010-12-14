﻿using System;
using NUnit.Framework;

namespace libgit2sharp.Tests
{
    [TestFixture]
    public class ApplyingATag : ReadWriteRepositoryFixtureBase
    {
        private static readonly Signature _signature = new Signature("me", "me@me.me", DateTimeOffset.Now);

        [Test]
        public void ShouldThrowIfPassedANonExistingTarget()
        {
            const string invalidTargetId = "deadbeef1b46c854b31185ea97743be6a8774479";

            using (var repo = new Repository(PathToRepository))
            {
                Assert.Throws<ObjectNotFoundException>(() => repo.ApplyTag(invalidTargetId, "tagged", "messaged", _signature));
            }

        }

        [Test]
        public void ShouldWork() // TODO: Split into different tests (returnATag, PersistTheObject, MultipleApplies, ...)
        {
            const string targetId = "8496071c1b46c854b31185ea97743be6a8774479";

            Tag appliedTag;
            using (var repo = new Repository(PathToRepository))
            {
                appliedTag = repo.ApplyTag(targetId, "tagged", "messaged", _signature);
            }

            var target = appliedTag.Target as Commit;
            Assert.IsNotNull(target);

            Assert.IsNotNull(target.Author);
            Assert.IsNotNull(target.Committer);
            Assert.IsNotNull(target.Message);

            Tag retrievedTag;
            using (var repo = new Repository(PathToRepository))
            {
                retrievedTag = repo.Resolve<Tag>(appliedTag.Id);
            }

            var target2 = retrievedTag.Target as Commit;
            Assert.IsNotNull(target2);

            Assert.IsNotNull(target2.Author);
            Assert.IsNotNull(target2.Committer);
            Assert.IsNotNull(target2.Message);


            Assert.AreEqual(appliedTag.Id, retrievedTag.Id);
            // TODO: Finalize comparison

            //
        }
    }
}