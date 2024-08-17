/*
 * Dakoda Meade
 * CST-350
 * Activity 6-2
 * Logger class
 * 
 */
using NLog;

namespace OCDD.Services.Utility
{
	public class MyLogger : ILogger
	{
		// Singleton design pattern
		private static MyLogger instance;
		private static Logger logger;

		public static MyLogger GetInstance()
		{
			if (instance == null)
			{
				instance = new MyLogger();
			}
			return instance;
		}

		private Logger GetLogger()
		{
			if (logger == null)
			{
				logger = LogManager.GetLogger("LoginAppLoggerrule");
			}
			return logger;
		}


		public void Debug(string message)
		{
			GetLogger().Debug(message);
		}

		public void Error(string message)
		{
			GetLogger().Error(message);
		}

		public void Info(string message)
		{
			GetLogger().Info(message);
		}

		public void Warning(string message)
		{
			GetLogger().Warn(message);
		}
	}
}
