using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Threading;

namespace LogLib
{
	public class Logger
	{

        public string version { get; set; } = "";
        public string user { get; set; } = "";

        public string logPath { get; set; } = "";

        //public bool _flag = false;
        //public bool flag
        //{
        //    get
        //    {
        //        return _flag;
        //    }
        //    set
        //    {
        //        if (_flag != value)
        //        {
        //            _flag = value;
        //            LogListChecker();
        //        }
        //    }
        //}

        public List<Log> _logs = new List<Log>();
        public List<Log> logs
        {
            get
			{
                return _logs;
			}
            set
			{
                _logs = value;
			}
        }

        /*
        //Создание потока для записывания логов в файл
        Thread loggerThread;
        */

        //Создание таска дл записывания логов в файл
        Task loggerTask;

        public Logger()
		{
            ////Готовим поток для записывания логов в файл
            //loggerThread = new Thread(() => LogListChecker());
            //Готовим таск для записывания логов в файл
            loggerTask = new Task(() => LogListChecker());
		}

        public Logger(string inVersion, string inUser, string inLogPath)
		{
            version = inVersion;
            user = inUser;
            logPath = inLogPath;

            ////Готовим поток для записывания логов в файл
            //loggerThread = new Thread(() => LogListChecker());
            //Готовим таск для записывания логов в файл
            loggerTask = new Task(() => LogListChecker());
        }

        public void LogListChecker()
		{
            Trace.WriteLine("Запуск Лог-чекера");
            while (logs.Count > 0)
			{
                Trace.WriteLine(Log.LogString(logs[0]));
                JSON.Serializer(logs[0], logPath, out string exceptionSerializer);
                if (!String.IsNullOrEmpty(exceptionSerializer))
                    Trace.WriteLine(exceptionSerializer);
                logs.RemoveAt(0);
                Task.Delay(500).Wait();
			}
		}

        public void NewLog(uint icode, string imessage)
        {
            try
            {
                string ilevel = "";
                switch (icode / 100)
                {
                    case 0:
                        ilevel = "Debug";
                        break;
                    /*
                    * Сообщения отладки, профилирования.
                    * В production системе обычно сообщения этого уровня включаются при первоначальном запуске системы или для поиска узких мест (bottleneck-ов).
                    */

                    case 1:
                        ilevel = "Info";
                        break;
                    /*
                    * Обычные сообщения, информирующие о действиях системы.
                    * Реагировать на такие сообщения вообще не надо, но они могут помочь, например, при поиске багов, расследовании интересных ситуаций итд.
                    */

                    case 2:
                        ilevel = "Warn";
                        break;
                    /*
                    * Записывая такое сообщение, система пытается привлечь внимание обслуживающего персонала.
                    * Произошло что-то странное. Возможно, это новый тип ситуации, ещё не известный системе.
                    * Следует разобраться в том, что произошло, что это означает, и отнести ситуацию либо к инфо-сообщению, либо к ошибке. Соответственно, придётся доработать код обработки таких ситуаций.
                    */

                    case 3:
                        ilevel = "Error";
                        break;
                    /*
                    * Ошибка в работе системы, требующая вмешательства. Что-то не сохранилось, что-то отвалилось. Необходимо принимать меры довольно быстро!
                    * Ошибки этого уровня и выше требуют немедленной записи в лог, чтобы ускорить реакцию на них.
                    * Нужно понимать, что ошибка пользователя – это не ошибка системы. Если пользователь ввёл в поле -1, где это не предполагалось – не надо писать об этом в лог ошибок.
                    */

                    case 4:
                        ilevel = "Fatal";
                        break;
                        /*
                         * это особый класс ошибок. Такие ошибки приводят к неработоспособности системы в целом, или неработоспособности одной из подсистем.
                         * Чаще всего случаются фатальные ошибки из-за неверной конфигурации или отказов оборудования. Требуют срочной, немедленной реакции.
                         * Возможно, следует предусмотреть уведомление о таких ошибках по SMS.
                         */
                }
                Log log = new Log
                {
                    level = ilevel,
                    dateTime = DateTime.Now,
                    version = version,
                    code = icode,
                    user = user,
                    message = imessage
                };
                Trace.WriteLine("+-----------------------------------------+");
                Trace.WriteLine("V                                         V");
                Trace.WriteLine(Log.LogString(log));
                //Task task = Task.Run(() => TestJsonLogClass.Serializer(log));

                Trace.WriteLine("Сериализация закончена");
                logs.Add(log);
                Trace.WriteLine("Лог добавлен в список");
                Trace.WriteLine("!                                         !");
                Trace.WriteLine("-------------------------------------------");

                /*
                //Уже устарело
                Trace.WriteLine("Состояние потока: " + loggerThread.ThreadState);
                if (loggerThread.ThreadState == System.Threading.ThreadState.Unstarted)
                    loggerThread.Start();
                if(loggerThread.ThreadState == System.Threading.ThreadState.Stopped)
				{
                    loggerThread = new Thread(() => LogListChecker());
                    loggerThread.Start();
                }
                */

                Trace.WriteLine("Состояния таска: " + loggerTask.Status);
                if (loggerTask.Status == TaskStatus.Created)
				{
                    loggerTask.Start();
                    Trace.WriteLine("Новое состояние таска: " + loggerTask.Status);
                }
                if(loggerTask.Status == TaskStatus.RanToCompletion || loggerTask.Status == TaskStatus.Faulted)
				{
                    loggerTask = new Task(() => LogListChecker());  
                    loggerTask.Start();
                    Trace.WriteLine("Новое состояние таска: " + loggerTask.Status);
                }

			}
            catch
            {

            }
        }
    }
}
