using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using WestMarchSite.Application;
using WestMarchSite.Core;
using WestMarchSite.Infrastructure;

namespace WestMarchSite.Tests
{
    [TestClass]
    public class SessionServiceTests
    {
        [TestMethod]
        public void CreateSession()
        {
            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult());

            var app = new SessionService(repo.Object);

            var result = app.StartSession(new CreateSessionDto
            {
                Description = "desc",
                Name = "name",
                Title = "title"
            });

            Assert.IsTrue(result.IsSuccess);
            repo.Verify(r => r.Save(It.Is<SessionEntity>(e => e.IsValid == true && e.SessionState == SessionStates.UnApproved)), "saved entity wasn't valid");
        }

        [TestMethod]
        [DataRow("something", "something", null)]
        [DataRow("something", "something", "")]
        [DataRow("something", null, "something")]
        [DataRow("something", "", "something")]
        [DataRow(null, "something", "something")]
        [DataRow("", "something", "something")]
        [DataRow("", "", "")]
        [DataRow(null, null, null)]
        public void CreateSessionInvalid(string description, string name, string title)
        {
            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult());
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult(SessionRepository.UpdateResultErrors.Technical));

            var app = new SessionService(repo.Object);

            var result = app.StartSession(new CreateSessionDto
            {
                Description = description,
                Name = name,
                Title = title
            });

            Assert.AreEqual(result.Error, SessionService.SetResultErrors.InvalidInput, "all inputs should be invalid");
        }

        [TestMethod]
        public void HostApproveSession()
        {
            var session = new SessionEntity();
            session.SetInfo("title", "descrip");
            session.SetLead("sean");
            session.ProgressState();
            Assert.AreEqual(session.SessionState, SessionStates.UnApproved);
            Assert.IsTrue(session.IsValid);

            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.GetSessionHostKey("hostKey"))
                .Returns(new SessionRepository.QueryResult<SessionEntity>(session));
            repo.Setup(r => r.Save(It.Is<SessionEntity>(e => e.IsValid)))
                .Returns(new SessionRepository.UpdateResult());

            var app = new SessionService(repo.Object);

            var result = app.HostApproveSession("hostKey", new ApproveSessionDto
            {
                Name = "hostname",
                Schedule = new SessionScheduleDateDto[]
                {
                    new SessionScheduleDateDto()
                    {
                        End = DateTime.Now,
                        Start = DateTime.Now
                    }
                }
            });


            Assert.IsTrue(result.IsSuccess);
            repo.Verify(r => r.Save(It.Is<SessionEntity>(e => e.IsValid && e.SessionState == SessionStates.Approved)), "didn't call save or was invalid");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void HostApproveSessionInvalidName(string hostname)
        {
            var session = new SessionEntity();
            session.SetInfo("title", "descrip");
            session.SetLead("sean");
            session.ProgressState();
            Assert.AreEqual(session.SessionState, SessionStates.UnApproved);
            Assert.IsTrue(session.IsValid);

            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.GetSessionHostKey("hostKey"))
                .Returns(new SessionRepository.QueryResult<SessionEntity>(session));
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult(SessionRepository.UpdateResultErrors.Technical));

            var app = new SessionService(repo.Object);

            var result = app.HostApproveSession("hostKey", new ApproveSessionDto
            {
                Name = hostname,
                Schedule = new SessionScheduleDateDto[]
                {
                    new SessionScheduleDateDto()
                    {
                        End = DateTime.Now,
                        Start = DateTime.Now
                    }
                }
            });

            Assert.AreEqual(result.Error, SessionService.SetResultErrors.InvalidInput, "should be invalid");
        }

        [TestMethod]
        public void HostApproveSessionInvalidDate()
        {
            var session = new SessionEntity();
            session.SetInfo("title", "descrip");
            session.SetLead("sean");
            session.ProgressState();
            Assert.AreEqual(session.SessionState, SessionStates.UnApproved);
            Assert.IsTrue(session.IsValid);

            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.GetSessionHostKey("hostKey"))
                .Returns(new SessionRepository.QueryResult<SessionEntity>(session));
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult(SessionRepository.UpdateResultErrors.Technical));

            var app = new SessionService(repo.Object);

            var result = app.HostApproveSession("hostKey", new ApproveSessionDto
            {
                Name = "hostname",
                Schedule = new SessionScheduleDateDto[]
                { }
            });

            Assert.AreEqual(result.Error, SessionService.SetResultErrors.InvalidInput, "should be invalid");
        }

        [TestMethod]
        public void LeadNarrowsSchedule()
        {
            var session = new SessionEntity();
            session.SetInfo("title", "descrip");
            session.SetLead("lead");
            session.ProgressState();
            session.SetHost("host");
            session.SetHostSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            Assert.AreEqual(session.SessionState, SessionStates.Approved);
            Assert.IsTrue(session.IsValid);

            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.GetSessionLeadKey("leadKey"))
                .Returns(new SessionRepository.QueryResult<SessionEntity>(session));
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult());

            var app = new SessionService(repo.Object);

            var result = app.LeadNarrowsSchedule("leadKey", new LeadScheduleDto()
            {
                Schedule = new SessionScheduleDateDto[] { new SessionScheduleDateDto { End = DateTime.Now, Start = DateTime.Now } },
            });

            Assert.IsTrue(result.IsSuccess);
            repo.Verify(r => r.Save(It.Is<SessionEntity>(e => e.IsValid && e.SessionState == SessionStates.Open)), "didn't call save or was invalid");
        }

        [TestMethod]
        public void LeadNarrowsScheduleInvalidSchedule()
        {
            var session = new SessionEntity();
            session.SetInfo("title", "descrip");
            session.SetLead("lead");
            session.ProgressState();
            session.SetHost("host");
            session.SetHostSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            Assert.AreEqual(session.SessionState, SessionStates.Approved);
            Assert.IsTrue(session.IsValid);

            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.GetSessionLeadKey("leadKey"))
                .Returns(new SessionRepository.QueryResult<SessionEntity>(session));
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult(SessionRepository.UpdateResultErrors.Technical));

            var app = new SessionService(repo.Object);

            var result = app.LeadNarrowsSchedule("leadKey", new LeadScheduleDto()
            {
                Schedule = new SessionScheduleDateDto[] { }
            });

            Assert.AreEqual(result.Error, SessionService.SetResultErrors.InvalidInput, "should be invalid");
        }


        [TestMethod]
        public void PlayerJoinSession()
        {
            var session = new SessionEntity();
            session.SetInfo("title", "descrip");
            session.SetLead("lead");
            session.ProgressState();
            session.SetHost("host");
            session.SetHostSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            session.SetLeadSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            Assert.AreEqual(session.SessionState, SessionStates.Open);
            Assert.IsTrue(session.IsValid);

            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.GetSessionPlayerKey("playerKey"))
                .Returns(new SessionRepository.QueryResult<SessionEntity>(session));
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult());

            var app = new SessionService(repo.Object);

            var result = app.PlayerJoinSession("playerKey", new PlayerJoinDto
            {
                Name = "playername",
                Schedule = new SessionScheduleDateDto[] { new SessionScheduleDateDto { End = DateTime.Now, Start = DateTime.Now } }
            });

            Assert.IsTrue(result.IsSuccess);
            repo.Verify(r => r.Save(It.Is<SessionEntity>(e => e.IsValid && e.SessionState == SessionStates.Open)), "didn't call save or was invalid");
        }

        [TestMethod]
        public void PlayerJoinSessionInvalidName()
        {
            var session = new SessionEntity();
            session.SetInfo("title", "descrip");
            session.SetLead("lead");
            session.ProgressState();
            session.SetHost("host");
            session.SetHostSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            session.SetLeadSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            Assert.AreEqual(session.SessionState, SessionStates.Open);
            Assert.IsTrue(session.IsValid);

            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.GetSessionPlayerKey("playerKey"))
                .Returns(new SessionRepository.QueryResult<SessionEntity>(session));
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult(SessionRepository.UpdateResultErrors.Technical));

            var app = new SessionService(repo.Object);

            var result = app.PlayerJoinSession("playerKey", new PlayerJoinDto
            {
                Name = "",
                Schedule = new SessionScheduleDateDto[] { new SessionScheduleDateDto { End = DateTime.Now, Start = DateTime.Now } }
            });

            Assert.AreEqual(result.Error, SessionService.SetResultErrors.InvalidInput, "should be invalid");
        }

        [TestMethod]
        public void PlayerJoinSessionInvalidSchedule()
        {
            var session = new SessionEntity();
            session.SetInfo("title", "descrip");
            session.SetLead("lead");
            session.ProgressState();
            session.SetHost("host");
            session.SetHostSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            session.SetLeadSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            Assert.AreEqual(session.SessionState, SessionStates.Open);
            Assert.IsTrue(session.IsValid);

            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.GetSessionPlayerKey("playerKey"))
                .Returns(new SessionRepository.QueryResult<SessionEntity>(session));
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult(SessionRepository.UpdateResultErrors.Technical));

            var app = new SessionService(repo.Object);

            var result = app.PlayerJoinSession("playerKey", new PlayerJoinDto
            {
                Name = "playername",
                Schedule = new SessionScheduleDateDto[] { }
            });

            Assert.AreEqual(result.Error, SessionService.SetResultErrors.InvalidInput, "should be invalid");
        }

        [TestMethod]
        public void HostFinalizes()
        {
            var session = new SessionEntity();
            session.SetInfo("title", "descrip");
            session.SetLead("lead");
            session.ProgressState();
            session.SetHost("host");
            session.SetHostSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            session.SetLeadSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            Assert.AreEqual(session.SessionState, SessionStates.Open);
            Assert.IsTrue(session.IsValid);

            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.GetSessionHostKey("hostKey"))
                .Returns(new SessionRepository.QueryResult<SessionEntity>(session));
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult());

            var app = new SessionService(repo.Object);

            var result = app.HostFinalizes("hostKey", new HostFinalizeDto
            {
                Schedule = new SessionScheduleDateDto[] { new SessionScheduleDateDto { End = DateTime.Now, Start = DateTime.Now } }
            });

            Assert.IsTrue(result.IsSuccess);
            repo.Verify(r => r.Save(It.Is<SessionEntity>(e => e.IsValid && e.SessionState == SessionStates.Finalized)), "didn't call save or was invalid");
        }

        [TestMethod]
        public void HostFinalizesInvalidSchedule()
        {
            var session = new SessionEntity();
            session.SetInfo("title", "descrip");
            session.SetLead("lead");
            session.ProgressState();
            session.SetHost("host");
            session.SetHostSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            session.SetLeadSchedule(new SessionSchedule(new SessionScheduleOption[] { new SessionScheduleOption(DateTime.Now, DateTime.Now) }));
            session.ProgressState();
            Assert.AreEqual(session.SessionState, SessionStates.Open);
            Assert.IsTrue(session.IsValid);

            var repo = new Mock<ISessionRepository>();
            repo.Setup(r => r.GetSessionHostKey("hostKey"))
                .Returns(new SessionRepository.QueryResult<SessionEntity>(session));
            repo.Setup(r => r.Save(It.IsAny<SessionEntity>()))
                .Returns(new SessionRepository.UpdateResult(SessionRepository.UpdateResultErrors.Technical));

            var app = new SessionService(repo.Object);

            var result = app.HostFinalizes("hostKey", new HostFinalizeDto
            {
                Schedule = new SessionScheduleDateDto[] { }
            });

            Assert.AreEqual(result.Error, SessionService.SetResultErrors.InvalidInput, "should be invalid");
        }
    }
}
