using System;
using System.Linq;
using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Util;
using NUnit.Framework;
using static NExpect.Expectations;
using NExpect;

namespace log4net_consumer
{
    [TestFixture]
    public class TestMemoryAppender
    {
        [Test]
        public void ShouldLog()
        {
            // Arrange
            var repo = LogManager.GetRepository(typeof(TestMemoryAppender).Assembly);
            var appender = new MemoryAppender();
            BasicConfigurator.Configure(repo, appender);

            var logger = LogManager.GetLogger(typeof(TestMemoryAppender));
            var data = new LoggingEventData()
            {
                TimeStampUtc = DateTime.UtcNow,
                LoggerName = "Logger",
                ThreadName = Thread.CurrentThread.ManagedThreadId.ToString(),
                Level = Level.Info,
                Message = "Hello",
                Properties = new PropertiesDictionary()
            };
            var ev = new LoggingEvent(data);
            
            // Act
            logger.Logger.Log(ev);

            // Assert
            var events = appender.GetEvents();

            Expect(events)
                .To.Contain.Only(1).Item();
            var captured = events.Single().GetLoggingEventData();
            Expect(captured.Level)
                .To.Equal(Level.Info);
            Expect(captured.Message)
                .To.Equal(data.Message);
            Expect(captured.LoggerName)
                .To.Equal(data.LoggerName);
        }
    }
}