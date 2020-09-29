using System;
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
            var repo = LogManager.GetRepository(typeof(TestMemoryAppender).Assembly);
            var appender = new MemoryAppender();
            BasicConfigurator.Configure(repo, appender);

            var logger = LogManager.GetLogger(typeof(TestMemoryAppender));
            logger.Logger.Log(new LoggingEvent(
                new LoggingEventData()
                {
                    TimeStampUtc = DateTime.UtcNow,
                    LoggerName = "Logger",
                    ThreadName = Thread.CurrentThread.ManagedThreadId.ToString(),
                    Level = Level.Info,
                    Message = "Hello",
                    Properties = new PropertiesDictionary()
                })
            );
            
            var events = appender.GetEvents();
            
            Expect(events)
                .To.Contain.Only(1).Item();
        }
    }
}